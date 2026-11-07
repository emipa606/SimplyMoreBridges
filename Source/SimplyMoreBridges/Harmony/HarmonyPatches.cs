using System.Reflection;
using Verse;

namespace SimplyMoreBridges.Harmony;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new HarmonyLib.Harmony("rimworld.lanilor.simplymorebridges").PatchAll(Assembly.GetExecutingAssembly());
    }
}