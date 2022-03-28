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
	[HarmonyPatch(typeof(DropCellFinder), "CanPhysicallyDropInto")]
	public class Harmony_DropCellFinder_CanPhysicallyDropInto
	{
		private const int MaxDistanceToEdge = 5;

		public static bool Prefix(ref bool __result, IntVec3 c, Map map, bool canRoofPunch)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.overwriteRoof != RoofOverwriteType.FullStable)
			{
				return true;
			}
			if (!c.Walkable(map))
			{
				__result = false;
				return false;
			}
			if (c.CloseToEdge(map, 5))
			{
				__result = true;
				return false;
			}
			RoofDef roof = c.GetRoof(map);
			if (roof != null && !canRoofPunch)
			{
				__result = false;
			}
			else
			{
				__result = true;
			}
			return false;
		}
	}
}
