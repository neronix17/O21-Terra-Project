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
	[HarmonyPatch(typeof(TradeUtility), "SpawnDropPod")]
	public class Harmony_TradeUtility_SpawnDropPod
	{
		public static bool Prefix(IntVec3 dropSpot, Map map, Thing t)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.overwriteRoof != RoofOverwriteType.FullStable)
			{
				return true;
			}
			GenPlace.TryPlaceThing(t, dropSpot, map, ThingPlaceMode.Near, (Action<Thing, int>)null, (Predicate<IntVec3>)null);
			return false;
		}
	}
}
