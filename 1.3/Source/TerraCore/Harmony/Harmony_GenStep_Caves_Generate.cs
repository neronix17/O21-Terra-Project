﻿using RimWorld;
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
	[HarmonyPatch(typeof(GenStep_Caves), "Generate")]
	public class Harmony_GenStep_Caves_Generate
	{
		public static bool Prefix(Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension != null && modExtension.overwriteCaves == GenFeatureTristate.Remove)
			{
				return false;
			}
			return true;
		}
	}
}
