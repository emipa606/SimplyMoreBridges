using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch("CanDesignateCell")]
	[HarmonyPatch(typeof(Designator_RemoveBridge))]
	public class Harmony_Designator_RemoveBridge_CanDesignateCell
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000023C0 File Offset: 0x000005C0
		private static bool DesignateCellHelper(IntVec3 c, Map map)
		{
			bool result;
			if (c.GetTerrain(map) == RimWorld.TerrainDefOf.Bridge)
			{
				result = true;
			}
			else
			{
				DesignatorDropdownGroupDef designatorDropdown = c.GetTerrain(map).designatorDropdown;
				result = (designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_Heavy || designatorDropdown == DesignatorDropdownGroupDefOf.Bridge_DeepWater);
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000026EC File Offset: 0x000008EC
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			int i = 0;
			int iLen = instructions.Count<CodeInstruction>();
			while (i < iLen)
			{
				CodeInstruction ci = instructions.ElementAt(i);
				if (ci.opcode == OpCodes.Call && instructions.ElementAt(i + 1).opcode == OpCodes.Ldsfld && instructions.ElementAt(i + 2).opcode == OpCodes.Beq && instructions.ElementAt(i - 1).opcode == OpCodes.Call && instructions.ElementAt(i - 4).opcode == OpCodes.Brfalse)
				{
					yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Harmony_Designator_RemoveBridge_CanDesignateCell), "DesignateCellHelper", null, null));
					yield return new CodeInstruction(OpCodes.Brtrue, instructions.ElementAt(i + 2).operand);
					i += 2;
				}
				else
				{
					yield return ci;
				}
				i++;
			}
			yield break;
		}
	}
}
