using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace SimplyMoreBridges
{
    public class GenerateBridges
    {
        public static void Prefix()
        {
            var materials =
                LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>().GenerateFromAll
                    ? (from dd in DefDatabase<ThingDef>.AllDefsListForReading
                       where dd.stuffProps?.categories != null
                             && (dd.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic)
                                 || dd.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
                       select dd).ToList()
                    : (from dd in DefDatabase<ThingDef>.AllDefsListForReading
                       where dd.defName == "Plasteel" || dd.defName == "Steel" || dd.defName == "BlocksSandstone"
                             || dd.defName == "BlocksGranite" || dd.defName == "BlocksLimestone"
                             || dd.defName == "BlocksSlate" || dd.defName == "BlocksMarble"
                       select dd).ToList();
            Log.Message($"SimplyMoreBridges: Found {materials.Count} materials to generate bridge-definitions for.");

            var stonyBridgesToAdd = new List<TerrainDef>();
            var metalBridgesToAdd = new List<TerrainDef>();
            foreach (var material in materials)
            {
                try
                {
                    if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
                    {
                        stonyBridgesToAdd.Add(GenerateBridgeDef(material, true));
                        if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                            .AddVisuals)
                        {
                            stonyBridgesToAdd.Add(GenerateBridgeDef(material, true, "Flagstone"));
                        }

                        stonyBridgesToAdd.Add(GenerateBridgeDef(material, false));
                        if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                            .AddVisuals)
                        {
                            stonyBridgesToAdd.Add(GenerateBridgeDef(material, false, "Flagstone"));
                        }
                    }
                    else
                    {
                        metalBridgesToAdd.Add(GenerateBridgeDef(material, true));
                        if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                            .AddVisuals)
                        {
                            if (material.defName == "Steel")
                            {
                                metalBridgesToAdd.Add(GenerateBridgeDef(material, true, "Concrete"));
                            }

                            metalBridgesToAdd.Add(GenerateBridgeDef(material, true, "PavedTile"));
                        }

                        if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                            .GenerateFloorlike)
                        {
                            if (material.defName == "Silver")
                            {
                                metalBridgesToAdd.Add(GenerateBridgeDef(material, true, "Sterile"));
                            }
                        }

                        metalBridgesToAdd.Add(GenerateBridgeDef(material, false));
                        if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                            .AddVisuals)
                        {
                            if (material.defName == "Steel")
                            {
                                metalBridgesToAdd.Add(GenerateBridgeDef(material, false, "Concrete"));
                            }

                            metalBridgesToAdd.Add(GenerateBridgeDef(material, false, "PavedTile"));
                        }

                        if (!LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                                .GenerateFloorlike)
                        {
                            continue;
                        }

                        if (material.defName == "Silver")
                        {
                            metalBridgesToAdd.Add(GenerateBridgeDef(material, false, "Sterile"));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Warning(
                        $"SimplyMoreBridges: Failed to generate bridge definition for {material.defName}. Error: {exception}");
                }
            }

            Log.Message(
                $"SimplyMoreBridges: Generated the following stony bridges: {string.Join(",", stonyBridgesToAdd)}");
            Log.Message(
                $"SimplyMoreBridges: Generated the following metal bridges: {string.Join(",", metalBridgesToAdd)}");
            foreach (var terrainDef in stonyBridgesToAdd)
            {
                DefGenerator.AddImpliedDef(terrainDef);
            }

            foreach (var terrainDef in metalBridgesToAdd)
            {
                DefGenerator.AddImpliedDef(terrainDef);
            }
        }

        private static TerrainDef GenerateBridgeDef(ThingDef material, bool deep, string alternateTexture = "")
        {
            var currentBridgeType = new TerrainDef
                                        {
                                            edgeType = TerrainDef.TerrainEdgeType.Hard,
                                            renderPrecedence = 400,
                                            layerable = true,
                                            affordances =
                                                new List<TerrainAffordanceDef>
                                                    {
                                                        RimWorld.TerrainAffordanceDefOf.Light,
                                                        RimWorld.TerrainAffordanceDefOf.Medium,
                                                        RimWorld.TerrainAffordanceDefOf.Heavy
                                                    },
                                            designationCategory = DesignationCategoryDefOf.Structure,
                                            fertility = 0,
                                            constructEffect = EffecterDefOf.ConstructMetal,
                                            destroyBuildingsOnDestroyed = true,
                                            destroyEffect =
                                                DefDatabase<EffecterDef>.GetNamedSilentFail("Bridge_Collapse"),
                                            destroyEffectWater =
                                                DefDatabase<EffecterDef>.GetNamedSilentFail("Bridge_CollapseWater"),
                                            description =
                                                "A flat surface of the chosen material on supportive beams which can be built over water. You can even build heavy structures on these bridges, but be careful, they are still fragile. If a bridge falls, buildings on top of it fall as well.",
                                            resourcesFractionWhenDeconstructed = 0,
                                            destroyOnBombDamageThreshold = 40,
                                            statBases = new List<StatModifier>
                                                            {
                                                                new() { stat = StatDefOf.Flammability, value = 0 }
                                                            }
                                        };

            if (deep)
            {
                currentBridgeType.uiIconPath = "Terrain/Surfaces/DeepWaterBridge_MenuIcon";
                currentBridgeType.terrainAffordanceNeeded = TerrainAffordanceDefOf.BridgeableDeep;
                currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.WorkToBuild, value = 3200 });
                currentBridgeType.designatorDropdown = DesignatorDropdownGroupDefOf.Bridge_DeepWater;
                currentBridgeType.researchPrerequisites = new List<ResearchProjectDef>
                                                              {
                                                                  DefDatabase<ResearchProjectDef>.GetNamedSilentFail(
                                                                      "DeepWaterBridges")
                                                              };
                currentBridgeType.label = $"{material.label.Replace(" blocks", string.Empty)} deep water bridge";
                currentBridgeType.defName =
                    $"DeepWaterBridge{material.defName.Replace("Blocks", string.Empty)}{alternateTexture}";
            }
            else
            {
                currentBridgeType.uiIconPath = "Terrain/Surfaces/HeavyBridge_MenuIcon";
                currentBridgeType.terrainAffordanceNeeded = TerrainAffordanceDefOf.Bridgeable;
                currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.WorkToBuild, value = 2200 });
                currentBridgeType.designatorDropdown = DesignatorDropdownGroupDefOf.Bridge_Heavy;
                currentBridgeType.researchPrerequisites = new List<ResearchProjectDef>
                                                              {
                                                                  DefDatabase<ResearchProjectDef>.GetNamedSilentFail(
                                                                      "HeavyBridges")
                                                              };
                currentBridgeType.label = $"{material.label.Replace(" blocks", string.Empty)} bridge";
                currentBridgeType.defName =
                    $"HeavyBridge{material.defName.Replace("Blocks", string.Empty)}{alternateTexture}";
            }

            if (alternateTexture != "Concrete")
            {
                currentBridgeType.color = material.stuffProps.color;
            }

            float hitPoints;
            if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic))
            {
                hitPoints = 300f;
                if (string.IsNullOrEmpty(alternateTexture))
                {
                    currentBridgeType.texturePath = "Terrain/Surfaces/DeepWaterBridgeMetal";
                    if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                        .AddVisuals)
                    {
                        currentBridgeType.label += " (FloorTile)";
                    }
                }
                else
                {
                    currentBridgeType.texturePath = $"Terrain/Surfaces/Bridge{alternateTexture}";
                    currentBridgeType.label += $" ({alternateTexture})";
                }

                currentBridgeType.researchPrerequisites.Add(
                    DefDatabase<ResearchProjectDef>.GetNamedSilentFail("Smithing"));
                if (deep)
                {
                    if (material.defName == "Steel")
                    {
                        if (currentBridgeType.label.Contains("FloorTile"))
                        {
                            currentBridgeType.texturePath = "Terrain/Surfaces/BridgeMetal";
                            currentBridgeType.color = DefDatabase<TerrainDef>.GetNamedSilentFail("MetalTile").color;
                        }

                        currentBridgeType.costList = new List<ThingDefCountClass>
                                                         {
                                                             new()
                                                                 {
                                                                     thingDef = ThingDefOf.Steel,
                                                                     count = GetCustomCost(20)
                                                                 }
                                                         };
                    }
                    else
                    {
                        var baseCost = 15;
                        if (material.smallVolume)
                        {
                            currentBridgeType.texturePath = "Terrain/Surfaces/BridgeMetal";
                            switch (material.defName)
                            {
                                case "Silver":
                                    currentBridgeType.color =
                                        DefDatabase<TerrainDef>.GetNamedSilentFail("SilverTile").color;
                                    break;
                                case "Gold":
                                    currentBridgeType.color =
                                        DefDatabase<TerrainDef>.GetNamedSilentFail("GoldTile").color;
                                    break;
                            }

                            baseCost *= 10;
                        }

                        currentBridgeType.costList = new List<ThingDefCountClass>
                                                         {
                                                             new()
                                                                 {
                                                                     thingDef = ThingDefOf.Steel,
                                                                     count = GetCustomCost(5)
                                                                 },
                                                             new()
                                                                 {
                                                                     thingDef = material,
                                                                     count = GetCustomCost(baseCost)
                                                                 }
                                                         };
                    }
                }
                else
                {
                    if (material.defName == "Steel")
                    {
                        currentBridgeType.texturePath = "Terrain/Surfaces/BridgeMetal";
                        currentBridgeType.color = DefDatabase<TerrainDef>.GetNamedSilentFail("MetalTile").color;
                        currentBridgeType.costList = new List<ThingDefCountClass>
                                                         {
                                                             new()
                                                                 {
                                                                     thingDef = ThingDefOf.Steel,
                                                                     count = GetCustomCost(12)
                                                                 }
                                                         };
                    }
                    else
                    {
                        var baseCost = 9;
                        if (material.smallVolume)
                        {
                            currentBridgeType.texturePath = "Terrain/Surfaces/BridgeMetal";
                            switch (material.defName)
                            {
                                case "Silver":
                                    currentBridgeType.color =
                                        DefDatabase<TerrainDef>.GetNamedSilentFail("SilverTile").color;
                                    break;
                                case "Gold":
                                    currentBridgeType.color =
                                        DefDatabase<TerrainDef>.GetNamedSilentFail("GoldTile").color;
                                    break;
                            }

                            baseCost *= 10;
                        }

                        currentBridgeType.costList = new List<ThingDefCountClass>
                                                         {
                                                             new()
                                                                 {
                                                                     thingDef = ThingDefOf.Steel,
                                                                     count = GetCustomCost(3)
                                                                 },
                                                             new()
                                                                 {
                                                                     thingDef = material,
                                                                     count = GetCustomCost(baseCost)
                                                                 }
                                                         };
                    }
                }
            }
            else
            {
                hitPoints = 200f;
                if (string.IsNullOrEmpty(alternateTexture))
                {
                    currentBridgeType.texturePath = "Terrain/Surfaces/HeavyBridgeStone";
                    if (LoadedModManager.GetMod<SimplyMoreBridgesMod>().GetSettings<SimplyMoreBridgesSettings>()
                        .AddVisuals)
                    {
                        currentBridgeType.label += " (StoneTile)";
                    }
                }
                else
                {
                    currentBridgeType.texturePath = $"Terrain/Surfaces/Bridge{alternateTexture}";
                    currentBridgeType.label += $" ({alternateTexture})";
                }

                if (deep)
                {
                    var baseCost = 17;
                    if (material.smallVolume)
                    {
                        baseCost *= 10;
                    }

                    currentBridgeType.costList = new List<ThingDefCountClass>
                                                     {
                                                         new()
                                                             {
                                                                 thingDef = ThingDefOf.Steel, count = GetCustomCost(5)
                                                             },
                                                         new() { thingDef = material, count = GetCustomCost(baseCost) }
                                                     };
                }
                else
                {
                    var baseCost = 10;
                    if (material.smallVolume)
                    {
                        baseCost *= 10;
                    }

                    currentBridgeType.costList = new List<ThingDefCountClass>
                                                     {
                                                         new()
                                                             {
                                                                 thingDef = ThingDefOf.Steel, count = GetCustomCost(3)
                                                             },
                                                         new() { thingDef = material, count = GetCustomCost(baseCost) }
                                                     };
                }
            }

            if (material.statBases.StatListContains(StatDefOf.StuffPower_Armor_Sharp))
            {
                var sharpValue = material.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp);
                hitPoints = (float)Math.Round(500f * sharpValue);
            }

            currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.MaxHitPoints, value = hitPoints });

            currentBridgeType.tags = new List<string>();

            if (alternateTexture != "Flagstone")
            {
                currentBridgeType.tags.Add("Floor");
            }

            if (material.stuffProps.statOffsets?.Any(
                    modifier => modifier.stat == StatDefOf.Beauty && modifier.value > 5) == true)
            {
                currentBridgeType.tags.Add("FineFloor");
            }

            if (alternateTexture != "Sterile")
            {
                return currentBridgeType;
            }

            currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.Cleanliness, value = 0.6f });
            currentBridgeType.color = DefDatabase<TerrainDef>.GetNamedSilentFail("SterileTile").color;
            currentBridgeType.researchPrerequisites.Add(
                DefDatabase<ResearchProjectDef>.GetNamedSilentFail("SterileMaterials"));
            return currentBridgeType;
        }

        private static int GetCustomCost(int originalCost)
        {
            var recountedCost = originalCost * LoadedModManager.GetMod<SimplyMoreBridgesMod>()
                                    .GetSettings<SimplyMoreBridgesSettings>().CostPercent;
            return Convert.ToInt32(Math.Ceiling(recountedCost));
        }
    }
}