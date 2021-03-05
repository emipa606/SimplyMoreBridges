using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch("SetTerrain")]
    [HarmonyPatch(typeof(TerrainGrid))]
    public class Harmony_TerrainGrid_SetTerrain
    {
        // Token: 0x0600001C RID: 28 RVA: 0x000028CC File Offset: 0x00000ACC
        private static bool SetTerrainHelper(TerrainDef terrain)
        {
            return terrain.passability != Traversability.Impassable ||
                   terrain.affordances.Contains(TerrainAffordanceDefOf.BridgeableDeep);
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00002BE0 File Offset: 0x00000DE0
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var i = 0;
            var iLen = instructions.Count();
            while (i < iLen)
            {
                var ci = instructions.ElementAt(i);
                if (ci.opcode == OpCodes.Ldelem_Ref && instructions.ElementAt(i + 1).opcode == OpCodes.Ldfld &&
                    instructions.ElementAt(i + 2).opcode == OpCodes.Ldc_I4_2 &&
                    instructions.ElementAt(i + 3).opcode == OpCodes.Beq)
                {
                    yield return ci;
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(Harmony_TerrainGrid_SetTerrain), "SetTerrainHelper"));
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
    }
}