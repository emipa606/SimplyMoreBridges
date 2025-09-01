using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreBridges.Harmony;

[HarmonyPatch(typeof(Designator_RemoveFloor), nameof(Designator_RemoveFloor.CanDesignateCell), typeof(IntVec3))]
public static class Designator_RemoveBridge_CanDesignateCell
{
    public static bool Prefix(IntVec3 c, ref AcceptanceReport __result, ref Designator_RemoveFloor __instance)
    {
        var terrainDef = c.GetTerrain(__instance.Map);
        if (Prefs.DevMode)
        {
            Log.Message($"terrainDef: {terrainDef}");
        }

        if (!terrainDef.defName.StartsWith("HeavyBridge") && !terrainDef.defName.StartsWith("DeepWaterBridge"))
        {
            return true;
        }

        __result = AcceptanceReport.WasAccepted;
        if (!c.InBounds(__instance.Map) || c.Fogged(__instance.Map))
        {
            __result = false;
        }

        if (__instance.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
        {
            __result = false;
        }

        var edifice = c.GetEdifice(__instance.Map);
        if (edifice != null && edifice.def.Fillage == FillCategory.Full
                            && edifice.def.passability == Traversability.Impassable)
        {
            __result = false;
        }

        if (!__instance.Map.terrainGrid.CanRemoveTopLayerAt(c))
        {
            __result = "TerrainMustBeRemovable".Translate();
        }

        if (WorkGiver_ConstructRemoveFloor.AnyBuildingBlockingFloorRemoval(c, __instance.Map))
        {
            __result = false;
        }

        return false;
    }
}