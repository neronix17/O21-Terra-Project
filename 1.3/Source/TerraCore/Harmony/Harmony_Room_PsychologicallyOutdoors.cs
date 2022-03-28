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
	[HarmonyPatch(typeof(Room), "PsychologicallyOutdoors", MethodType.Getter)]
	public class Harmony_Room_PsychologicallyOutdoors
	{
		public static bool Prefix(Room __instance, ref bool __result)
		{
			ModExt_Biome_FeatureControl modExtension = __instance.Map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.roomCalculationType == RoomCalculationType.Default)
			{
				return true;
			}
			__result = __instance.OutdoorsByRCType(modExtension.roomCalculationType);
			return false;
		}
	}
}
