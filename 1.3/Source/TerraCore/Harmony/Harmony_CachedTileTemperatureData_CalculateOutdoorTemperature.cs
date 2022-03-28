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
	public class Harmony_CachedTileTemperatureData_CalculateOutdoorTemperatureAtTile
	{
		public static void Postfix(object __instance, ref float __result)
		{
			int value = Traverse.Create(__instance).Field("tile").GetValue<int>();
			ModExt_Biome_Temperature modExtension = Find.WorldGrid[value].biome.GetModExtension<ModExt_Biome_Temperature>();
			if (modExtension != null)
			{
				__result = __result * (1f - modExtension.tempStableWeight) + modExtension.tempStableValue * modExtension.tempStableWeight + modExtension.tempOffset;
			}
		}
	}
}
