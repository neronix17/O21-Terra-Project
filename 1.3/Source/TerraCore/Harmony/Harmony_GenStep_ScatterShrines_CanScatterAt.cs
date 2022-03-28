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
	[HarmonyPatch(typeof(GenStep_ScatterShrines), "CanScatterAt")]
	public class Harmony_GenStep_ScatterShrines_CanScatterAt
	{
		public static bool Prefix(Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.removeShrines)
			{
				return false;
			}
			return true;
		}
	}
}
