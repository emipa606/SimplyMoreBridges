using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges.Harmony;

[HarmonyPatch(typeof(TerrainGrid), nameof(TerrainGrid.SetTerrain))]
public class TerrainGrid_SetTerrain
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
    {
        var i = 0;
        var iLen = instructions.Count();
        while (i < iLen)
        {
            var ci = instructions.ElementAt(i);
            if (ci.opcode == OpCodes.Ldelem_Ref && instructions.ElementAt(i + 1).opcode == OpCodes.Ldfld
                                                && instructions.ElementAt(i + 2).opcode == OpCodes.Ldc_I4_2
                                                && instructions.ElementAt(i + 3).opcode == OpCodes.Beq)
            {
                yield return ci;
                yield return new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(typeof(TerrainGrid_SetTerrain), nameof(SetTerrainHelper)));
                yield return new CodeInstruction(OpCodes.Brfalse, instructions.ElementAt(i + 3).operand);
                i += 3;
            }
            else
            {
                yield return ci;
            }

            i++;
        }
    }

    private static bool SetTerrainHelper(TerrainDef terrain)
    {
        return terrain.passability != Traversability.Impassable
               || terrain.affordances.Contains(TerrainAffordanceDefOf.BridgeableDeep);
    }
}