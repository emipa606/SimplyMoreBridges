using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges
{

    [HarmonyPatch(typeof(TerrainGrid), "RemoveTopLayer", new Type[] { typeof(IntVec3), typeof(bool) })]
    public static class Harmony_TerrainGrid_RemoveTopLayer
    {
        public static bool Prefix(ref TerrainGrid __instance, IntVec3 c, bool doLeavings = true)
        {
            var terrainDef = __instance.TerrainAt(c);
            if (Prefs.DevMode) Log.Message($"terrainDef: {terrainDef}");
            if (terrainDef == TerrainDefOf.DeepWaterBridgeSteel ||
                terrainDef == TerrainDefOf.DeepWaterBridgePlasteel ||
                terrainDef == TerrainDefOf.DeepWaterBridgeSandstone ||
                terrainDef == TerrainDefOf.DeepWaterBridgeGranite ||
                terrainDef == TerrainDefOf.DeepWaterBridgeLimestone ||
                terrainDef == TerrainDefOf.DeepWaterBridgeSlate ||
                terrainDef == TerrainDefOf.DeepWaterBridgeMarble)
            {
                __instance.SetTerrain(c, TerrainDef.Named("WaterDeep"));
                return false;
            }
            return true;
        }
    }
}
