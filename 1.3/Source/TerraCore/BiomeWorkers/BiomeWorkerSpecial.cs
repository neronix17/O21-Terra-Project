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
	public abstract class BiomeWorkerSpecial : BiomeWorker
	{
		protected float genChance = 0f;

		protected virtual float InitialGenChance => 0f;

		protected virtual float GenChanceOffsetAfterFirstHit => 0f;

		protected virtual float GenChancePerHitFactor => 0f;

		protected virtual float GenChancePerHitOffset => 0f;

		public override float GetScore(Tile tile, int tileID)
		{
			return -100f;
		}

		public virtual bool MinPreRequirements(Tile tile)
		{
			return !tile.WaterCovered;
		}

		public virtual bool PreRequirements(Tile tile)
		{
			return false;
		}

		public virtual void PostGeneration(int tileID)
		{
		}

		public void ResetChance()
		{
			genChance = InitialGenChance;
		}

		public bool TryGenerateByChance()
		{
			if (Rand.Value < genChance)
			{
				if (genChance == InitialGenChance)
				{
					genChance -= GenChanceOffsetAfterFirstHit;
				}
				genChance = genChance * GenChancePerHitFactor - GenChancePerHitOffset;
				return true;
			}
			return false;
		}

		protected virtual void ChangeTileAfterSuccessfulDig(Tile tile, bool end)
		{
		}

		protected void DigTilesForBiomeChange(int startTileID, int digLengthMin, int digLengthMax, int maxDirChange, bool digBothDirections = true)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			bool flag = false;
			int tileID = startTileID;
			int dir = Rand.RangeInclusive(0, 5);
			for (int i = 0; i < digLengthMax; i++)
			{
				int dir2 = GenWorldGen.NextRandomDigDir(dir, maxDirChange);
				tileID = worldGrid.GetTileNeighborByDirection6WayInt(tileID, dir2);
				Tile tile = worldGrid[tileID];
				if ((i >= digLengthMin || !MinPreRequirements(tile)) && !PreRequirements(tile))
				{
					if (flag)
					{
						break;
					}
					tileID = startTileID;
					dir = GenWorldGen.InvertDigDir(dir);
					i = -1;
					flag = true;
					continue;
				}
				if (tile.biome.WorkerSpecial() == null)
				{
					bool end = i == digLengthMax - 1;
					ChangeTileAfterSuccessfulDig(tile, end);
				}
				if (digBothDirections && i == digLengthMax - 1 && !flag)
				{
					tileID = startTileID;
					dir = GenWorldGen.InvertDigDir(dir);
					i = -1;
					flag = true;
				}
			}
		}

		public virtual WLTileGraphicData GetWLTileGraphicData(WorldGrid grid, int tileID)
		{
			return null;
		}
	}
}
