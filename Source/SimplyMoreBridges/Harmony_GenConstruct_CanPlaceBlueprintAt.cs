using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch("CanPlaceBlueprintAt")]
	[HarmonyPatch(typeof(GenConstruct))]
	public class Harmony_GenConstruct_CanPlaceBlueprintAt
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000271C File Offset: 0x0000091C
		public static void Postfix(ref AcceptanceReport __result, BuildableDef entDef, IntVec3 center, Map map)
		{
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				TerrainAffordanceDef terrainAffordanceNeeded = map.terrainGrid.TerrainAt(center).terrainAffordanceNeeded;
				if (terrainAffordanceNeeded == TerrainAffordanceDefOf.Bridgeable || terrainAffordanceNeeded == TerrainAffordanceDefOf.BridgeableDeep)
				{
					__result = new AcceptanceReport(Translator.Translate("NoFloorsOnBridges"));
				}
			}
		}
	}
}
