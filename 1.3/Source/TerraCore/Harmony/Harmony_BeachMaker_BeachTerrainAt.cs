using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using System.Reflection.Emit;

namespace TerraCore
{
	[HarmonyPatch(typeof(BeachMaker), "BeachTerrainAt")]
	public class Harmony_BeachMaker_BeachTerrainAt
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			int i = 0;
			for (int iLen = instructions.Count(); i < iLen; i++)
			{
				CodeInstruction ci = instructions.ElementAt(i);
				if (ci.opcode == OpCodes.Ldc_R4 && ci.operand.GetType() == typeof(float) && (float)ci.operand == 0.45f)
				{
					Label jumpTarget = il.DefineLabel();
					yield return new CodeInstruction(OpCodes.Ldc_R4, (object)0.25f);
					yield return new CodeInstruction(OpCodes.Bge_Un, (object)jumpTarget);
					yield return new CodeInstruction(OpCodes.Ldsfld, (object)AccessTools.Field(typeof(TerraCore.TerrainDefOf), "WaterOceanChestDeep"));
					yield return new CodeInstruction(OpCodes.Ret, (object)null);
					CodeInstruction val = new CodeInstruction(OpCodes.Ldloc_0, (object)null);
					val.labels = new List<Label> { jumpTarget };
					yield return val;
				}
				yield return ci;
			}
		}
	}
}
