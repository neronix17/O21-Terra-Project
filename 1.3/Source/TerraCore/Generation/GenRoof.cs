using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{

	public static class GenRoof
	{
		private const int deepRoofDistance = 11;

		public static void SetRoofComplete(Map map, RoofDef type)
		{
			foreach (IntVec3 allCell in map.AllCells)
			{
				map.roofGrid.SetRoof(allCell, type);
			}
			map.roofGrid.RoofGridUpdate();
		}

		public static void SetStableDeepRoof(Map map)
		{
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (!map.roofGrid.Roofed(allCell))
				{
					list.Add(allCell);
				}
			}
			GenMorphology.Dilate(list, 11, map);
			foreach (IntVec3 allCell2 in map.AllCells)
			{
				if (!list.Contains(allCell2))
				{
					map.roofGrid.SetRoof(allCell2, RoofDefOf.RoofRockUncollapsable);
				}
			}
			map.roofGrid.RoofGridUpdate();
		}
	}
}
