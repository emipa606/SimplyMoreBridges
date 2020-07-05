using Verse;

namespace SimplyMoreBridges
{
    class SimplyMoreBridgesModExt : DefModExtension
    {
        // wood or concrete 
        public string foundationType = "wood";
        // Diggable or Bridgeable
        public string terrainAffordanceDefName = "Bridgeable";
        // Gravel or Marsh
        public string defaultTerrainDefName = "Marsh";
    }
}