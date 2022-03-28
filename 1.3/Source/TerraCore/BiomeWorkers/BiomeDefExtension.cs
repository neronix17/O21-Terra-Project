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
	public static class BiomeDefExtension
	{
		public static BiomeWorkerSpecial WorkerSpecial(this BiomeDef biome)
		{
			return biome.Worker as BiomeWorkerSpecial;
		}
	}
}
