using System;
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
    private static SimplyMoreBridgesMod instance;

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
        new Harmony("mlie.simplymorebridges").Patch(original, new HarmonyMethod(prefix));
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
        var currentPercent = Math.Round(Settings.CostPercent * 100);
        listing_Standard.Label("SimplyMoreBridges.BridgeCostPercent".Translate(currentPercent));
        Settings.CostPercent = listing_Standard.Slider(Settings.CostPercent, 0.01f, 2f);
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