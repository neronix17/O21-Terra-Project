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
	[HarmonyPatch(typeof(RockNoises), "Init")]
	public class Harmony_RockNoises_Init
	{
		public static void Postfix(Map map)
		{
			IslandNoises.Init(map);
		}
	}
}
