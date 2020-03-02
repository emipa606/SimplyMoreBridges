using System;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000004 RID: 4
	[DefOf]
	public static class DesignatorDropdownGroupDefOf
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002076 File Offset: 0x00000276
		static DesignatorDropdownGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DesignatorDropdownGroupDefOf));
		}

		// Token: 0x04000011 RID: 17
		public static DesignatorDropdownGroupDef Bridge_Heavy;

		// Token: 0x04000012 RID: 18
		public static DesignatorDropdownGroupDef Bridge_DeepWater;
	}
}
