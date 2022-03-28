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
	public class BiomeWorker_Empty : BiomeWorker
	{
		public override float GetScore(Tile tile, int tileID)
		{
			return -100f;
		}
	}
}
