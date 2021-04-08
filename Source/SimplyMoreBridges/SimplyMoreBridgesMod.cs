using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SimplyMoreBridges
{
    [StaticConstructorOnStartup]
    internal class SimplyMoreBridgesMod : Mod
    {
        /// <summary>
        ///     The instance of the settings to be read by the mod
        /// </summary>
        private static SimplyMoreBridgesMod instance;

        /// <summary>
        ///     The private settings
        /// </summary>
        private SimplyMoreBridgesSettings settings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="content"></param>
        public SimplyMoreBridgesMod(ModContentPack content)
            : base(content)
        {
            instance = this;
            var original = typeof(DefGenerator).GetMethod("GenerateImpliedDefs_PreResolve");
            var prefix = typeof(GenerateBridges).GetMethod("Prefix");
            new Harmony("mlie.simplymorebridges").Patch(original, new HarmonyMethod(prefix));
        }

        /// <summary>
        ///     The instance-settings for the mod
        /// </summary>
        private SimplyMoreBridgesSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = GetSettings<SimplyMoreBridgesSettings>();
                }

                return settings;
            }

            set => settings = value;
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

            listing_Standard.End();
            Settings.Write();
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
}