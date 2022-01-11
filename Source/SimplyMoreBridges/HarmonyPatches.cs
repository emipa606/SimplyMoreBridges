using System.Reflection;
using HarmonyLib;
using Verse;

namespace SimplyMoreBridges;

[StaticConstructorOnStartup]
internal class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmonyInstance = new Harmony("rimworld.lanilor.simplymorebridges");
        harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}