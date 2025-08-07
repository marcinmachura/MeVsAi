<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# Turn-Based Football Game - Copilot Instructions

This is a TypeScript-based turn-based football/soccer game project with the following characteristics:

## Project Structure
- `src/shared/` - Shared game logic and types (hex grid, game state, messaging)
- `src/client/` - Browser-based game client with canvas rendering
- `src/server/` - Node.js WebSocket server for multiplayer support

## Key Technologies
- TypeScript for type-safe development
- HTML5 Canvas for game rendering
- WebSocket for real-time multiplayer communication
- Hexagonal grid system using axial coordinates
- Webpack for bundling and development server

## Game Mechanics
- Hexagonal grid field (90x120 hexes)
- 11 players per team with movement points (MP) system
- Ball can be on ground, flying, or with player
- Three game modes: Local, Online, AI
- Turn-based gameplay with 16 MP per team per turn

## Code Guidelines
- Use interfaces for data structures
- Implement immutable state updates
- Follow hex coordinate system conventions (q, r)
- Maintain separation between client and server logic
- Use JSON for client-server communication

## Testing Considerations
- Focus on game logic validation
- Test hex grid calculations
- Verify movement point calculations
- Test multiplayer synchronization

When working on this project, prioritize game logic correctness, clean TypeScript patterns, and maintainable architecture.
