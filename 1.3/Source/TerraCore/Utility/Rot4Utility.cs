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
	public static class Rot4Utility
	{
		public static Rot4 RandomButExclude(Rot4 exclude)
		{
			Rot4 random;
			do
			{
				random = Rot4.Random;
			}
			while (random == exclude);
			return random;
		}
	}
}
