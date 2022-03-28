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
	public class ModExt_Biome_GenStep_Spikes : DefModExtension
	{
		public float baseFrequency = 0.0647f;

		public float sineX = 5f;

		public float sineZ = 5f;

		public float sineScale = 1f;

		public float sineOffset = 0f;

		public GenStepCalculationType calcElevationType = GenStepCalculationType.None;

		public float noiseElevationPreScale = 1f;

		public float noiseElevationPreOffset = 0f;

		public float elevationPostOffset = 0f;

		public GenStepCalculationType calcFertilityType = GenStepCalculationType.None;

		public float noiseFertilityPreScale = 1f;

		public float noiseFertilityPreOffset = 0f;

		public float fertilityPostOffset = 0f;
	}
}
