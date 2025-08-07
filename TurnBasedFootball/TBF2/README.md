# Turn-Based Football Game

A sophisticated turn-based football/soccer game built with TypeScript, featuring a hexagonal grid system and supporting multiple game modes including local play, online multiplayer, and AI opponents.

## Features

### Game Mechanics
- **Hexagonal Grid Field**: 90x120 hex grid representing a football pitch
- **Turn-Based Gameplay**: Teams alternate turns with 16 movement points per turn
- **Realistic Player Management**: 11 players per team with position-based abilities
- **Movement Point System**: Strategic resource management with MP carryover
- **Ball Physics**: Ground, flying, and carried ball states
- **Multiple Actions**: Move, kick, intercept with varying MP costs

### Game Modes
- **Local Play**: Two players sharing the same browser
- **Online Multiplayer**: Real-time network play via WebSocket
- **AI Opponent**: Single-player mode with AI competitor

### Technical Features
- **TypeScript**: Type-safe development with modern ES2020+ features
- **Canvas Rendering**: Smooth 2D graphics with hex grid visualization
- **WebSocket Communication**: Real-time multiplayer synchronization
- **Modular Architecture**: Clean separation of client/server/shared code

## Quick Start

### Prerequisites
- Node.js 18+ and npm
- Modern web browser with Canvas support

### Installation & Setup

```bash
# Clone and navigate to the project
cd tfb1

# Install dependencies
npm install

# Start development server (client)
npm run dev

# Start game server (multiplayer)
npm run dev:server
```

### Development Commands

```bash
npm run build      # Production build
npm run dev        # Development client server
npm run server     # Production server
npm run dev:server # Development server with watch mode
npm run clean      # Clean build artifacts
```

## How to Play

### Basic Controls
- **Mouse Click**: Select players and target locations
- **M Key**: Switch to move mode
- **K Key**: Switch to kick mode  
- **I Key**: Switch to intercept mode
- **ESC Key**: Cancel current action
- **End Turn Button**: Finish your turn

### Game Rules
1. **Team Setup**: Each team has 11 players (1 GK, 4 DEF, 4 MID, 2 FWD)
2. **Movement Points**: Each player starts with 6 MP, team gets 16 MP total per turn
3. **Actions Cost MP**:
   - Move: Distance in hexes
   - Kick: 1 MP
   - Intercept: 1 MP (2 MP for goalkeeper)
4. **MP Carryover**: Unused MP carries over (half) to next turn
5. **Turn End**: Click "End Turn" after using MP or when done

### Strategies
- **Positioning**: Use hex distance calculations for optimal player placement
- **Resource Management**: Balance MP usage between offense and defense
- **Ball Control**: Intercept and kick strategically to maintain possession
- **Formation Play**: Coordinate players based on their positions

## Architecture

### Project Structure
```
src/
├── client/           # Browser game client
│   ├── index.html   # Game UI template
│   ├── index.ts     # Client entry point
│   ├── GameClient.ts # Main game controller
│   └── GameRenderer.ts # Canvas rendering engine
├── server/          # Node.js multiplayer server
│   └── index.ts     # WebSocket server
└── shared/          # Common game logic
    ├── HexGrid.ts   # Hexagonal coordinate system
    ├── GameLogic.ts # Core game state & rules
    └── Messages.ts  # Client-server communication
```

### Key Components

#### HexGrid System
- Axial coordinate system (q, r) for hex positioning
- Pixel coordinate conversion for rendering
- Distance calculation and pathfinding
- Field boundary validation

#### Game State Management
- Immutable state updates
- Turn-based logic with MP tracking
- Player action validation
- Ball state transitions

#### Multiplayer Architecture
- WebSocket-based real-time communication
- JSON message protocol
- Game room management
- Player synchronization

## Development

### Adding New Features
1. Define types in `src/shared/` for shared logic
2. Implement client-side in `src/client/`
3. Add server handling in `src/server/` if needed
4. Update UI and rendering as required

### Testing
```bash
# Run type checking
npx tsc --noEmit

# Check for lint errors
# (Add ESLint configuration as needed)
```

### Building for Production
```bash
npm run build
npm start
```

## Browser Compatibility
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing
This project follows TypeScript best practices with:
- Strict type checking enabled
- Interface-based design patterns
- Modular architecture
- Clean separation of concerns

## License
ISC License

## Future Enhancements
- [ ] AI player implementation
- [ ] Advanced ball physics (trajectory, bouncing)
- [ ] Player fatigue system
- [ ] Match statistics and replay
- [ ] Tournament mode
- [ ] Mobile responsive design
- [ ] Sound effects and animations
