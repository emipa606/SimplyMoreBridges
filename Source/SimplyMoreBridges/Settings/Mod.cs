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
        /// Cunstructor
        /// </summary>
        /// <param name="content"></param>
        public SimplyMoreBridgesMod(ModContentPack content) : base(content)
        {
            instance = this;
            var original = typeof(DefGenerator).GetMethod("GenerateImpliedDefs_PreResolve");
            var prefix = typeof(GenerateBridges).GetMethod("Prefix");
            new Harmony("mlie.simplymorebridges").Patch(original, prefix: new HarmonyMethod(prefix));
        }

        /// <summary>
        /// The instance-settings for the mod
        /// </summary>
        internal SimplyMoreBridgesSettings Settings
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
        /// The title for the mod-settings
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return "Simply More Bridges";
        }

        /// <summary>
        /// The settings-window
        /// For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
        /// </summary>
        /// <param name="rect"></param>
        public override void DoSettingsWindowContents(Rect rect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Label("Changes require a game restart for effect.");
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled("Add different visuals", ref Settings.AddVisuals, "Also adds bridges with Concrete, Flagstone and Paved Tile-visuals.");
            listing_Standard.CheckboxLabeled("Generate bridges from more materials", ref Settings.GenerateFromAll, "Will generate bridges from all loaded Stoney and Metaly materials.");
            if (Settings.GenerateFromAll)
            {
                listing_Standard.CheckboxLabeled("Generate sterile bridges", ref Settings.GenerateFloorlike, "Will add sterile bridges made of silver.");
            }
            listing_Standard.Gap();
            var currentPercent = System.Math.Round(Settings.CostPercent * 100);
            listing_Standard.Label($"Cost of bridges in percent: {currentPercent}%");
            Settings.CostPercent = listing_Standard.Slider(Settings.CostPercent, 0.01f, 2f);

            listing_Standard.End();
            Settings.Write();
        }

        public override void WriteSettings()
        {
            if(!Settings.GenerateFromAll)
            {
                Settings.GenerateFloorlike = false;
            }
            base.WriteSettings();
        }

        /// <summary>
        /// The instance of the settings to be read by the mod
        /// </summary>
        public static SimplyMoreBridgesMod instance;

        /// <summary>
        /// The private settings
        /// </summary>
        private SimplyMoreBridgesSettings settings;

    }
}
