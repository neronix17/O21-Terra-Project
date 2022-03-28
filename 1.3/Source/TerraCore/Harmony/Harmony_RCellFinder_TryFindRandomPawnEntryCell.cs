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
	[HarmonyPatch(typeof(RCellFinder), "TryFindRandomPawnEntryCell")]
	public class Harmony_RCellFinder_TryFindRandomPawnEntryCell
	{
		public static bool Prefix(ref bool __result, out IntVec3 result, Map map, float roadChance, Predicate<IntVec3> extraValidator = null)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.overwriteRoof != RoofOverwriteType.FullStable)
			{
				result = IntVec3.Invalid;
				return true;
			}
			__result = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(map) && map.reachability.CanReachColony(c) && GridsUtility.GetRoom(c, map).TouchesMapEdge && (extraValidator == null || extraValidator(c)), map, roadChance, out result);
			return false;
		}
	}
}
