using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public class WorldGenStep_SpecialTerrain : WorldGenStep
	{
		private const float ImpassableChangeChance = 0.8f;

		private const float ImpassableChangeRecursiveChance = 0.2f;

		private const int MaxImpassableChangeDepth = 3;

		private static List<int> tmpNeighbors = new List<int>();

		public override int SeedPart => 144374476;

		public override void GenerateFresh(string seed)
		{
			List<BiomeDef> allDefsListForReading = DefDatabase<BiomeDef>.AllDefsListForReading;
			List<BiomeDef> list = new List<BiomeDef>();
			for (int num = allDefsListForReading.Count - 1; num >= 0; num--)
			{
				if (allDefsListForReading[num].WorkerSpecial() != null)
				{
					list.Add(allDefsListForReading[num]);
					allDefsListForReading[num].WorkerSpecial().ResetChance();
				}
			}
			WorldGrid worldGrid = Find.WorldGrid;
			List<Tile> tiles = worldGrid.tiles;
			int tilesCount = worldGrid.TilesCount;
			for (int num = 0; num < tilesCount; num++)
			{
				if (list.Contains(tiles[num].biome))
				{
					continue;
				}
				foreach (BiomeDef item in list)
				{
					BiomeWorkerSpecial biomeWorkerSpecial = item.WorkerSpecial();
					Tile tile = tiles[num];
					if (biomeWorkerSpecial.PreRequirements(tile) && biomeWorkerSpecial.TryGenerateByChance())
					{
						tile.biome = item;
						GenWorldGen.UpdateTileByBiomeModExts(tile);
						biomeWorkerSpecial.PostGeneration(num);
					}
				}
			}
			for (int num = 0; num < tilesCount; num++)
			{
				Tile tile = tiles[num];
				if (tile.biome == TerraCore.BiomeDefOf.CaveOasis || tile.biome == TerraCore.BiomeDefOf.TunnelworldCave)
				{
					List<BiomeDef> list2 = new List<BiomeDef>(list);
					list2.Add(RimWorld.BiomeDefOf.SeaIce);
					list2.Add(RimWorld.BiomeDefOf.Lake);
					list2.Add(RimWorld.BiomeDefOf.Ocean);
					MakeImpassableHillsAroundTile(worldGrid, num, list2, 1);
				}
			}
		}

		private void MakeImpassableHillsAroundTile(WorldGrid grid, int tileID, List<BiomeDef> excludeBiomes, int currDepth)
		{
			bool flag = false;
			grid.GetTileNeighbors(tileID, tmpNeighbors);
			for (int i = 0; i < tmpNeighbors.Count; i++)
			{
				if (grid[tmpNeighbors[i]].biome == TerraCore.BiomeDefOf.CaveEntrance)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return;
			}
			for (int i = 0; i < tmpNeighbors.Count; i++)
			{
				Tile tile = grid[tmpNeighbors[i]];
				if (!excludeBiomes.Contains(tile.biome) && Rand.Value < 0.8f)
				{
					tile.hilliness = Hilliness.Impassable;
					if (currDepth < 3 && Rand.Value < 0.2f)
					{
						MakeImpassableHillsAroundTile(grid, tmpNeighbors[i], excludeBiomes, currDepth + 1);
					}
				}
			}
		}
	}
}
