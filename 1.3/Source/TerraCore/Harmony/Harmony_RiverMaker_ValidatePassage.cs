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
	[HarmonyPatch(typeof(RiverMaker), "ValidatePassage")]
	public class Harmony_RiverMaker_ValidatePassage
	{
		public static void Postfix(object __instance, Map map)
		{
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			TerrainDef[] topGrid = map.terrainGrid.topGrid;
			int i = 0;
			for (int num = topGrid.Count(); i < num; i++)
			{
				if (topGrid[i] != TerraCore.TerrainDefOf.WaterMovingChestDeep)
				{
					continue;
				}
				IntVec3 intVec = map.cellIndices.IndexToCell(i);
				for (int j = 0; j < 4; j++)
				{
					IntVec3 c = intVec + cardinalDirections[j];
					if (!c.InBounds(map) || map.terrainGrid.TerrainAt(c) == TerraCore.TerrainDefOf.WaterMovingShallow)
					{
					}
				}
			}
		}
	}
}
