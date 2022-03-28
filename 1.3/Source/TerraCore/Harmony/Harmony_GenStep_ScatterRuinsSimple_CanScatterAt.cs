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
	[HarmonyPatch(typeof(GenStep_ScatterRuinsSimple), "CanScatterAt")]
	public class Harmony_GenStep_ScatterRuinsSimple_CanScatterAt
	{
		public static bool Prefix(Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.removeRuinsSimple)
			{
				return false;
			}
			return true;
		}
	}
}
