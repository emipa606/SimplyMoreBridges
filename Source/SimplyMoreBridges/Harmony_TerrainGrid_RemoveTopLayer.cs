using HarmonyLib;
using Verse;

namespace SimplyMoreBridges;

[HarmonyPatch(typeof(TerrainGrid), "RemoveTopLayer", typeof(IntVec3), typeof(bool))]
public static class Harmony_TerrainGrid_RemoveTopLayer
{
    public static bool Prefix(ref TerrainGrid __instance, IntVec3 c, bool doLeavings = true)
    {
        var terrainDef = __instance.TerrainAt(c);
        if (Prefs.DevMode)
        {
            Log.Message($"terrainDef: {terrainDef}");
        }

        if (!terrainDef.defName.StartsWith("DeepWaterBridge"))
        {
            return true;
        }

        __instance.SetTerrain(c, TerrainDef.Named("WaterDeep"));
        return false;
    }
}