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
	[HarmonyPatch(typeof(GenStep_Terrain), "TerrainFrom")]
	public class Harmony_GenStep_Terrain_TerrainFrom
	{
		public static void Postfix(ref TerrainDef __result, IntVec3 c, Map map)
		{
			BiomeDef biome = map.Biome;
			ModExt_Biome_Replacement modExt_Biome_Replacement = biome.GetModExtension<ModExt_Biome_Replacement>() ?? ModExt_Biome_Replacement.Default;
			if (__result == RimWorld.TerrainDefOf.Sand)
			{
				__result = modExt_Biome_Replacement.sandReplacement;
			}
			if (__result == RimWorld.TerrainDefOf.Gravel)
			{
				__result = modExt_Biome_Replacement.gravelReplacement;
			}
			if (biome.HasModExtension<ModExt_Biome_GenStep_Islands>())
			{
				TerrainDef terrainDef = IslandNoises.TerrainAtFromTerrainPatchMakerByFertility(c, map, __result);
				if (terrainDef != null)
				{
					__result = terrainDef;
				}
			}
			if (__result == TerraCore.TerrainDefOf.FillerStone)
			{
				__result = GenStep_RocksFromGrid.RockDefAt(c).building.naturalTerrain;
			}
		}
	}
}
