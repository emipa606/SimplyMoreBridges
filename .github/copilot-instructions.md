# GitHub Copilot Instructions for Simply More Bridges (Continued) Mod

## Mod Overview and Purpose

**Mod Name**: Simply More Bridges (Continued)

The Simply More Bridges (Continued) mod is an extension and enhancement of Lanilor's original mod, compatible with RimWorld version 1.6. It introduces a variety of additional bridge types, expanding the existing functionality of wood bridges in the RimWorld core by providing more construction options for different materials and environments. The mod aims to offer improved integration with medieval playthroughs and other mods by adapting research requirements and material compatibility. 

## Key Features and Systems

- **Material Diversity**: Build bridges using a wide range of materials, including those introduced by other mods. Materials include wood, plasteel, steel, and stone types.
  
- **Visual and Functional Enhancements**: Option to build bridges with different visual styles like Flagstone, Paved Tile, and Concrete. Bridges now also recognize their material's beauty value.

- **Research**: Separate research paths for "heavy bridges" and "deep water bridges" make them compatible with different tech levels.

- **Gameplay Integration**: New bridge types that support heavy structures and building in deep water, broadening the player's construction capabilities.

- **Cost Configuration**: Players can adjust the material cost for construction using a slider.

- **Localization Support**: Includes translations for Russian and French languages.

## Coding Patterns and Conventions

- **Project Structure**: The project is organized into multiple C# files, each targeting specific functionality. Classes are generally kept small and focused on a single purpose.

- **Naming Conventions**: Classes and methods use PascalCase, while private methods and fields are maintained in camelCase. Static classes are used for utility methods.

- **Accessibility**: Class and method scopes are explicitly defined, with internal access modifiers used for non-public aspects of the mod to prevent unintended access.

## XML Integration

The mod relies on the XML definition files for integrating modded content seamlessly with the RimWorld engine:

- **Terrain Definitions**: New terrainDefs are properly added in XML to support the additional bridge types. Ensure all additions respect the game's XML schema.

- **Beauty and Affordance Values**: XML should correctly reflect the real-world attributes of materials used for bridge construction, such as beauty and support affordances.

## Harmony Patching

Harmony is used to apply runtime patches to the existing game functionality to ensure compatibility and extend features without modifying the core game code:

- **File: HarmonyPatches.cs**: This file contains the necessary patches and should follow best practices for stability. Always ensure that patches are surrounded by try-catch blocks to handle potential exceptions.

- **Patch Focus**: The patches update the game engine to facilitate new functionality such as terrain support and bridge properties.

## Suggestions for Copilot

To ensure Copilot suggestions align with the project's goals, consider the following:

- **Contextual Completion**: Leverage Copilot's ability to auto-complete based on context when writing new methods for bridge properties using existing naming conventions.

- **Code Generation**: Use Copilot to generate boilerplate code for XML parsing and Harmony patches, reducing manual coding time.

- **Refactoring Support**: Allow Copilot to suggest refactoring opportunities to improve code readability and maintainability.

- **Error Handling**: Enable Copilot to provide suggestions on implementing robust error handling patterns, especially for runtime patches.

By following these guidelines, you'll ensure that the mod maintains high code quality while providing an engaging gameplay experience for RimWorld players.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
