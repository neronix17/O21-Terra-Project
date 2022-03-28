using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;

using HarmonyLib;

namespace TerraCore
{
	[HarmonyPatch(typeof(GenStep_CavesTerrain), "Generate")]
	public class Harmony_GenStep_CavesTerrain_Generate
	{
		public static bool Prefix(Map map)
		{
			if (!Find.World.HasCaves(map.Tile))
			{
				return false;
			}
			ModExt_Biome_GenStep_BetterCaves modExtension = map.Biome.GetModExtension<ModExt_Biome_GenStep_BetterCaves>();
			if (modExtension == null || (modExtension.terrainPatchMakerCaveWater == null && modExtension.terrainPatchMakerCaveGravel == null))
			{
				return true;
			}
			ModuleBase moduleBase = new Perlin(modExtension.terrainPatchMakerFrequencyCaveWater, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			ModuleBase moduleBase2 = new Perlin(modExtension.terrainPatchMakerFrequencyCaveGravel, 2.0, 0.5, 6, Rand.Int, QualityMode.Medium);
			MapGenFloatGrid caves = MapGenerator.Caves;
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (!(caves[allCell] > 0f))
				{
					continue;
				}
				TerrainDef terrain = allCell.GetTerrain(map);
				if (terrain.IsRiver)
				{
					continue;
				}
				float val = moduleBase.GetValue(allCell);
				TerrainDef current2 = map.terrainGrid.TerrainAt(allCell);
				TerrainDef terrainDef = TerrainThresholdWEO.TerrainAtValue(modExtension.terrainPatchMakerCaveWater, val, current2);
				if (terrainDef != null)
				{
					map.terrainGrid.SetTerrain(allCell, terrainDef);
					continue;
				}
				float val2 = moduleBase2.GetValue(allCell);
				TerrainDef terrainDef2 = TerrainThresholdWEO.TerrainAtValue(modExtension.terrainPatchMakerCaveGravel, val2, current2);
				if (terrainDef2 != null)
				{
					map.terrainGrid.SetTerrain(allCell, terrainDef2);
				}
			}
			return false;
		}
	}
}
