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
	[HarmonyPatch(typeof(GenTemperature), "GetTemperatureFromSeasonAtTile")]
	public class Harmony_GenTemperature_GetTemperatureFromSeasonAtTile
	{
		public static void Postfix(ref float __result, int tile)
		{
			ModExt_Biome_Temperature modExtension = Find.WorldGrid[tile].biome.GetModExtension<ModExt_Biome_Temperature>();
			if (modExtension != null)
			{
				__result = __result * (1f - modExtension.tempStableWeight) + modExtension.tempStableValue * modExtension.tempStableWeight + modExtension.tempOffset;
			}
		}
	}
}
