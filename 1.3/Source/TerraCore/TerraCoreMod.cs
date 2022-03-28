using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;
using System.Reflection;
using RimWorld.Planet;

namespace TerraCore
{
    public class TerraCoreMod : Mod
    {
        public TerraCoreMod mod;
        public TerraCoreSettings settings;

        public TerraCoreMod(ModContentPack content) : base(content)
        {
            this.mod = this;
            this.settings = GetSettings<TerraCoreSettings>();

            Harmony harmony = new Harmony("rimworld.neronix17.terraproject");
            harmony.PatchAll();
            harmony.Patch((MethodBase)AccessTools.Method(AccessTools.TypeByName("CachedTileTemperatureData"), "CalculateOutdoorTemperatureAtTile", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(Harmony_CachedTileTemperatureData_CalculateOutdoorTemperatureAtTile), "Postfix", (Type[])null), (HarmonyMethod)null);
            //harmony.Patch((MethodBase)AccessTools.Method(typeof(WorldLayer_Hills).GetNestedTypes(BindingFlags.Instance | BindingFlags.NonPublic).First(), "MoveNext", (Type[])null, (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, new HarmonyMethod(typeof(Harmony_WorldLayer_Hills_Regenerate), "Transpiler", (Type[])null));
        }

        public override string SettingsCategory() => "Terra Project";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
        }
    }
}
