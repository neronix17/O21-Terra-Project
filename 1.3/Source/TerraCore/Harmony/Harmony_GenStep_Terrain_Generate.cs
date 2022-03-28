using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace TerraCore
{
	[HarmonyPatch(typeof(GenStep_Terrain), "Generate")]
	public class Harmony_GenStep_Terrain_Generate
	{
		public static void Postfix(Map map)
		{
			IntVec3[] adjacentCells = GenAdj.AdjacentCells;
			TerrainDef[] topGrid = map.terrainGrid.topGrid;
			int i;
			if (map.Biome == TerraCore.BiomeDefOf.TundraSkerries)
			{
				i = 0;
				for (int num = topGrid.Count(); i < num; i++)
				{
					int num2 = 0;
					if (topGrid[i] != TerraCore.TerrainDefOf.NormalSand && topGrid[i] != TerraCore.TerrainDefOf.Sandstone_Rough && topGrid[i] != TerraCore.TerrainDefOf.Granite_Rough && topGrid[i] != TerraCore.TerrainDefOf.Limestone_Rough && topGrid[i] != TerraCore.TerrainDefOf.Slate_Rough && topGrid[i] != TerraCore.TerrainDefOf.Marble_Rough)
					{
						continue;
					}
					IntVec3 intVec = map.cellIndices.IndexToCell(i);
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c = intVec + adjacentCells[j];
						if (c.InBounds(map) && map.terrainGrid.TerrainAt(c) == TerraCore.TerrainDefOf.WaterOceanShallow)
						{
							num2++;
						}
					}
					if (num2 > 5)
					{
						map.terrainGrid.SetTerrain(intVec, TerraCore.TerrainDefOf.WaterOceanShallow);
					}
				}
			}
			i = 0;
			for (int num = topGrid.Count(); i < num; i++)
			{
				if (topGrid[i] != TerraCore.TerrainDefOf.WaterOceanDeep)
				{
					continue;
				}
				IntVec3 intVec = map.cellIndices.IndexToCell(i);
				for (int j = 0; j < 8; j++)
				{
					IntVec3 c = intVec + adjacentCells[j];
					if (c.InBounds(map) && map.terrainGrid.TerrainAt(c) == TerraCore.TerrainDefOf.WaterOceanShallow)
					{
						map.terrainGrid.SetTerrain(c, TerraCore.TerrainDefOf.WaterOceanChestDeep);
						map.terrainGrid.SetTerrain(intVec, TerraCore.TerrainDefOf.WaterOceanChestDeep);
					}
				}
			}
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null)
			{
				if (modExtension.overwriteRoof == RoofOverwriteType.FullStable)
				{
					GenRoof.SetRoofComplete(map, TerraCore.RoofDefOf.RoofRockUncollapsable);
				}
				else if (modExtension.overwriteRoof == RoofOverwriteType.DeepOnlyStable)
				{
					GenRoof.SetStableDeepRoof(map);
				}
			}
		}
	}
}
