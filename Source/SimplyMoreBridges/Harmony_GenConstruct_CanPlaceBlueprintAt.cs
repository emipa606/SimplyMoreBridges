using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreBridges;

[HarmonyPatch("CanPlaceBlueprintAt")]
[HarmonyPatch(typeof(GenConstruct))]
public class Harmony_GenConstruct_CanPlaceBlueprintAt
{
    public static void Postfix(ref AcceptanceReport __result, BuildableDef entDef, IntVec3 center, Map map)
    {
        var centerDef = map.terrainGrid.TerrainAt(center);
        if (!(entDef is TerrainDef) || entDef.HasModExtension<SimplyMoreBridgesModExt>() ||
            centerDef.HasModExtension<SimplyMoreBridgesModExt>())
        {
            return;
        }

        var terrainAffordanceNeeded = centerDef.terrainAffordanceNeeded;
        var terrainAffordanceBridgeableDeep =
            DefDatabase<TerrainAffordanceDef>.AllDefs.FirstOrDefault(x => x.defName == "BridgeableDeep");
        if (terrainAffordanceBridgeableDeep == null && terrainAffordanceNeeded == TerrainAffordanceDefOf.Bridgeable)
        {
            __result = new AcceptanceReport("SimplyMoreBridges.NoFloorsOnBridges".Translate());
        }

        if (terrainAffordanceBridgeableDeep != null &&
            (terrainAffordanceNeeded == TerrainAffordanceDefOf.Bridgeable ||
             terrainAffordanceNeeded == terrainAffordanceBridgeableDeep))
        {
            __result = new AcceptanceReport("SimplyMoreBridges.NoFloorsOnBridges".Translate());
        }
    }
}