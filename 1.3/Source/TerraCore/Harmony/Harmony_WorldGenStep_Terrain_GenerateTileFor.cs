using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;
using RimWorld.Planet;

namespace TerraCore
{
	[HarmonyPatch(typeof(WorldGenStep_Terrain), "GenerateTileFor")]
	public class Harmony_WorldGenStep_Terrain_GenerateTileFor
	{
		public static void Postfix(ref Tile __result)
		{
			GenWorldGen.UpdateTileByBiomeModExts(__result);
		}
	}
}
