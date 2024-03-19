using HarmonyLib;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimplyMoreBridges;

[StaticConstructorOnStartup]
internal class SimplyMoreBridgesMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SimplyMoreBridgesMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    public readonly SimplyMoreBridgesSettings Settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SimplyMoreBridgesMod(ModContentPack content)
        : base(content)
    {
        instance = this;
        var original = typeof(DefGenerator).GetMethod("GenerateImpliedDefs_PreResolve");
        Settings = GetSettings<SimplyMoreBridgesSettings>();
        var prefix = typeof(GenerateBridges).GetMethod("Prefix");
        new HarmonyLib.Harmony("mlie.simplymorebridges").Patch(original, new HarmonyMethod(prefix));
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Label("SimplyMoreBridges.RestartInfo".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled(
            "SimplyMoreBridges.AddVisualsLabel".Translate(),
            ref Settings.AddVisuals,
            "SimplyMoreBridges.AddVisualsTooltip".Translate());
        listing_Standard.CheckboxLabeled(
            "SimplyMoreBridges.GenerateFromAllLabel".Translate(),
            ref Settings.GenerateFromAll,
            "SimplyMoreBridges.GenerateFromAllTooltip".Translate());
        if (Settings.GenerateFromAll)
        {
            listing_Standard.CheckboxLabeled(
                "SimplyMoreBridges.GenerateFloorlikeLabel".Translate(),
                ref Settings.GenerateFloorlike,
                "SimplyMoreBridges.GenerateFloorlikeTooltip".Translate());
        }

        listing_Standard.Gap();
        listing_Standard.Label(
            "SimplyMoreBridges.BridgeCostPercent".Translate(Settings.CostPercent.ToStringPercent("F0")));
        Settings.CostPercent = listing_Standard.Slider(Settings.CostPercent, 0.01f, 2f);
        var stoneBridge = GenerateBridges.GetCost(false, false, ThingDefOf.BlocksGranite);
        var metalBridge = GenerateBridges.GetCost(true, false, ThingDefOf.Steel);
        var stoneDeepBridge = GenerateBridges.GetCost(false, true, ThingDefOf.BlocksGranite);
        var metalDeepBridge = GenerateBridges.GetCost(true, true, ThingDefOf.Steel);

        listing_Standard.Gap();
        listing_Standard.Label("SimplyMoreBridges.SteelPercent".Translate(Settings.SteelPercent.ToStringPercent("F0"),
            stoneBridge[0].count, stoneBridge[1].count, metalBridge[0].count, metalBridge[1].count));
        Settings.SteelPercent = listing_Standard.Slider(Settings.SteelPercent, 0.05f, 0.95f);

        listing_Standard.Gap();
        listing_Standard.Label("SimplyMoreBridges.SteelPercentDeep".Translate(
            Settings.SteelPercentDeep.ToStringPercent("F0"), stoneDeepBridge[0].count, stoneDeepBridge[1].count,
            metalDeepBridge[0].count, metalDeepBridge[1].count));
        Settings.SteelPercentDeep = listing_Standard.Slider(Settings.SteelPercentDeep, 0.03f, 0.97f);

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SimplyMoreBridges.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Simply More Bridges";
    }

    public override void WriteSettings()
    {
        if (!Settings.GenerateFromAll)
        {
            Settings.GenerateFloorlike = false;
        }

        base.WriteSettings();
    }
}