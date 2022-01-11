using Verse;

namespace SimplyMoreBridges;

internal class SimplyMoreBridgesModExt : DefModExtension
{
    // Gravel or Marsh
    public string defaultTerrainDefName = "Marsh";

    // wood or concrete 
    public string foundationType = "wood";

    // Diggable or Bridgeable
    public string terrainAffordanceDefName = "Bridgeable";
}