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
	public static class RavineNoises
	{
		public static void WorkOnMapGenerator(Map map)
		{
			ModExt_Biome_GenStep_Ravine modExtension = map.Biome.GetModExtension<ModExt_Biome_GenStep_Ravine>();
			if (modExtension == null)
			{
				return;
			}
			string text = "ravine " + map.Biome.defName;
			double num = map.Size.x / 2;
			double num2 = map.Size.z / 2;
			float num3 = Rand.Range(modExtension.ravineWidthMin, modExtension.ravineWidthMax);
			double a = Rand.Range(modExtension.modAMin, modExtension.modAMax);
			double b = Rand.Range(modExtension.modBMin, modExtension.modBMax);
			double c = Rand.Range(modExtension.modCMin, modExtension.modCMax);
			ModuleBase source = new DistFromAxis((float)map.Size.x * num3);
			source = new CurveAxis(a, b, c, source);
			source = new ScaleBias(modExtension.slopeFactor, -1.0, source);
			if (modExtension.invert)
			{
				source = new Invert(source);
			}
			source = new Clamp(0.0, 999.0, source);
			NoiseDebugUI.StoreNoiseRender(source, text + " base");
			float num4 = (Rand.Value - 0.5f) * modExtension.relativeOffsetVariance + modExtension.relativeOffsetFixed;
			num += (double)((float)map.Size.x * num4);
			num2 += (double)((float)map.Size.z * num4);
			double num5 = (Rand.Value - 0.5f) * (float)modExtension.rotationVariance;
			Rot4 random;
			do
			{
				random = Rot4.Random;
			}
			while (random == Find.World.CoastDirectionAt(map.Tile));
			if (random == Rot4.North)
			{
				source = new Rotate(0.0, 270.0 + num5, 0.0, source);
				source = new Translate(0.0, 0.0, num2, source);
			}
			else if (random == Rot4.West)
			{
				source = new Rotate(0.0, 180.0 + num5, 0.0, source);
				source = new Translate(num, 0.0, 0.0, source);
			}
			else if (random == Rot4.South)
			{
				source = new Rotate(0.0, 90.0 + num5, 0.0, source);
				source = new Translate(0.0, 0.0, 0.0 - num2, source);
			}
			else if (random == Rot4.East)
			{
				source = new Rotate(0.0, 0.0 + num5, 0.0, source);
				source = new Translate(0.0 - num, 0.0, 0.0, source);
			}
			ModuleBase moduleBase = new ScaleBias(modExtension.noiseElevationPreScale, modExtension.noiseElevationPreOffset, source);
			NoiseDebugUI.StoreNoiseRender(moduleBase, text + " elevation");
			ModuleBase moduleBase2 = new ScaleBias(modExtension.noiseFertilityPreScale, modExtension.noiseFertilityPreOffset, source);
			NoiseDebugUI.StoreNoiseRender(moduleBase2, text + " fertility");
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			MapGenFloatGrid fertility = MapGenerator.Fertility;
			foreach (IntVec3 allCell in map.AllCells)
			{
				if (modExtension.calcElevationType == GenStepCalculationType.Set)
				{
					elevation[allCell] = moduleBase.GetValue(allCell) + modExtension.elevationPostOffset;
				}
				else if (modExtension.calcElevationType == GenStepCalculationType.Add)
				{
					elevation[allCell] += moduleBase.GetValue(allCell) + modExtension.elevationPostOffset;
				}
				if (modExtension.calcFertilityType == GenStepCalculationType.Set)
				{
					fertility[allCell] = moduleBase2.GetValue(allCell) + modExtension.fertilityPostOffset;
				}
				else if (modExtension.calcFertilityType == GenStepCalculationType.Add)
				{
					fertility[allCell] += moduleBase2.GetValue(allCell) + modExtension.fertilityPostOffset;
				}
			}
		}
	}
}
