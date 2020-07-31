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
            var postfix = typeof(GenerateBridges).GetMethod("Postfix");
            new Harmony("mlie.simplymorebridges").Patch(original, postfix: new HarmonyMethod(postfix));
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
            set
            {
                settings = value;
            }
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
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Label("Changes require a game restart for effect.");
            listing_Standard.Gap();
            listing_Standard.CheckboxLabeled("Generate bridges from more materials", ref Settings.GenerateFromAll, "Will generate bridges from all loaded Stoney and Metaly materials.");
            listing_Standard.End();
            Settings.Write();
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
