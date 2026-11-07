# GitHub Copilot Instructions for Simply More Bridges (Continued)

## Mod Overview and Purpose

**Simply More Bridges (Continued)** is an update to Lanilor's original mod that adds various types of bridges to RimWorld. This mod takes advantage of the improved support for Asset Bundles introduced in RimWorld 1.6. The primary purpose of the mod is to enhance the building options in RimWorld by introducing new types of bridges that can be built from different materials, including those from other mods.

## Key Features and Systems

- **Splitted Research Requirements**: The research requirements for bridges have been modified to make the mod more compatible with medieval playthroughs.
- **Material Flexibility**: Players can build bridges from various materials, including those from other mods, and even generate wooden bridges from all woody materials.
- **Visual Options**: Players can build bridges using different visual styles, such as Flagstone, Paved Tile, and Concrete visuals.
- **Translation Support**: The mod includes Russian and French translations.
- **Cost Adjustment**: A slider allows players to set the cost of bridges.
- **Royalty and Fine Floor Compatibility**: All bridges (except Flagstone) are now considered Floors for Royalty, and those built from beautiful materials (Gold, Jade, etc.) are considered Fine Floors.
- **New Bridge Types**: The mod introduces heavy bridges that provide support for heavy structures and deep water bridges that can be built in deep water. These new bridges can be built from steel, plasteel, and stone types.
- **Compatibility**: The mod can be added to an existing save. To remove the mod, players must first remove all steel bridges from their world.

## Coding Patterns and Conventions

This mod follows established coding patterns and conventions used in RimWorld modding, including:

- **C# Classes and Methods**: The mod uses C# classes and methods to define and implement its functionality. Key classes include `GenerateBridges`, `HarmonyPatches`, and `DesignatorDropdownGroupDefOf`.
- **XML Definitions**: XML files are used to define various aspects of the mod, such as `TerrainDef`, `ResearchProjectDef`, and `DesignatorDropdownGroupDef`. These files are located in the `1.7/Defs` directory.
- **Harmony Patches**: Harmony patches are used to modify the behavior of RimWorld's existing code. Key harmony patches include `Designator_RemoveBridge_CanDesignateCell`, `GenConstruct_CanPlaceBlueprintAt`, and `HarmonyPatches`.

## XML Integration

The mod integrates with RimWorld using XML definitions. Key XML files include:

- **TerrainDefs_Terrain_Special.xml**: Defines the new bridge terrain types.
- **DesignatorDropdownGroupDef.xml**: Defines the designator dropdown groups for the new bridges.
- **TerrainAffordances.xml**: Defines the terrain affordance for the new bridge types.
- **ResearchProjects_Misc.xml**: Defines the research projects for the new bridges.

These XML files are located in the `1.7/Defs` directory and are parsed by RimWorld to add the new bridge types and their properties to the game.

## Harmony Patching

The mod uses Harmony to patch RimWorld's existing code. Key harmony patches include:

- **Designator_RemoveBridge_CanDesignateCell**: Modifies the behavior of the `Designator_RemoveBridge_CanDesignateCell` method to allow the removal of the new bridge types.
- **GenConstruct_CanPlaceBlueprintAt**: Modifies the behavior of the `GenConstruct_CanPlaceBlueprintAt` method to allow the placement of blueprints for the new bridge types.
- **HarmonyPatches**: A general harmony patch file that includes other patches and methods for modifying the behavior of RimWorld.

These harmony patches are located in the `Harmony` directory of the mod's source code.

## Suggestions for Copilot

To assist in the development of the Simply More Bridges (Continued) mod, here are some suggestions for using GitHub Copilot:

- **C# Code Generation**: Use Copilot to generate C# code for new classes, methods, and logic related to the mod's functionality. For example, you can use Copilot to generate a new method for calculating the cost of a bridge based on the material used.
- **XML Integration**: Use Copilot to generate XML definitions for new bridge types, terrain affordances, and research projects. For example, you can use Copilot to generate a new `TerrainDef` entry for a new heavy bridge type.
- **Harmony Patches**: Use Copilot to generate harmony patches for modifying existing RimWorld code. For example, you can use Copilot to generate a new harmony patch for modifying the behavior of the `Designator_RemoveBridge_CanDesignateCell` method to allow the removal of the new bridge types.

By following these guidelines, developers can use GitHub Copilot to enhance their modding experience and quickly generate code and definitions for the Simply More Bridges (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

