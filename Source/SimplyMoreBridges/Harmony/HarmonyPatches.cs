using System.Reflection;
using Verse;

namespace SimplyMoreBridges.Harmony;

[StaticConstructorOnStartup]
internal class HarmonyPatches
{
    static HarmonyPatches()
    {
        new HarmonyLib.Harmony("rimworld.lanilor.simplymorebridges").PatchAll(Assembly.GetExecutingAssembly());
    }
}