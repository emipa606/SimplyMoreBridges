using RimWorld;
using Verse;

namespace SimplyMoreBridges;

[DefOf]
public static class DesignatorDropdownGroupDefOf
{
    public static DesignatorDropdownGroupDef Bridge;

    public static DesignatorDropdownGroupDef Bridge_Heavy;

    public static DesignatorDropdownGroupDef Bridge_DeepWater;

    static DesignatorDropdownGroupDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DesignatorDropdownGroupDefOf));
    }
}