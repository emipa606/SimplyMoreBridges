using System.Reflection;
using Verse;

namespace SimplyMoreBridges.Harmony;

[StaticConstructorOnStartup]
internal class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmonyInstance = new HarmonyLib.Harmony("rimworld.lanilor.simplymorebridges");
        harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}