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
	[HarmonyPatch(typeof(GenStep_CaveHives), "Generate")]
	public class Harmony_GenStep_CaveHives_Generate
	{
		public static void Postfix(GenStep_CaveHives __instance, Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && (modExtension.overwriteHives == HiveOverwriteType.Add || modExtension.overwriteHives == HiveOverwriteType.AddActive))
			{
				int num = map.ScaleValueOnSize(modExtension.additionalHivesScaling);
				Traverse val = Traverse.Create((object)__instance).Method("TrySpawnHive", new object[1] { map });
				for (int i = 0; i < num; i++)
				{
					val.GetValue();
				}
			}
		}
	}
}
