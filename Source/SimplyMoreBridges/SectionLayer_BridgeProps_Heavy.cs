using UnityEngine;
using Verse;

namespace SimplyMoreBridges;

[StaticConstructorOnStartup]
public class SectionLayer_BridgeProps_Heavy(Section section) : SectionLayer_BridgeProps(section)
{
    private static readonly Material propsLoopMat = MaterialPool.MatFrom(
        "Terrain/Misc/HeavyBridgeProps_Loop",
        ShaderDatabase.Transparent);

    private static readonly Material propsRightMat = MaterialPool.MatFrom(
        "Terrain/Misc/HeavyBridgeProps_Right",
        ShaderDatabase.Transparent);

    protected override Material PropsLoopMat => propsLoopMat;

    protected override Material PropsRightMat => propsRightMat;

    protected override bool IsTerrainThisBridge(TerrainDef terrain)
    {
        return terrain.designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_Heavy;
    }
}