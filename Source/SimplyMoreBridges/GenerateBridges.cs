using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace SimplyMoreBridges;

public class GenerateBridges
{
    public static void Prefix()
    {
        var materials = SimplyMoreBridgesMod.instance.Settings.GenerateFromAll
            ? (from dd in DefDatabase<ThingDef>.AllDefsListForReading
                where dd.stuffProps?.categories != null &&
                      dd.defName != "WoodLog"
                      && (dd.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic)
                          || dd.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)
                          || dd.stuffProps.categories.Contains(StuffCategoryDefOf.Woody))
                select dd).ToList()
            : (from dd in DefDatabase<ThingDef>.AllDefsListForReading
                where dd.defName is "Plasteel" or "Steel" or "BlocksSandstone" or "BlocksGranite" or "BlocksLimestone"
                    or "BlocksSlate" or "BlocksMarble"
                select dd).ToList();
        Log.Message($"SimplyMoreBridges: Found {materials.Count} materials to generate bridge-definitions for.");

        var stonyBridgesToAdd = new HashSet<TerrainDef>();
        var metalBridgesToAdd = new HashSet<TerrainDef>();
        var woodyBridgesToAdd = new HashSet<TerrainDef>();
        foreach (var material in materials)
        {
            try
            {
                if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
                {
                    stonyBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, stonyBridgesToAdd));
                    if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
                    {
                        stonyBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, stonyBridgesToAdd,
                            "Flagstone"));
                    }

                    stonyBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, stonyBridgesToAdd));
                    if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
                    {
                        stonyBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, stonyBridgesToAdd,
                            "Flagstone"));
                    }

                    continue;
                }

                if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic))
                {
                    metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, metalBridgesToAdd));
                    if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
                    {
                        if (material.defName == "Steel")
                        {
                            metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, metalBridgesToAdd,
                                "Concrete"));
                        }

                        metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, metalBridgesToAdd,
                            "PavedTile"));
                    }

                    if (!SimplyMoreBridgesMod.instance.Settings.GenerateFloorlike)
                    {
                        continue;
                    }

                    if (material.defName == "Silver")
                    {
                        metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Bridge, metalBridgesToAdd,
                            "Sterile"));
                    }

                    metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, metalBridgesToAdd));
                    if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
                    {
                        if (material.defName == "Steel")
                        {
                            metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, metalBridgesToAdd,
                                "Concrete"));
                        }

                        metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, metalBridgesToAdd,
                            "PavedTile"));
                    }

                    if (SimplyMoreBridgesMod.instance.Settings.GenerateFloorlike)
                    {
                        if (material.defName == "Silver")
                        {
                            metalBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Deep, metalBridgesToAdd,
                                "Sterile"));
                        }
                    }

                    continue;
                }

                if (!material.stuffProps.categories.Contains(StuffCategoryDefOf.Woody))
                {
                    continue;
                }

                if (material.modExtensions?.Any(extension =>
                        extension.GetType().Name == "VFEArchitect.StuffExtension_Cost") == false)
                {
                    Log.Message(
                        $"[SimplyMoreBridges]: Skipping creating a woody bridge of {material} since its a fake-wood by added by Vanilla Furniture Expanded - Architect");
                }
                else
                {
                    woodyBridgesToAdd.Add(generateBridgeDef(material, BridgeType.Wooden, woodyBridgesToAdd));
                }
            }
            catch (Exception exception)
            {
                Log.Warning(
                    $"SimplyMoreBridges: Failed to generate bridge definition for {material.defName}. Error: {exception}");
            }
        }

        Log.Message(
            $"SimplyMoreBridges: Generated the following stony bridges:{Environment.NewLine}{string.Join(",", stonyBridgesToAdd)}");
        Log.Message(
            $"SimplyMoreBridges: Generated the following metal bridges:{Environment.NewLine}{string.Join(",", metalBridgesToAdd)}");
        Log.Message(
            $"SimplyMoreBridges: Generated the following woody bridges:{Environment.NewLine}{string.Join(",", woodyBridgesToAdd)}");

        foreach (var terrainDef in stonyBridgesToAdd)
        {
            DefGenerator.AddImpliedDef(terrainDef);
        }

        foreach (var terrainDef in metalBridgesToAdd)
        {
            DefGenerator.AddImpliedDef(terrainDef);
        }

        if (!woodyBridgesToAdd.Any())
        {
            return;
        }

        foreach (var terrainDef in woodyBridgesToAdd)
        {
            DefGenerator.AddImpliedDef(terrainDef);
        }

        TerrainDef.Named("Bridge").designatorDropdown = DesignatorDropdownGroupDefOf.Bridge;
    }

    private static TerrainDef generateBridgeDef(ThingDef material, BridgeType bridgeType,
        HashSet<TerrainDef> currentBridges,
        string alternateTexture = "")
    {
        string defName;
        string label;
        switch (bridgeType)
        {
            case BridgeType.Wooden:
                defName = $"WoodenBridge{material.defName.Replace("Blocks", string.Empty)}{alternateTexture}";
                label = $"{material.label.Replace(" blocks", string.Empty)} bridge";
                if (currentBridges.Any(def => def.defName == defName))
                {
                    defName = $"WoodenBridge{material.defName}{alternateTexture}";
                    label = $"{material.label} bridge";
                }

                break;
            case BridgeType.Bridge:
                defName = $"HeavyBridge{material.defName.Replace("Blocks", string.Empty)}{alternateTexture}";
                label = $"{material.label.Replace(" blocks", string.Empty)} bridge";
                if (currentBridges.Any(def => def.defName == defName))
                {
                    defName = $"HeavyBridge{material.defName}{alternateTexture}";
                    label = $"{material.label} bridge";
                }

                break;
            case BridgeType.Deep:
                defName = $"DeepWaterBridge{material.defName.Replace("Blocks", string.Empty)}{alternateTexture}";
                label = $"{material.label.Replace(" blocks", string.Empty)} deep water bridge";
                if (currentBridges.Any(def => def.defName == defName))
                {
                    defName = $"DeepWaterBridge{material.defName}{alternateTexture}";
                    label = $"{material.label} deep water bridge";
                }

                break;
            default:
                return null;
        }

        var currentBridgeType = new TerrainDef
        {
            defName = defName,
            label = label,
            edgeType = TerrainDef.TerrainEdgeType.Hard,
            renderPrecedence = 400,
            autoRebuildable = true,
            bridge = true,
            layerable = true,
            isPaintable = true,
            affordances =
            [
                RimWorld.TerrainAffordanceDefOf.Light,
                RimWorld.TerrainAffordanceDefOf.Medium,
                RimWorld.TerrainAffordanceDefOf.Heavy
            ],
            designationCategory = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail("Structure"),
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
            statBases = [new StatModifier { stat = StatDefOf.Flammability, value = 0 }]
        };

        var hitPoints = 100f;
        switch (bridgeType)
        {
            case BridgeType.Wooden:
                currentBridgeType.uiIconPath = "Terrain/Surfaces/Bridge_MenuIcon";
                currentBridgeType.texturePath = "Terrain/Surfaces/Bridge";
                currentBridgeType.terrainAffordanceNeeded = TerrainAffordanceDefOf.Bridgeable;
                currentBridgeType.statBases =
                [
                    new StatModifier { stat = StatDefOf.Flammability, value = 0.8f },
                    new StatModifier { stat = StatDefOf.WorkToBuild, value = 1500 }
                ];
                currentBridgeType.designatorDropdown = DesignatorDropdownGroupDefOf.Bridge;
                currentBridgeType.affordances =
                    [RimWorld.TerrainAffordanceDefOf.Light];

                currentBridgeType.costList = GetCost(true, false, material);
                break;
            case BridgeType.Bridge:
                currentBridgeType.uiIconPath = "Terrain/Surfaces/HeavyBridge_MenuIcon";
                currentBridgeType.terrainAffordanceNeeded = TerrainAffordanceDefOf.Bridgeable;
                currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.WorkToBuild, value = 2200 });
                currentBridgeType.designatorDropdown = DesignatorDropdownGroupDefOf.Bridge_Heavy;
                currentBridgeType.researchPrerequisites =
                [
                    DefDatabase<ResearchProjectDef>.GetNamedSilentFail(
                        "HeavyBridges")
                ];
                break;
            case BridgeType.Deep:
                currentBridgeType.uiIconPath = "Terrain/Surfaces/DeepWaterBridge_MenuIcon";
                currentBridgeType.terrainAffordanceNeeded = TerrainAffordanceDefOf.BridgeableDeep;
                currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.WorkToBuild, value = 3200 });
                currentBridgeType.designatorDropdown = DesignatorDropdownGroupDefOf.Bridge_DeepWater;
                currentBridgeType.researchPrerequisites =
                [
                    DefDatabase<ResearchProjectDef>.GetNamedSilentFail(
                        "DeepWaterBridges")
                ];
                break;
        }

        if (alternateTexture != "Concrete" && alternateTexture != "PavedTile")
        {
            var tile = DefDatabase<TerrainDef>.GetNamedSilentFail(
                $"Tile{material.defName.Replace("Blocks", string.Empty)}");
            currentBridgeType.color = tile?.color ?? material.stuffProps.color;
        }

        if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic))
        {
            hitPoints = 300f;

            currentBridgeType.researchPrerequisites.Add(
                DefDatabase<ResearchProjectDef>.GetNamedSilentFail("Smithing"));
            currentBridgeType.texturePath = "Terrain/Surfaces/BridgeMetal";
            switch (bridgeType)
            {
                case BridgeType.Bridge:
                    if (material.defName == "Steel")
                    {
                        if (alternateTexture != "Concrete" && alternateTexture != "PavedTile")
                        {
                            currentBridgeType.color = DefDatabase<TerrainDef>.GetNamedSilentFail("MetalTile").color;
                        }

                        currentBridgeType.costList = GetCost(true, false, null);
                    }
                    else
                    {
                        if (material.smallVolume)
                        {
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
                        }

                        currentBridgeType.costList = GetCost(true, false, material);
                    }

                    break;
                case BridgeType.Deep:
                    if (material.defName == "Steel")
                    {
                        if (currentBridgeType.label.Contains("FloorTile"))
                        {
                            currentBridgeType.color = DefDatabase<TerrainDef>.GetNamedSilentFail("MetalTile").color;
                        }

                        currentBridgeType.costList = GetCost(true, true, null);
                    }
                    else
                    {
                        if (material.smallVolume)
                        {
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
                        }

                        currentBridgeType.costList = GetCost(true, true, material);
                    }

                    break;
            }

            if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
            {
                if (string.IsNullOrEmpty(alternateTexture))
                {
                    currentBridgeType.label += " (FloorTile)";
                }
                else
                {
                    currentBridgeType.texturePath = $"Terrain/Surfaces/Bridge{alternateTexture}";
                    currentBridgeType.label += $" ({alternateTexture})";
                }
            }
        }

        if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Stony))
        {
            hitPoints = 200f;
            if (string.IsNullOrEmpty(alternateTexture))
            {
                currentBridgeType.texturePath = "Terrain/Surfaces/HeavyBridgeStone";
                if (SimplyMoreBridgesMod.instance.Settings.AddVisuals)
                {
                    currentBridgeType.label += " (StoneTile)";
                }
            }
            else
            {
                currentBridgeType.texturePath = $"Terrain/Surfaces/Bridge{alternateTexture}";
                currentBridgeType.label += $" ({alternateTexture})";
            }

            switch (bridgeType)
            {
                case BridgeType.Bridge:
                    currentBridgeType.costList = GetCost(false, false, material);
                    break;
                case BridgeType.Deep:
                    currentBridgeType.costList = GetCost(false, true, material);
                    break;
            }
        }

        if (material.statBases.StatListContains(StatDefOf.StuffPower_Armor_Sharp))
        {
            var sharpValue = material.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp);
            hitPoints = (float)Math.Round(500f * sharpValue);
        }

        currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.MaxHitPoints, value = hitPoints });

        currentBridgeType.tags = [];

        if (alternateTexture != "Flagstone")
        {
            currentBridgeType.tags.Add("Floor");
        }

        if (material.stuffProps.statOffsets?.Any(modifier => modifier.stat == StatDefOf.Beauty) == true)
        {
            var beauty = material.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);

            currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.Beauty, value = beauty });
            if (beauty > 5)
            {
                currentBridgeType.tags.Add("FineFloor");
            }
        }
        else
        {
            currentBridgeType.statBases.Add(new StatModifier { stat = StatDefOf.Beauty, value = 1 });
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

    public static List<ThingDefCountClass> GetCost(bool metalOrWood, bool deep, ThingDef material)
    {
        var total = 13;
        if (metalOrWood)
        {
            total = 12;
        }

        var steel = (int)Math.Round(total * SimplyMoreBridgesMod.instance.Settings.SteelPercent);
        int materialCost;
        if (!deep)
        {
            total = getCustomCost(total);
            steel = getCustomCost(steel);
            if (material == null)
            {
                return [new ThingDefCountClass(ThingDefOf.Steel, total)];
            }

            if (material.stuffProps.categories.Contains(StuffCategoryDefOf.Woody))
            {
                return [new ThingDefCountClass(material, total)];
            }

            materialCost = total - steel;
            if (material.smallVolume)
            {
                materialCost *= 10;
            }

            return [new ThingDefCountClass(material, materialCost), new ThingDefCountClass(ThingDefOf.Steel, steel)];
        }

        total = 22;
        if (metalOrWood)
        {
            total = 20;
        }

        steel = (int)Math.Round(total * SimplyMoreBridgesMod.instance.Settings.SteelPercentDeep);
        total = getCustomCost(total);
        steel = getCustomCost(steel);

        if (material == null)
        {
            return [new ThingDefCountClass(ThingDefOf.Steel, total)];
        }


        materialCost = total - steel;
        if (material.smallVolume)
        {
            materialCost *= 10;
        }

        return [new ThingDefCountClass(material, materialCost), new ThingDefCountClass(ThingDefOf.Steel, steel)];
    }

    private static int getCustomCost(int originalCost)
    {
        var recountedCost = originalCost * SimplyMoreBridgesMod.instance.Settings.CostPercent;
        return Convert.ToInt32(Math.Ceiling(recountedCost));
    }

    private enum BridgeType
    {
        Wooden,
        Bridge,
        Deep
    }
}