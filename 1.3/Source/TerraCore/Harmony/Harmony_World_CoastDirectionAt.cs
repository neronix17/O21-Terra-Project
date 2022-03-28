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
	[HarmonyPatch(typeof(World), "CoastDirectionAt")]
	public class Harmony_World_CoastDirectionAt
	{
		public static bool Prefix(World __instance, ref Rot4 __result, int tileID)
		{
			Tile tile = __instance.grid[tileID];
			ModExt_Biome_FeatureControl modExtension = tile.biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.removeBeach)
			{
				__result = Rot4.Invalid;
				return false;
			}
			return true;
		}
	}
}
