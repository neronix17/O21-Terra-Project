using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;
using RimWorld.Planet;

namespace TerraCore
{
	[HarmonyPatch(typeof(World), "HasCaves")]
	public class Harmony_World_HasCaves
	{
		public static bool Prefix(World __instance, ref bool __result, int tile)
		{
			BiomeDef biome = __instance.grid[tile].biome;
			ModExt_Biome_GenStep_BetterCaves modExtension = biome.GetModExtension<ModExt_Biome_GenStep_BetterCaves>();
			if (modExtension != null)
			{
				__result = true;
				return false;
			}
			ModExt_Biome_FeatureControl modExtension2 = biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension2 != null)
			{
				if (modExtension2.overwriteCaves == GenFeatureTristate.Add)
				{
					__result = true;
					return false;
				}
				if (modExtension2.overwriteCaves == GenFeatureTristate.Remove)
				{
					__result = false;
					return false;
				}
			}
			return true;
		}
	}
}
