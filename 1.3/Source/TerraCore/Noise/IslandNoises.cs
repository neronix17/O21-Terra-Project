using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace TerraCore
{
	public static class IslandNoises
	{
		public class IslandCone
		{
			public int centerX;

			public int centerZ;

			public int radiusX;

			public int radiusZ;

			public float distZFactor;

			public int radiusSquared;
		}

		private static ModExt_Biome_GenStep_Islands extIslands;

		private static ModuleBase noiseElevation;

		private static ModuleBase noiseFertility;

		private static List<IslandCone> islandCones;

		public static void Init(Map map)
		{
			ModExt_Biome_GenStep_Islands modExtension = map.Biome.GetModExtension<ModExt_Biome_GenStep_Islands>();
			if (modExtension != null)
			{
				extIslands = modExtension;
				string text = "islands " + map.Biome.defName;
				ModuleBase moduleBase = new Perlin(modExtension.baseFrequency, 2.0, 0.5, 5, Rand.Range(0, int.MaxValue), QualityMode.High);
				NoiseDebugUI.StoreNoiseRender(moduleBase, text + " base");
				noiseElevation = new ScaleBias(modExtension.noiseElevationPreScale, modExtension.noiseElevationPreOffset, moduleBase);
				NoiseDebugUI.StoreNoiseRender(noiseElevation, text + " elevation");
				noiseFertility = new ScaleBias(modExtension.noiseFertilityPreScale, modExtension.noiseFertilityPreOffset, moduleBase);
				NoiseDebugUI.StoreNoiseRender(noiseFertility, text + " fertility");
				islandCones = new List<IslandCone>();
				int num = Rand.Range(modExtension.islandCountMin, modExtension.islandCountMax);
				for (int i = 0; i < num; i++)
				{
					IslandCone islandNoise = new IslandCone();
					SetIslandOnRandomLocation(ref islandNoise, map);
					islandCones.Add(islandNoise);
				}
			}
		}

		public static void Reset()
		{
			extIslands = null;
			noiseElevation = null;
			noiseFertility = null;
			islandCones = null;
		}

		public static void WorkOnMapGenerator(Map map)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (extIslands.calcElevationType == GenStepCalculationType.Set)
				{
					elevation[allCell] = GetValueAt(allCell, noiseElevation) * extIslands.elevationPostScale + extIslands.elevationPostOffset;
				}
				else if (extIslands.calcElevationType == GenStepCalculationType.Add)
				{
					elevation[allCell] += GetValueAt(allCell, noiseElevation) * extIslands.elevationPostScale + extIslands.elevationPostOffset;
				}
				if (extIslands.calcFertilityType == GenStepCalculationType.Set)
				{
					fertility[allCell] = GetValueAt(allCell, noiseFertility) * extIslands.fertilityPostScale + extIslands.fertilityPostOffset;
				}
				else if (extIslands.calcFertilityType == GenStepCalculationType.Add)
				{
					fertility[allCell] += GetValueAt(allCell, noiseFertility) * extIslands.fertilityPostScale + extIslands.fertilityPostOffset;
				}
			}
		}

		public static TerrainDef TerrainAtFromTerrainPatchMakerByFertility(IntVec3 c, Map map, TerrainDef current)
		{
			if (extIslands.terrainPatchMakerByIslandFertility == null)
			{
				return null;
			}
			return TerrainThresholdWEO.TerrainAtValue(extIslands.terrainPatchMakerByIslandFertility, GetValueAt(c, noiseFertility), current);
		}

		private static void SetIslandOnRandomLocation(ref IslandCone islandNoise, Map map)
		{
			ModExt_Biome_GenStep_Islands modExtension = map.Biome.GetModExtension<ModExt_Biome_GenStep_Islands>();
			int min = (int)((float)map.Size.x * modExtension.minSizeX);
			int max = (int)((float)map.Size.x * modExtension.maxSizeX);
			int min2 = (int)((float)map.Size.z * modExtension.minSizeZ);
			int max2 = (int)((float)map.Size.z * modExtension.maxSizeZ);
			int num = Rand.Range(min, max) / 2;
			int num2 = Rand.Range(min2, max2) / 2;
			islandNoise.radiusX = num;
			islandNoise.radiusZ = num2;
			islandNoise.centerX = Rand.Range(num + 5, map.Size.x - num - 5);
			islandNoise.centerZ = Rand.Range(num2 + 5, map.Size.z - num2 - 5);
			islandNoise.distZFactor = (float)num / (float)num2;
			islandNoise.radiusSquared = num * num;
		}

		private static float GetValueAt(IntVec3 cell, ModuleBase noise)
		{
			float num = -1000f;
			for (int i = 0; i < islandCones.Count; i++)
			{
				IslandCone islandCone = islandCones[i];
				float num2 = islandCone.centerX - cell.x;
				float num3 = islandCone.distZFactor * (float)(islandCone.centerZ - cell.z);
				float x = num2 * num2 + num3 * num3;
				float num4 = GenMath.LerpDouble(0f, islandCone.radiusSquared, extIslands.centerHeightValue, 0f, x);
				if (num4 > num)
				{
					num = num4;
				}
			}
			num += noise.GetValue(cell);
			if (num > extIslands.invertOver)
			{
				num -= (num - extIslands.invertOver) * 2f;
			}
			if (num < extIslands.invertUnder)
			{
				num += (extIslands.invertUnder - num) * 2f;
				if (num > extIslands.centerHeightValue)
				{
					num = extIslands.centerHeightValue;
				}
			}
			if (num < 0f)
			{
				num = 0f;
			}
			if (extIslands.invertSwap)
			{
				num = extIslands.centerHeightValue - num;
			}
			return num;
		}
	}
}
