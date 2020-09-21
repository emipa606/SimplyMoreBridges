using Verse;

namespace SimplyMoreBridges
{
    /// <summary>
    /// Definition of the settings for the mod
    /// </summary>
    internal class SimplyMoreBridgesSettings : ModSettings
    {
        public bool GenerateFromAll = false;
        public bool AddVisuals = false;

        /// <summary>
        /// Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GenerateFromAll, "GenerateFromAll", false, false);
            Scribe_Values.Look(ref AddVisuals, "AddVisuals", false, false);
        }
    }
}