using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public static class GenWorldGen
	{
		public static void UpdateTileByBiomeModExts(Tile tile)
		{
			ModExt_Biome_Replacement modExtension = tile.biome.GetModExtension<ModExt_Biome_Replacement>();
			if (modExtension != null)
			{
				if (modExtension.replaceElevation)
				{
					tile.elevation = Rand.RangeInclusive(modExtension.elevationMin, modExtension.elevationMax);
				}
				if (modExtension.replaceHilliness.HasValue)
				{
					tile.hilliness = modExtension.replaceHilliness.Value;
				}
			}
			ModExt_Biome_Temperature modExtension2 = tile.biome.GetModExtension<ModExt_Biome_Temperature>();
			if (modExtension2 != null)
			{
				tile.temperature = tile.temperature * (1f - modExtension2.tempStableWeight) + modExtension2.tempStableValue * modExtension2.tempStableWeight + modExtension2.tempOffset;
			}
		}

		public static int NextRandomDigDir(int dir, int step)
		{
			step = Mathf.Clamp(step, 1, 3);
			dir += Rand.RangeInclusive(-step, step);
			if (dir < 0)
			{
				dir += 6;
			}
			if (dir > 5)
			{
				dir -= 6;
			}
			return dir;
		}

		public static int InvertDigDir(int dir)
		{
			dir += 3;
			if (dir > 5)
			{
				dir -= 6;
			}
			return dir;
		}
	}

}
