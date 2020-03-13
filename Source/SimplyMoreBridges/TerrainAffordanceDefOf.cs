using System;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000003 RID: 3
	[DefOf]
	public static class TerrainAffordanceDefOf
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002063 File Offset: 0x00000263
		static TerrainAffordanceDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainAffordanceDefOf));
		}

		// Token: 0x0400000F RID: 15
		public static TerrainAffordanceDef Bridgeable;

		// Token: 0x04000010 RID: 16
		public static TerrainAffordanceDef BridgeableDeep;
	}
}
