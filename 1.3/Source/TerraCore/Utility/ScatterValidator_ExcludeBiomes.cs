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
	public class ScattererValidator_ExcludeBiomes : ScattererValidator
	{
		public List<BiomeDef> biomes;

		public override bool Allows(IntVec3 c, Map map)
		{
			BiomeDef biome = map.Biome;
			foreach (BiomeDef biome2 in biomes)
			{
				if (biome == biome2)
				{
					return false;
				}
			}
			return true;
		}
	}
}
