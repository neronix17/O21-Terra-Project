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
	[HarmonyPatch(typeof(ScenPart_PlayerPawnsArriveMethod), "GenerateIntoMap")]
	public class Harmony_ScenPart_PlayerPawnsArriveMethod_GenerateIntoMap
	{
		public static void Prefix(ScenPart_PlayerPawnsArriveMethod __instance, Map map)
		{
			if (Find.GameInitData != null)
			{
				ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
				if (modExtension != null && modExtension.overwriteRoof == RoofOverwriteType.FullStable)
				{
					Traverse.Create((object)__instance).Field("method").SetValue((object)PlayerPawnsArriveMethod.Standing);
				}
			}
		}
	}
}
