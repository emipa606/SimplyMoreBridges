using Verse;

namespace SimplyMoreBridges
{
    /// <summary>
    /// Definition of the settings for the mod
    /// </summary>
    internal class SimplyMoreBridgesSettings : ModSettings
    {
        public bool AddVisuals = false;
        public bool GenerateFromAll = false;
        public bool GenerateFloorlike = false;
        public float CostPercent = 1f;

        /// <summary>
        /// Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref AddVisuals, "AddVisuals", false, false);
            Scribe_Values.Look(ref GenerateFromAll, "GenerateFromAll", false, false);
            Scribe_Values.Look(ref GenerateFloorlike, "GenerateFloorlike", false, false);
            Scribe_Values.Look(ref CostPercent, "CostPercent", 1f, false);
        }
    }
}