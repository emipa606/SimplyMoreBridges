using UnityEngine;
using Verse;

namespace SimplyMoreBridges
{
    // Token: 0x02000009 RID: 9
    [StaticConstructorOnStartup]
    public class SectionLayer_BridgeProps_DeepWater : SectionLayer_BridgeProps
    {
        // Token: 0x04000013 RID: 19
        private static readonly Material propsLoopMat =
            MaterialPool.MatFrom("Terrain/Misc/DeepWaterBridgeProps_Loop", ShaderDatabase.Transparent);

        // Token: 0x04000014 RID: 20
        private static readonly Material propsRightMat =
            MaterialPool.MatFrom("Terrain/Misc/DeepWaterBridgeProps_Right", ShaderDatabase.Transparent);

        // Token: 0x06000012 RID: 18 RVA: 0x000027B9 File Offset: 0x000009B9
        public SectionLayer_BridgeProps_DeepWater(Section section) : base(section)
        {
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000013 RID: 19 RVA: 0x000027C8 File Offset: 0x000009C8
        protected override Material PropsLoopMat => propsLoopMat;

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000014 RID: 20 RVA: 0x000027E0 File Offset: 0x000009E0
        protected override Material PropsRightMat => propsRightMat;

        // Token: 0x06000015 RID: 21 RVA: 0x000027F8 File Offset: 0x000009F8
        protected override bool IsTerrainThisBridge(TerrainDef terrain)
        {
            return terrain.designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_DeepWater;
        }
    }
}