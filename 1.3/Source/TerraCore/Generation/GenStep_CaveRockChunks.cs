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
	public class GenStep_CaveRockChunks : GenStep
	{
		private const float ThreshLooseRock = 0.55f;

		private const float PlaceProbabilityPerCell = 0.006f;

		private const float RubbleProbability = 0.2f;

		private ModuleBase freqFactorNoise;

		private static readonly IntRange MaxRockChunksPerGroup = new IntRange(1, 6);

		public override int SeedPart => 488758298;

		public override void Generate(Map map, GenStepParams parms)
		{
			if (map.TileInfo.WaterCovered)
			{
				return;
			}
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.overwriteRockChunks != RockChunksOverwriteType.AddToCaves)
			{
				return;
			}
			freqFactorNoise = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			freqFactorNoise = new ScaleBias(1.0, 1.0, freqFactorNoise);
			NoiseDebugUI.StoreNoiseRender(freqFactorNoise, "cave_rock_chunks_freq_factor");
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 allCell in map.AllCells)
			{
				float num = 0.006f * freqFactorNoise.GetValue(allCell);
				if (elevation[allCell] >= 0.55f && Rand.Value < num)
				{
					GrowLowRockFormationFrom(allCell, map);
				}
			}
			freqFactorNoise = null;
		}

		private void GrowLowRockFormationFrom(IntVec3 root, Map map)
		{
			ThingDef filth_RubbleRock = ThingDefOf.Filth_RubbleRock;
			ThingDef mineableThing = Find.World.NaturalRockTypesIn(map.Tile).RandomElement().building.mineableThing;
			Rot4 random = Rot4.Random;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			IntVec3 intVec = root;
			int randomInRange = MaxRockChunksPerGroup.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				intVec += Rot4Utility.RandomButExclude(random).FacingCell;
				if (!intVec.InBounds(map) || intVec.GetEdifice(map) != null || intVec.GetFirstItem(map) != null || elevation[intVec] < 0.55f)
				{
					break;
				}
				List<TerrainAffordanceDef> affordances = map.terrainGrid.TerrainAt(intVec).affordances;
				if (!affordances.Contains(TerrainAffordanceDefOf.Medium) && !affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					break;
				}
				GenSpawn.Spawn(mineableThing, intVec, map);
				IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
				foreach (IntVec3 intVec2 in adjacentCellsAndInside)
				{
					if (Rand.Value < 0.2f)
					{
						continue;
					}
					IntVec3 intVec3 = intVec + intVec2;
					if (!intVec3.InBounds(map))
					{
						continue;
					}
					bool flag = false;
					List<Thing> thingList = intVec3.GetThingList(map);
					for (int k = 0; k < thingList.Count; k++)
					{
						Thing thing = thingList[k];
						if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						FilthMaker.TryMakeFilth(intVec3, map, filth_RubbleRock, 1);
					}
				}
			}
		}
	}
}
