using System.Reflection;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges
{
    // Token: 0x02000008 RID: 8
    [StaticConstructorOnStartup]
	internal class HarmonyPatches
	{
		// Token: 0x06000010 RID: 16 RVA: 0x0000278C File Offset: 0x0000098C
		static HarmonyPatches()
		{
			var harmonyInstance = new Harmony("rimworld.lanilor.simplymorebridges");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
