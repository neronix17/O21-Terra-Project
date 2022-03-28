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
	[HarmonyPatch(typeof(GenStep_FindPlayerStartSpot), "Generate")]
	public class Harmony_GenStep_FindPlayerStartSpot_Generate
	{
		private const int MaxDistanceToEdge = 6;

		private const int MinRoomCellCount = 30;

		public static bool Prefix(Map map)
		{
			ModExt_Biome_FeatureControl modExtension = map.Biome.GetModExtension<ModExt_Biome_FeatureControl>();
			if (modExtension == null || modExtension.overwriteRoof != RoofOverwriteType.FullStable)
			{
				return true;
			}
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 30, (IntVec3 x) => x.CloseToEdge(map, 6));
			return false;
		}
	}
}
