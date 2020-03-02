using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000008 RID: 8
	[StaticConstructorOnStartup]
	internal class Harmony
	{
		// Token: 0x06000010 RID: 16 RVA: 0x0000278C File Offset: 0x0000098C
		static Harmony()
		{
			var harmonyInstance = new HarmonyLib.Harmony("rimworld.lanilor.simplymorebridges");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
