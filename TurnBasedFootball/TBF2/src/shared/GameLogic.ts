import { HexCoordinate } from './HexGrid';

export enum Team {
  A = 'A',
  B = 'B'
}

export enum PlayerPosition {
  GOALKEEPER = 'GK',
  DEFENDER = 'DEF',
  MIDFIELDER = 'MID',
  FORWARD = 'FWD'
}

export enum BallState {
  ON_GROUND = 'ON_GROUND',
  FLYING = 'FLYING',
  WITH_PLAYER = 'WITH_PLAYER'
}

export enum GameMode {
  LOCAL = 'local',
  ONLINE = 'online',
  AI = 'ai'
}

export interface Player {
  id: string;
  team: Team;
  position: PlayerPosition;
  hexPosition: HexCoordinate;
  movementPoints: number;
  unusedMovementPoints: number;
  isSelected: boolean;
  hasBall: boolean;
}

export interface Ball {
  state: BallState;
  hexPosition?: HexCoordinate; // For ON_GROUND and WITH_PLAYER states
  pixelPosition?: { x: number; y: number }; // For FLYING state
  trajectory?: {
    start: { x: number; y: number };
    end: { x: number; y: number };
    progress: number; // 0 to 1
  };
  playerId?: string; // When WITH_PLAYER
}

export interface GameState {
  mode: GameMode;
  currentTeam: Team;
  currentTurn: number;
  players: Player[];
  ball: Ball;
  teamAMovementPoints: number;
  teamBMovementPoints: number;
  isGameActive: boolean;
  selectedPlayerId?: string;
}

/**
 * Game logic and state management
 */
export class FootballGame {
  private state: GameState;
  private readonly MAX_MOVEMENT_POINTS = 16;
  private readonly DEFAULT_PLAYER_MP = 6;

  constructor(mode: GameMode = GameMode.LOCAL) {
    this.state = this.initializeGame(mode);
  }

  /**
   * Initialize a new game
   */
  private initializeGame(mode: GameMode): GameState {
    const players = this.createInitialPlayers();
    const ball = this.createInitialBall();

    return {
      mode,
      currentTeam: Team.A,
      currentTurn: 1,
      players,
      ball,
      teamAMovementPoints: this.MAX_MOVEMENT_POINTS,
      teamBMovementPoints: this.MAX_MOVEMENT_POINTS,
      isGameActive: true,
      selectedPlayerId: undefined
    };
  }

  /**
   * Create initial player positions (11 players per team)
   * Adjusted for 60x45 field (rotated for left-right play)
   */
  private createInitialPlayers(): Player[] {
    const players: Player[] = [];
    
    // Team A formation (4-4-2) - positioned on left side
    const teamAPositions: { position: PlayerPosition; hex: HexCoordinate }[] = [
      { position: PlayerPosition.GOALKEEPER, hex: { q: 5, r: 22 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 12, r: 10 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 12, r: 18 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 12, r: 26 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 12, r: 34 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 20, r: 10 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 20, r: 18 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 20, r: 26 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 20, r: 34 } },
      { position: PlayerPosition.FORWARD, hex: { q: 27, r: 18 } },
      { position: PlayerPosition.FORWARD, hex: { q: 27, r: 26 } }
    ];

    // Team B formation (4-4-2) - positioned on right side (mirrored)
    const teamBPositions: { position: PlayerPosition; hex: HexCoordinate }[] = [
      { position: PlayerPosition.GOALKEEPER, hex: { q: 54, r: 22 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 47, r: 10 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 47, r: 18 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 47, r: 26 } },
      { position: PlayerPosition.DEFENDER, hex: { q: 47, r: 34 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 39, r: 10 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 39, r: 18 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 39, r: 26 } },
      { position: PlayerPosition.MIDFIELDER, hex: { q: 39, r: 34 } },
      { position: PlayerPosition.FORWARD, hex: { q: 32, r: 18 } },
      { position: PlayerPosition.FORWARD, hex: { q: 32, r: 26 } }
    ];

    // Create Team A players
    teamAPositions.forEach((pos, index) => {
      players.push({
        id: `A${index + 1}`,
        team: Team.A,
        position: pos.position,
        hexPosition: pos.hex,
        movementPoints: this.DEFAULT_PLAYER_MP,
        unusedMovementPoints: 0,
        isSelected: false,
        hasBall: false
      });
    });

    // Create Team B players
    teamBPositions.forEach((pos, index) => {
      players.push({
        id: `B${index + 1}`,
        team: Team.B,
        position: pos.position,
        hexPosition: pos.hex,
        movementPoints: this.DEFAULT_PLAYER_MP,
        unusedMovementPoints: 0,
        isSelected: false,
        hasBall: false
      });
    });

    return players;
  }

  /**
   * Create initial ball at center of field
   */
  private createInitialBall(): Ball {
    return {
      state: BallState.ON_GROUND,
      hexPosition: { q: 30, r: 22 } // Center of 60x45 field
    };
  }

  /**
   * Get current game state
   */
  getState(): GameState {
    return { ...this.state };
  }

  /**
   * Select a player
   */
  selectPlayer(playerId: string): boolean {
    const player = this.state.players.find(p => p.id === playerId);
    if (!player || player.team !== this.state.currentTeam) {
      return false;
    }

    // Deselect all players
    this.state.players.forEach(p => p.isSelected = false);
    
    // Select the specified player
    player.isSelected = true;
    this.state.selectedPlayerId = playerId;
    
    return true;
  }

  /**
   * Move a player to a new hex position
   */
  movePlayer(playerId: string, targetHex: HexCoordinate): boolean {
    const player = this.state.players.find(p => p.id === playerId);
    if (!player || player.team !== this.state.currentTeam) {
      return false;
    }

    // Check if target hex is occupied by another player
    const targetOccupied = this.state.players.some(p => 
      p.id !== playerId && 
      p.hexPosition.q === targetHex.q && 
      p.hexPosition.r === targetHex.r
    );

    if (targetOccupied) {
      return false;
    }

    // Calculate movement cost (distance between hexes)
    const movementCost = this.calculateDistance(player.hexPosition, targetHex);
    
    // Check if player has enough movement points
    if (movementCost > player.movementPoints) {
      return false;
    }

    // Check if moving with ball (ball follows player)
    if (player.hasBall && this.state.ball.state === BallState.WITH_PLAYER) {
      this.state.ball.hexPosition = targetHex;
    }

    // Update player position and movement points
    player.hexPosition = targetHex;
    player.movementPoints -= movementCost;

    // Update team movement points
    if (player.team === Team.A) {
      this.state.teamAMovementPoints -= movementCost;
    } else {
      this.state.teamBMovementPoints -= movementCost;
    }

    return true;
  }

  /**
   * Player attempts to kick the ball
   */
  kickBall(playerId: string, targetHex: HexCoordinate): boolean {
    const player = this.state.players.find(p => p.id === playerId);
    if (!player || player.team !== this.state.currentTeam || player.movementPoints < 1) {
      return false;
    }

    // Check if player can kick the ball (has ball or ball is adjacent)
    const canKick = player.hasBall || 
                   (this.state.ball.state === BallState.ON_GROUND &&
                    this.state.ball.hexPosition &&
                    this.calculateDistance(player.hexPosition, this.state.ball.hexPosition) <= 1);

    if (!canKick) {
      return false;
    }

    // Consume 1 MP for kicking
    player.movementPoints -= 1;
    if (player.team === Team.A) {
      this.state.teamAMovementPoints -= 1;
    } else {
      this.state.teamBMovementPoints -= 1;
    }

    // Update ball state
    if (player.hasBall) {
      player.hasBall = false;
    }

    this.state.ball = {
      state: BallState.ON_GROUND,
      hexPosition: targetHex,
      playerId: undefined
    };

    return true;
  }

  /**
   * Player attempts to intercept the ball
   */
  interceptBall(playerId: string): boolean {
    const player = this.state.players.find(p => p.id === playerId);
    if (!player || player.team !== this.state.currentTeam) {
      return false;
    }

    const interceptCost = player.position === PlayerPosition.GOALKEEPER ? 2 : 1;
    
    if (player.movementPoints < interceptCost) {
      return false;
    }

    // Check if ball is interceptable (on ground and adjacent or same hex)
    if (this.state.ball.state !== BallState.ON_GROUND || !this.state.ball.hexPosition) {
      return false;
    }

    const distance = this.calculateDistance(player.hexPosition, this.state.ball.hexPosition);
    if (distance > 1) {
      return false;
    }

    // Consume MP for interception
    player.movementPoints -= interceptCost;
    if (player.team === Team.A) {
      this.state.teamAMovementPoints -= interceptCost;
    } else {
      this.state.teamBMovementPoints -= interceptCost;
    }

    // Player gets the ball
    player.hasBall = true;
    this.state.ball = {
      state: BallState.WITH_PLAYER,
      hexPosition: player.hexPosition,
      playerId: player.id
    };

    return true;
  }

  /**
   * End current team's turn
   */
  endTurn(): void {
    // Preserve unused movement points (half carried over)
    this.state.players
      .filter(p => p.team === this.state.currentTeam)
      .forEach(p => {
        p.unusedMovementPoints = Math.floor(p.movementPoints / 2);
        p.movementPoints = this.DEFAULT_PLAYER_MP + p.unusedMovementPoints;
        p.isSelected = false;
      });

    // Switch teams
    this.state.currentTeam = this.state.currentTeam === Team.A ? Team.B : Team.A;
    
    // Reset team movement points
    if (this.state.currentTeam === Team.A) {
      this.state.teamAMovementPoints = this.MAX_MOVEMENT_POINTS;
      this.state.currentTurn++;
    } else {
      this.state.teamBMovementPoints = this.MAX_MOVEMENT_POINTS;
    }

    this.state.selectedPlayerId = undefined;
  }

  /**
   * Reset the game
   */
  resetGame(): void {
    this.state = this.initializeGame(this.state.mode);
  }

  /**
   * Calculate distance between two hex coordinates
   */
  private calculateDistance(hexA: HexCoordinate, hexB: HexCoordinate): number {
    return (Math.abs(hexA.q - hexB.q) + 
            Math.abs(hexA.q + hexA.r - hexB.q - hexB.r) + 
            Math.abs(hexA.r - hexB.r)) / 2;
  }

  /**
   * Get current team's remaining movement points
   */
  getCurrentTeamMovementPoints(): number {
    return this.state.currentTeam === Team.A ? 
           this.state.teamAMovementPoints : 
           this.state.teamBMovementPoints;
  }

  /**
   * Check if current team can end turn (used all movement points or chosen to end)
   */
  canEndTurn(): boolean {
    return this.getCurrentTeamMovementPoints() >= 0;
  }
}
