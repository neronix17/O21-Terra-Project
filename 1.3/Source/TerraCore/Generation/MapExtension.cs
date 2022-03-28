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
	public static class MapExtension
	{
		private const int normalMapWidth = 250;

		private const int normalMapHeight = 250;

		public static float ScaleValueOnSize(this Map map, float value)
		{
			return value * (float)map.Size.x * (float)map.Size.y / 250f / 250f;
		}

		public static int ScaleValueOnSize(this Map map, int value)
		{
			return GenMath.RoundRandom((float)value * (float)map.Size.x * (float)map.Size.y / 250f / 250f);
		}
	}
}
