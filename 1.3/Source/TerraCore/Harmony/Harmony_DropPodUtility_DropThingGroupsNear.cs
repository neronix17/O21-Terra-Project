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
	[HarmonyPatch(typeof(DropPodUtility), "DropThingGroupsNear")]
	public class Harmony_DropPodUtility_DropThingGroupsNear
	{
		private const int MaxDistanceToEdge = 6;

		public static void Prefix(IntVec3 dropCenter, Map map, ref bool instaDrop)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.overwriteRoof == RoofOverwriteType.FullStable && dropCenter.CloseToEdge(map, 6))
			{
				instaDrop = true;
			}
		}
	}
}
