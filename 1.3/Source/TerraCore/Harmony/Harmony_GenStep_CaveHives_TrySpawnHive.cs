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
	[HarmonyPatch(typeof(GenStep_CaveHives), "TrySpawnHive")]
	internal class Harmony_GenStep_CaveHives_TrySpawnHive
	{
		public static bool Prefix(GenStep_CaveHives __instance, Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.overwriteHives == HiveOverwriteType.Remove)
			{
				return false;
			}
			return true;
		}

		public static void Postfix(GenStep_CaveHives __instance, Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.overwriteHives == HiveOverwriteType.AddActive)
			{
				List<Hive> value = Traverse.Create((object)__instance).Field("spawnedHives").GetValue<List<Hive>>();
				Hive hive = value.Last();
				hive.PawnSpawner.canSpawnPawns = true;
				hive.GetComp<CompSpawnerHives>().canSpawnHives = true;
			}
		}
	}
}
