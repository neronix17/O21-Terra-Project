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
	public static class RoomExtension
	{
		public static bool OutdoorsByRCType(this Room room, RoomCalculationType rcType)
		{
			switch (rcType)
			{
				case RoomCalculationType.LessSky:
					{
						if (room.TouchesMapEdge)
						{
							return true;
						}
						float num = room.OpenRoofCount;
						float num2 = num / (float)room.CellCount;
						float num3 = 0.05f / (0.003f * num + 0.1f);
						return num2 >= num3;
					}
				case RoomCalculationType.NoSky:
					return room.TouchesMapEdge;
				default:
					return room.PsychologicallyOutdoors;
			}
		}
	}
}
