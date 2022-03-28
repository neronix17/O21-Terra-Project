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
	[HarmonyPatch(typeof(RoofCollapseUtility), "WithinRangeOfRoofHolder")]
	public class Harmony_GenStep_RoofCollapseUtility_WithinRangeOfRoofHolder
	{
		public static bool Prefix(ref bool __result, IntVec3 c, Map map)
		{
			if (map.roofGrid.RoofAt(c) == RoofDefOf.RoofRockUncollapsable)
			{
				__result = true;
				return false;
			}
			return true;
		}
	}
}
