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
	public class ModExt_Biome_GenStep_Islands : DefModExtension
	{
		public float baseFrequency = 0.02f;

		public int islandCountMin = 0;

		public int islandCountMax = 0;

		public float minSizeX = 0.2f;

		public float maxSizeX = 0.3f;

		public float minSizeZ = 0.2f;

		public float maxSizeZ = 0.3f;

		public float centerHeightValue = 3f;

		public float invertOver = 1000f;

		public float invertUnder = -1000f;

		public bool invertSwap = false;

		public GenStepCalculationType calcElevationType = GenStepCalculationType.None;

		public float noiseElevationPreScale = 1f;

		public float noiseElevationPreOffset = 0f;

		public float elevationPostScale = 0.3333f;

		public float elevationPostOffset = 0f;

		public GenStepCalculationType calcFertilityType = GenStepCalculationType.None;

		public float noiseFertilityPreScale = 1f;

		public float noiseFertilityPreOffset = 0f;

		public float fertilityPostScale = 1f;

		public float fertilityPostOffset = 0f;

		public List<TerrainThresholdWEO> terrainPatchMakerByIslandFertility = null;
	}
}
