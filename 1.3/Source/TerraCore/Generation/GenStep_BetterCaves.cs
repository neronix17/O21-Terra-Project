using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace TerraCore
{
	public class GenStep_BetterCaves : GenStep
	{
		private enum BranchType
		{
			Normal,
			Room,
			Tunnel
		}

		private const float DirectionNoiseFrequency = 0.00205f;

		private const float TunnelWidthNoiseFrequency = 0.02f;

		private static readonly SimpleCurve TunnelsWidthPerRockCount = new SimpleCurve
	{
		new CurvePoint(300f, 2.5f),
		new CurvePoint(600f, 4.2f),
		new CurvePoint(6000f, 9.5f),
		new CurvePoint(30000f, 15.5f)
	};

		private ModExt_Biome_GenStep_BetterCaves extCaves;

		private ModuleBase directionNoise;

		private ModuleBase tunnelWidthNoise;

		private static HashSet<IntVec3> groupSet = new HashSet<IntVec3>();

		private static HashSet<IntVec3> groupVisited = new HashSet<IntVec3>();

		private static List<IntVec3> subGroup = new List<IntVec3>();

		private static List<IntVec3> tmpCells = new List<IntVec3>();

		private static HashSet<IntVec3> tmpGroupSet = new HashSet<IntVec3>();

		public override int SeedPart => 235321433;

		public override void Generate(Map map, GenStepParams parms)
		{
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			ModExt_Biome_GenStep_BetterCaves modExtension = map.Biome.GetModExtension<ModExt_Biome_GenStep_BetterCaves>();
			if (modExtension == null)
			{
				return;
			}
			extCaves = modExtension;
			directionNoise = new Perlin(0.0020500000100582838, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			tunnelWidthNoise = new Perlin(0.019999999552965164, 2.0, 0.5, 4, Rand.Int, QualityMode.Medium);
			tunnelWidthNoise = new ScaleBias(0.4, 1.0, tunnelWidthNoise);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			BoolGrid visited = new BoolGrid(map);
			List<IntVec3> rockCells = new List<IntVec3>();
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (!visited[allCell] && IsRock(allCell, elevation, map))
				{
					rockCells.Clear();
					map.floodFiller.FloodFill(allCell, (Predicate<IntVec3>)((IntVec3 x) => IsRock(x, elevation, map)), (Action<IntVec3>)delegate (IntVec3 x)
					{
						visited[x] = true;
						rockCells.Add(x);
					}, int.MaxValue, rememberParents: false, (IEnumerable<IntVec3>)null);
					Trim(rockCells, map);
					RemoveSmallDisconnectedSubGroups(rockCells, map);
					if (rockCells.Count >= modExtension.minRocksToGenerateAnyTunnel)
					{
						StartWithTunnel(rockCells, map);
					}
				}
			}
			SmoothGenerated(map);
		}

		private void Trim(List<IntVec3> group, Map map)
		{
			GenMorphology.Open(group, 6, map);
		}

		private bool IsRock(IntVec3 c, MapGenFloatGrid elevation, Map map)
		{
			return c.InBounds(map) && elevation[c] > 0.7f;
		}

		private void StartWithTunnel(List<IntVec3> group, Map map)
		{
			int num = GenMath.RoundRandom((float)group.Count * extCaves.startTunnelsPer10k / 10000f);
			num = Mathf.Min(num, extCaves.maxStartTunnelsPerRockGroup);
			float num2 = TunnelsWidthPerRockCount.Evaluate(group.Count);
			for (int i = 0; i < num; i++)
			{
				IntVec3 start = IntVec3.Invalid;
				float num3 = -1f;
				float dir = -1f;
				float num4 = -1f;
				for (int j = 0; j < 10; j++)
				{
					IntVec3 intVec = FindRandomEdgeCellForTunnel(group, map);
					float distToCave = GetDistToCave(intVec, group, map, 40f, treatOpenSpaceAsCave: false);
					float dist;
					float num5 = FindBestInitialDir(intVec, group, out dist);
					if (!start.IsValid || distToCave > num3 || (distToCave == num3 && dist > num4))
					{
						start = intVec;
						num3 = distToCave;
						dir = num5;
						num4 = dist;
					}
				}
				float width = num2 * Rand.Range(extCaves.tunnelStartWidthFactorMin, extCaves.tunnelStartWidthFactorMax);
				Dig(start, dir, width, group, map, BranchType.Normal, 1);
			}
		}

		private IntVec3 FindRandomEdgeCellForTunnel(List<IntVec3> group, Map map)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			tmpCells.Clear();
			tmpGroupSet.Clear();
			tmpGroupSet.AddRange(group);
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].DistanceToEdge(map) < 3 || !(caves[group[i]] <= 0f))
				{
					continue;
				}
				for (int j = 0; j < 4; j++)
				{
					IntVec3 item = group[i] + cardinalDirections[j];
					if (!tmpGroupSet.Contains(item))
					{
						tmpCells.Add(group[i]);
						break;
					}
				}
			}
			if (!tmpCells.Any())
			{
				Log.Warning("Could not find any valid edge cell.");
				return group.RandomElement();
			}
			return tmpCells.RandomElement();
		}

		private float FindBestInitialDir(IntVec3 start, List<IntVec3> group, out float dist)
		{
			float num = GetDistToNonRock(start, group, IntVec3.East, 40);
			float num2 = GetDistToNonRock(start, group, IntVec3.West, 40);
			float num3 = GetDistToNonRock(start, group, IntVec3.South, 40);
			float num4 = GetDistToNonRock(start, group, IntVec3.North, 40);
			float num5 = GetDistToNonRock(start, group, IntVec3.NorthWest, 40);
			float num6 = GetDistToNonRock(start, group, IntVec3.NorthEast, 40);
			float num7 = GetDistToNonRock(start, group, IntVec3.SouthWest, 40);
			float num8 = GetDistToNonRock(start, group, IntVec3.SouthEast, 40);
			dist = Mathf.Max(new float[8] { num, num2, num3, num4, num5, num6, num7, num8 });
			return GenMath.MaxByRandomIfEqual(0f, num + num8 / 2f + num6 / 2f, 45f, num8 + num3 / 2f + num / 2f, 90f, num3 + num8 / 2f + num7 / 2f, 135f, num7 + num3 / 2f + num2 / 2f, 180f, num2 + num7 / 2f + num5 / 2f, 225f, num5 + num4 / 2f + num2 / 2f, 270f, num4 + num6 / 2f + num5 / 2f, 315f, num6 + num4 / 2f + num / 2f);
		}

		private void Dig(IntVec3 start, float dir, float width, List<IntVec3> group, Map map, BranchType branchType, int depth, HashSet<IntVec3> visited = null)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = start.ToVector3Shifted();
			IntVec3 intVec = start;
			float num = 0f;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			int num2 = 0;
			int num3 = 0;
			float num4 = Rand.Range(extCaves.widthOffsetPerCellMin, extCaves.widthOffsetPerCellMax);
			if (visited == null)
			{
				visited = new HashSet<IntVec3>();
			}
			tmpGroupSet.Clear();
			tmpGroupSet.AddRange(group);
			int num5 = 0;
			while (true)
			{
				if (branchType == BranchType.Normal)
				{
					if (num5 - num2 >= extCaves.allowBranchingAfterSteps && Rand.Chance(extCaves.branchChance))
					{
						BranchType branchType2 = RandomBranchTypeByChance();
						float num6 = CalculateBranchWidth(branchType, width);
						if (num6 > extCaves.minTunnelWidth)
						{
							DigInBestDirection(intVec, dir, new FloatRange(-90f, -40f), num6, group, map, branchType2, depth, visited);
							num2 = num5;
						}
					}
					if (num5 - num3 >= extCaves.allowBranchingAfterSteps && Rand.Chance(extCaves.branchChance))
					{
						BranchType branchType2 = RandomBranchTypeByChance();
						float num6 = CalculateBranchWidth(branchType, width);
						if (num6 > extCaves.minTunnelWidth)
						{
							DigInBestDirection(intVec, dir, new FloatRange(40f, 90f), num6, group, map, branchType2, depth, visited);
							num3 = num5;
						}
					}
				}
				SetCaveAround(intVec, width, map, visited, out var hitAnotherTunnel);
				if (hitAnotherTunnel || (branchType == BranchType.Room && num5 >= extCaves.branchRoomMaxLength))
				{
					break;
				}
				while (val.ToIntVec3() == intVec)
				{
					val += Vector3Utility.FromAngleFlat(dir) * 0.5f;
					num += 0.5f;
				}
				IntVec3 intVec2 = val.ToIntVec3();
				if (!tmpGroupSet.Contains(intVec2))
				{
					break;
				}
				IntVec3 intVec3 = new IntVec3(intVec.x, 0, intVec2.z);
				if (IsRock(intVec3, elevation, map))
				{
					caves[intVec3] = Math.Max(caves[intVec3], width);
					visited.Add(intVec3);
				}
				width = ((branchType != BranchType.Tunnel) ? (width - num4) : (width - num4 * extCaves.widthOffsetPerCellTunnelFactor));
				if (width < extCaves.minTunnelWidth)
				{
					break;
				}
				intVec = intVec2;
				dir += (float)directionNoise.GetValue(num * 60f, (float)start.x * 200f, (float)start.z * 200f) * extCaves.directionChangeSpeed;
				num5++;
			}
		}

		private void DigInBestDirection(IntVec3 curIntVec, float curDir, FloatRange dirOffset, float width, List<IntVec3> group, Map map, BranchType branchType, int depth, HashSet<IntVec3> visited = null)
		{
			float dir = -1f;
			int num = -1;
			for (int i = 0; i < 6; i++)
			{
				float num2 = curDir + dirOffset.RandomInRange;
				int distToNonRock = GetDistToNonRock(curIntVec, group, num2, 50);
				if (distToNonRock > num)
				{
					num = distToNonRock;
					dir = num2;
				}
			}
			if (num >= extCaves.minDistToOutsideForBranching)
			{
				Dig(curIntVec, dir, width, group, map, branchType, depth + 1, visited);
			}
		}

		private void SetCaveAround(IntVec3 center, float tunnelWidth, Map map, HashSet<IntVec3> visited, out bool hitAnotherTunnel)
		{
			hitAnotherTunnel = false;
			int num = GenRadial.NumCellsInRadius(tunnelWidth / 2f * tunnelWidthNoise.GetValue(center));
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid caves = MapGenerator.Caves;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (IsRock(intVec, elevation, map))
				{
					if (caves[intVec] > 0f && !visited.Contains(intVec))
					{
						hitAnotherTunnel = true;
					}
					caves[intVec] = Math.Max(caves[intVec], tunnelWidth);
					visited.Add(intVec);
				}
			}
		}

		private BranchType RandomBranchTypeByChance()
		{
			float value = Rand.Value;
			if (value < extCaves.branchChanceTypeRoom)
			{
				return BranchType.Room;
			}
			if (value < extCaves.branchChanceTypeRoom + extCaves.branchChanceTypeTunnel)
			{
				return BranchType.Tunnel;
			}
			return BranchType.Normal;
		}

		private float CalculateBranchWidth(BranchType branchType, float prevWidth)
		{
			switch (branchType)
			{
				case BranchType.Room:
					return Rand.Range(extCaves.branchRoomFixedWidthMin, extCaves.branchRoomFixedWidthMax);
				case BranchType.Tunnel:
					return Rand.Range(extCaves.branchTunnelFixedWidthMin, extCaves.branchTunnelFixedWidthMax);
				default:
					return prevWidth * Rand.Range(extCaves.branchWidthFactorMin, extCaves.branchWidthFactorMax);
			}
		}

		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, IntVec3 offset, int maxDist)
		{
			groupSet.Clear();
			groupSet.AddRange(group);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = from + offset * i;
				if (!groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		private int GetDistToNonRock(IntVec3 from, List<IntVec3> group, float dir, int maxDist)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			groupSet.Clear();
			groupSet.AddRange(group);
			Vector3 val = Vector3Utility.FromAngleFlat(dir);
			for (int i = 0; i <= maxDist; i++)
			{
				IntVec3 item = (from.ToVector3Shifted() + val * (float)i).ToIntVec3();
				if (!groupSet.Contains(item))
				{
					return i;
				}
			}
			return maxDist;
		}

		private float GetDistToCave(IntVec3 cell, List<IntVec3> group, Map map, float maxDist, bool treatOpenSpaceAsCave)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			tmpGroupSet.Clear();
			tmpGroupSet.AddRange(group);
			int num = GenRadial.NumCellsInRadius(maxDist);
			IntVec3[] radialPattern = GenRadial.RadialPattern;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + radialPattern[i];
				if ((treatOpenSpaceAsCave && !tmpGroupSet.Contains(intVec)) || (intVec.InBounds(map) && caves[intVec] > 0f))
				{
					return cell.DistanceTo(intVec);
				}
			}
			return maxDist;
		}

		private void RemoveSmallDisconnectedSubGroups(List<IntVec3> group, Map map)
		{
			groupSet.Clear();
			groupSet.AddRange(group);
			groupVisited.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (groupVisited.Contains(group[i]) || !groupSet.Contains(group[i]))
				{
					continue;
				}
				subGroup.Clear();
				map.floodFiller.FloodFill(group[i], (Predicate<IntVec3>)((IntVec3 x) => groupSet.Contains(x)), (Action<IntVec3>)delegate (IntVec3 x)
				{
					subGroup.Add(x);
					groupVisited.Add(x);
				}, int.MaxValue, rememberParents: false, (IEnumerable<IntVec3>)null);
				if (subGroup.Count < extCaves.minRocksToGenerateAnyTunnel || (float)subGroup.Count < 0.05f * (float)group.Count)
				{
					for (int j = 0; j < subGroup.Count; j++)
					{
						groupSet.Remove(subGroup[j]);
					}
				}
			}
			group.Clear();
			group.AddRange(groupSet);
		}

		private void SmoothGenerated(Map map)
		{
			MapGenFloatGrid caves = MapGenerator.Caves;
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (caves[allCell] > 0f)
				{
					list.Add(allCell);
				}
			}
			GenMorphology.Close(list, 3, map);
			foreach (IntVec3 allCell2 in map.AllCells)
			{
				if (allCell2.CloseToEdge(map, 3))
				{
					continue;
				}
				if (list.Contains(allCell2))
				{
					if (caves[allCell2] <= 0f)
					{
						caves[allCell2] = 1f;
					}
				}
				else if (caves[allCell2] > 0f)
				{
					caves[allCell2] = 0f;
				}
			}
		}
	}
}
