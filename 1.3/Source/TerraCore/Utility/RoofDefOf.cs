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
	[DefOf]
	public static class RoofDefOf
	{
		public static RoofDef RoofRockUncollapsable;

		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerraCore.RoofDefOf));
		}
	}
}
