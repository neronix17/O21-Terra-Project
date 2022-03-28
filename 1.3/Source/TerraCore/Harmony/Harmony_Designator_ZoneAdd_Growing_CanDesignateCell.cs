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
	[HarmonyPatch(typeof(Designator_ZoneAdd_Growing), "CanDesignateCell")]
	public class Harmony_Designator_ZoneAdd_Growing_CanDesignateCell
	{
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			int i = 0;
			for (int iLen = instructions.Count(); i < iLen; i++)
			{
				CodeInstruction ci = instructions.ElementAt(i);
				if (ci.opcode == OpCodes.Ldsfld && instructions.ElementAt(i - 1).opcode == OpCodes.Callvirt && instructions.ElementAt(i + 3).opcode == OpCodes.Bge_Un)
				{
					yield return new CodeInstruction(OpCodes.Ldc_R4, (object)0f);
					yield return new CodeInstruction(OpCodes.Bgt_Un, instructions.ElementAt(i + 3).operand);
					i += 3;
				}
				else
				{
					yield return ci;
				}
			}
		}
	}
}
