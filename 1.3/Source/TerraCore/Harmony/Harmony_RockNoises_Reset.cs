using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace TerraCore
{
	[HarmonyPatch(typeof(RockNoises), "Reset")]
	public class Harmony_RockNoises_Reset
	{
		public static void Postfix()
		{
			IslandNoises.Reset();
		}
	}
}
