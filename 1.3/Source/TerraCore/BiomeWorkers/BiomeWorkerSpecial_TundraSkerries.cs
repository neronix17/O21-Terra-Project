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
	public class BiomeWorkerSpecial_TundraSkerries : BiomeWorkerSpecial
	{
		private const int BiomeChangeDigLengthMin = 5;

		private const float BiomeChangeChance = 0.3f;

		public static readonly FloatRange WLSizeFactor = new FloatRange(1f, 1.2f);

		public static readonly FloatRange WLPosOffset = new FloatRange(0f, 0.06f);

		private static readonly IntRange BiomeChangeDigLengthMax = new IntRange(8, 20);

		protected override float InitialGenChance => 0.05f;

		protected override float GenChanceOffsetAfterFirstHit => 0.042f;

		protected override float GenChancePerHitFactor => 0.7f;

		public override bool MinPreRequirements(Tile tile)
		{
			return tile.WaterCovered;
		}

		public override bool PreRequirements(Tile tile)
		{
			if (!tile.WaterCovered || tile.elevation > -20f || tile.elevation < -130f)
			{
				return false;
			}
			if (tile.temperature > 8f)
			{
				return false;
			}
			if (BiomeWorker_IceSheet.PermaIceScore(tile) > 20f)
			{
				return false;
			}
			return true;
		}

		public override void PostGeneration(int tileID)
		{
			int randomInRange = BiomeChangeDigLengthMax.RandomInRange;
			DigTilesForBiomeChange(tileID, 5, randomInRange, 1);
		}

		protected override void ChangeTileAfterSuccessfulDig(Tile tile, bool end)
		{
			if (Rand.Value < 0.3f)
			{
				tile.biome = TerraCore.BiomeDefOf.TundraSkerries;
				GenWorldGen.UpdateTileByBiomeModExts(tile);
			}
		}

		public override WLTileGraphicData GetWLTileGraphicData(WorldGrid grid, int tileID)
		{
			return new WLTileGraphicData(TerraCore.WorldMaterials.TundraSkerries, WLSizeFactor, WLPosOffset);
		}
	}
}
