# GitHub Copilot Instructions for Simply More Bridges (Continued)

Welcome to the development guide for the "Simply More Bridges (Continued)" mod for RimWorld. This file serves as a comprehensive guide for leveraging GitHub Copilot to enhance and maintain the mod effectively. Below, you'll find detailed instructions on the mod’s purpose, key features, coding patterns, XML integration, Harmony patching, and suggestions for using Copilot.

## Mod Overview and Purpose

"Simply More Bridges (Continued)" is a mod that expands the bridge-building capabilities within RimWorld by introducing new bridge types and materials. The mod aims to enhance gameplay flexibility, especially in medieval and other themed playthroughs, by allowing bridges to be constructed from various materials and with different visual appearances.

## Key Features and Systems

- **Expanded Material Options**: Create bridges from an array of materials, including those provided by other mods.
- **Visual Customization**: Options to build bridges using flagstone, paved tile, and concrete visuals.
- **Heavy and Deep Water Bridges**: New bridge types support heavy structures and can be built in deep water.
- **Royalty and Fine Floors**: Integration with RimWorld Royalty; beautiful materials yield fine floors.
- **Research Integration**: New research projects for heavy and deep water bridges.
- **Localizations**: Includes Russian and French translations.

## Coding Patterns and Conventions

- **Class and Method Naming**: Classes and methods are named concisely and descriptively, e.g., `Harmony_Designator_RemoveBridge_CanDesignateCell`.
- **Accessibility Modifiers**: Utilize `public`, `internal`, and `private` to designate the intended accessibility of classes and methods.
- **Inheritance**: Use of inheritance for section layer classes like `SectionLayer_BridgeProps_DeepWater`.

## XML Integration

- Use XML to define new `TerrainDef` for bridge types, ensuring compatibility with existing terrain definitions.
- XML files should maintain proper indentation and descriptive comments to ease updates and expansion.
- Designator and research projects should also be defined within XML to appear correctly in the game’s UI.

## Harmony Patching

- **Harmony Patches**: Facilitates the mod’s core functionality by patching into RimWorld's methods.
- **Patch Locations**: Files such as `Harmony_Designator_RemoveBridge_CanDesignateCell.cs` and `Harmony_GenConstruct_CanPlaceBlueprintAt.cs` handle specific functionalities like removing and placing bridges.
- **Defensive Programming**: Ensure patches are safe and account for potential edge cases to prevent game crashes.

## Suggestions for Copilot

- **Code Completion**: Use Copilot to rapidly generate boilerplate code for new classes and methods based on existing patterns.
- **XML Templates**: Leverage Copilot to quickly create and modify XML definitions for new bridge types and visual styles.
- **Harmony Integration**: Utilize Copilot to suggest and refine Harmony patches, drawing on existing code examples in the mod.
- **Documentation**: Encourage Copilot to add comments and documentation to improve code readability and maintainability.

By following these guidelines and actively utilizing GitHub Copilot, you can contribute effectively to the ongoing development and improvement of the "Simply More Bridges (Continued)" mod. Happy modding!
