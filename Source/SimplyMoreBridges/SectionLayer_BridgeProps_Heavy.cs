using System;
using UnityEngine;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x0200000A RID: 10
	[StaticConstructorOnStartup]
	public class SectionLayer_BridgeProps_Heavy : SectionLayer_BridgeProps
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002841 File Offset: 0x00000A41
		public SectionLayer_BridgeProps_Heavy(Section section) : base(section)
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002850 File Offset: 0x00000A50
		protected override Material PropsLoopMat
		{
			get
			{
				return SectionLayer_BridgeProps_Heavy.propsLoopMat;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002868 File Offset: 0x00000A68
		protected override Material PropsRightMat
		{
			get
			{
				return SectionLayer_BridgeProps_Heavy.propsRightMat;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002880 File Offset: 0x00000A80
		protected override bool IsTerrainThisBridge(TerrainDef terrain)
		{
			return terrain.designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_Heavy;
		}

		// Token: 0x04000015 RID: 21
		private static readonly Material propsLoopMat = MaterialPool.MatFrom("Terrain/Misc/HeavyBridgeProps_Loop", ShaderDatabase.Transparent);

		// Token: 0x04000016 RID: 22
		private static readonly Material propsRightMat = MaterialPool.MatFrom("Terrain/Misc/HeavyBridgeProps_Right", ShaderDatabase.Transparent);
	}
}
