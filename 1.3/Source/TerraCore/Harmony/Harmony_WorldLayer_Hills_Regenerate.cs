using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	[HarmonyPatch(typeof(WorldLayer_Hills), "Regenerate")]
	public class Harmony_WorldLayer_Hills_Regenerate
	{
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

		[HarmonyReversePatch]
		[HarmonyPatch(typeof(WorldLayer), nameof(WorldLayer.Regenerate))]
		[MethodImpl(MethodImplOptions.NoInlining)]
		static WorldLayer BaseMethodDummy(WorldLayer_Hills instance) { return null; }

		[HarmonyPrefix]
		public static bool Prefix(WorldLayer_Hills __instance, IEnumerable<object> __result)
		{
			__result = DetourMethod(__instance, BaseMethodDummy(__instance));
			return false;
            

            //Label? jumpTarget = null;
            //int j = 0;
            //for (int num = instructions.Count(); j < num; j++)
            //{
            //	CodeInstruction val = instructions.ElementAt(j);
            //	if (val.opcode == OpCodes.Ldloc_3 && instructions.ElementAt(j + 1).opcode == OpCodes.Ldc_I4_1 && instructions.ElementAt(j + 2).opcode == OpCodes.Add && instructions.ElementAt(j + 3).opcode == OpCodes.Stloc_3)
            //	{
            //		jumpTarget = val.labels[0];
            //		break;
            //	}
            //}
            //if (!jumpTarget.HasValue)
            //{
            //	Log.Error("Transpiler Harmony_WorldLayer_Hills_Regenerate could not find needed jumpTarget.");
            //	yield break;
            //}
            //int i = 0;
            //for (int iLen = instructions.Count(); i < iLen; i++)
            //{
            //	CodeInstruction ci = instructions.ElementAt(i);
            //	if (ci.opcode == OpCodes.Stloc_S && instructions.ElementAt(i - 1).opcode == OpCodes.Callvirt && instructions.ElementAt(i - 2).opcode == OpCodes.Ldloc_3 && instructions.ElementAt(i - 3).opcode == OpCodes.Ldfld && instructions.ElementAt(i - 4).opcode == OpCodes.Ldarg_0)
            //	{
            //		yield return ci;
            //		yield return new CodeInstruction(OpCodes.Ldloc_S, (object)4);
            //		yield return new CodeInstruction(OpCodes.Call, (object)AccessTools.Method(typeof(Harmony_WorldLayer_Hills_Regenerate), "CheckSkipHelper", (Type[])null, (Type[])null));
            //		yield return new CodeInstruction(OpCodes.Brtrue, (object)jumpTarget.Value);
            //	}
            //	else
            //	{
            //		yield return ci;
            //	}
            //}
        }

        public static IEnumerable<object> DetourMethod(WorldLayer_Hills instance, WorldLayer baseClass)
        {
            foreach (object item in BaseMethodDummy(instance).Regenerate())
            {
                yield return item;
            }
            Rand.PushState();
            Rand.Seed = Find.World.info.Seed;
            WorldGrid worldGrid = Find.WorldGrid;
            int tilesCount = worldGrid.TilesCount;
            for (int i = 0; i < tilesCount; i++)
            {
                if (CheckSkipHelper(worldGrid[i]))
                {
                    break;
                }
                Material material;
                FloatRange floatRange;
                switch (worldGrid[i].hilliness)
                {
                    case Hilliness.SmallHills:
                        material = RimWorld.Planet.WorldMaterials.SmallHills;
                        floatRange = BasePosOffsetRange_SmallHills;
                        break;
                    case Hilliness.LargeHills:
                        material = RimWorld.Planet.WorldMaterials.LargeHills;
                        floatRange = BasePosOffsetRange_LargeHills;
                        break;
                    case Hilliness.Mountainous:
                        material = RimWorld.Planet.WorldMaterials.Mountains;
                        floatRange = BasePosOffsetRange_Mountains;
                        break;
                    case Hilliness.Impassable:
                        material = RimWorld.Planet.WorldMaterials.ImpassableMountains;
                        floatRange = BasePosOffsetRange_ImpassableMountains;
                        break;
                    default:
                        continue;
                }
                LayerSubMesh subMesh = instance.GetSubMesh(material);
                Vector3 tileCenter = worldGrid.GetTileCenter(i);
                Vector3 posForTangents = tileCenter;
                float magnitude = tileCenter.magnitude;
                tileCenter = (tileCenter + Rand.UnitVector3 * floatRange.RandomInRange * worldGrid.averageTileSize).normalized * magnitude;
                RimWorld.Planet.WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, posForTangents, BaseSizeRange.RandomInRange * worldGrid.averageTileSize, 0.005f, subMesh, counterClockwise: false, randomizeRotation: true, printUVs: false);
                RimWorld.Planet.WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, TexturesInAtlas.x), Rand.Range(0, TexturesInAtlas.z), TexturesInAtlas.x, TexturesInAtlas.z, subMesh);
            }
            Rand.PopState();
            instance.FinalizeMesh(MeshParts.All);
        }

		private static bool CheckSkipHelper(Tile tile)
		{
			if (tile.hilliness == Hilliness.Impassable)
			{
				return true;
			}
			if (tile.biome.WorkerSpecial() != null || tile.biome == BiomeDefOf.CaveEntrance || tile.biome == BiomeDefOf.TunnelworldCave)
			{
				return true;
			}
			return false;
		}
	}
}
