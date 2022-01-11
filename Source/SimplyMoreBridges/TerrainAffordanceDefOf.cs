using RimWorld;
using Verse;

namespace SimplyMoreBridges;

[DefOf]
public static class TerrainAffordanceDefOf
{
    public static TerrainAffordanceDef Bridgeable;

    public static TerrainAffordanceDef BridgeableDeep;

    static TerrainAffordanceDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
    }
}