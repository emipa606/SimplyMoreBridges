using Verse;

namespace SimplyMoreBridges;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class SimplyMoreBridgesSettings : ModSettings
{
    public bool AddVisuals;

    public float CostPercent = 1f;

    public bool GenerateFloorlike;

    public bool GenerateFromAll;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AddVisuals, "AddVisuals");
        Scribe_Values.Look(ref GenerateFromAll, "GenerateFromAll");
        Scribe_Values.Look(ref GenerateFloorlike, "GenerateFloorlike");
        Scribe_Values.Look(ref CostPercent, "CostPercent", 1f);
    }
}