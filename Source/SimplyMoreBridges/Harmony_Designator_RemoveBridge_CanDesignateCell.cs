using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{

    [HarmonyPatch(typeof(Designator_RemoveBridge), "CanDesignateCell", new Type[] { typeof(IntVec3) })]
    public static class Harmony_Designator_RemoveBridge_CanDesignateCell
    {
        public static bool Prefix(IntVec3 c, ref AcceptanceReport __result, ref Designator_RemoveBridge __instance)
        {
            var terrainDef = c.GetTerrain(__instance.Map);
            if (Prefs.DevMode)
            {
                Log.Message($"terrainDef: {terrainDef}");
            }

            if (terrainDef.defName.StartsWith("HeavyBridge") || terrainDef.defName.StartsWith("DeepWaterBridge"))
            {
                __result = AcceptanceReport.WasAccepted;
                if (!c.InBounds(__instance.Map) || c.Fogged(__instance.Map))
                {
                    __result = false;
                }
                if (__instance.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
                {
                    __result = false;
                }
                Building edifice = c.GetEdifice(__instance.Map);
                if (edifice != null && edifice.def.Fillage == FillCategory.Full && edifice.def.passability == Traversability.Impassable)
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
            return true;
        }
    }

    //  // Token: 0x02000006 RID: 6
    //  [HarmonyPatch("CanDesignateCell")]
    //  [HarmonyPatch(typeof(Designator_RemoveBridge))]
    //  public class Harmony_Designator_RemoveBridge_CanDesignateCell
    //  {
    //      // Token: 0x0600000B RID: 11 RVA: 0x000023C0 File Offset: 0x000005C0
    //      private static bool DesignateCellHelper(IntVec3 c, Map map)
    //      {
    //          bool result;
    //          if (c.GetTerrain(map) == RimWorld.TerrainDefOf.Bridge)
    //          {
    //              return true;
    //          }
    //          else
    //          {
    //              var terrainDef = c.GetTerrain(map);
    //              if (Prefs.DevMode) Log.Message($"terrainDef: {terrainDef}");

    //          }
    //          return false;
    //          DesignatorDropdownGroupDef designatorDropdown = c.GetTerrain(map).designatorDropdown;
    //          result = (designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_Heavy || designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_DeepWater);
    //      }
    //	return result;
    //}

    //  // Token: 0x0600000C RID: 12 RVA: 0x000026EC File Offset: 0x000008EC
    //  public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    //  {
    //      int i = 0;
    //      int iLen = instructions.Count();
    //      while (i < iLen)
    //      {
    //          CodeInstruction ci = instructions.ElementAt(i);
    //          if (ci.opcode == OpCodes.Call && instructions.ElementAt(i + 1).opcode == OpCodes.Ldsfld && instructions.ElementAt(i + 2).opcode == OpCodes.Beq && instructions.ElementAt(i - 1).opcode == OpCodes.Call && instructions.ElementAt(i - 4).opcode == OpCodes.Brfalse)
    //          {
    //              yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Harmony_Designator_RemoveBridge_CanDesignateCell), "DesignateCellHelper", null, null));
    //              yield return new CodeInstruction(OpCodes.Brtrue, instructions.ElementAt(i + 2).operand);
    //              i += 2;
    //          }
    //          else
    //          {
    //              yield return ci;
    //          }
    //          i++;
    //      }
    //      yield break;
    //}
}
