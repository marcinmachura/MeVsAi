import express from 'express';
import { WebSocket, WebSocketServer } from 'ws';
import { createServer } from 'http';
import path from 'path';
import { FootballGame, GameMode, Team } from '../shared/GameLogic';
import { ClientMessage, ServerMessage } from '../shared/Messages';

/**
 * Game server that handles multiplayer games
 */
export class GameServer {
  private app: express.Application;
  private server: any;
  private wss: WebSocketServer;
  private games: Map<string, FootballGame> = new Map();
  private gamePlayers: Map<string, { playerA?: WebSocket; playerB?: WebSocket }> = new Map();
  private playerGames: Map<WebSocket, string> = new Map();

  constructor(port: number = 3001) {
    this.app = express();
    this.server = createServer(this.app);
    this.wss = new WebSocketServer({ server: this.server });
    
    this.setupExpress();
    this.setupWebSocket();
    
    this.server.listen(port, () => {
      console.log(`Game server running on port ${port}`);
    });
  }

  /**
   * Setup Express server for serving static files
   */
  private setupExpress(): void {
    // Serve static files from dist folder
    this.app.use(express.static(path.join(__dirname, '../../dist')));
    
    // API endpoints
    this.app.get('/api/health', (req, res) => {
      res.json({ status: 'ok', games: this.games.size });
    });

    this.app.get('/api/games', (req, res) => {
      const gameList = Array.from(this.games.entries()).map(([id, game]) => ({
        id,
        players: Object.keys(this.gamePlayers.get(id) || {}).length,
        currentTeam: game.getState().currentTeam,
        turn: game.getState().currentTurn
      }));
      res.json(gameList);
    });
  }

  /**
   * Setup WebSocket server for real-time communication
   */
  private setupWebSocket(): void {
    this.wss.on('connection', (ws: WebSocket) => {
      console.log('New client connected');

      ws.on('message', (data: Buffer) => {
        try {
          const message: ClientMessage = JSON.parse(data.toString());
          this.handleClientMessage(ws, message);
        } catch (error) {
          console.error('Invalid message received:', error);
          this.sendError(ws, 'Invalid message format');
        }
      });

      ws.on('close', () => {
        console.log('Client disconnected');
        this.handlePlayerDisconnect(ws);
      });

      ws.on('error', (error) => {
        console.error('WebSocket error:', error);
      });
    });
  }

  /**
   * Handle client messages
   */
  private handleClientMessage(ws: WebSocket, message: ClientMessage): void {
    switch (message.type) {
      case 'JOIN_GAME':
        this.handleJoinGame(ws, message);
        break;
      case 'MOVE_PLAYER':
        this.handleMovePlayer(ws, message);
        break;
      case 'KICK_BALL':
        this.handleKickBall(ws, message);
        break;
      case 'INTERCEPT_BALL':
        this.handleInterceptBall(ws, message);
        break;
      case 'SELECT_PLAYER':
        this.handleSelectPlayer(ws, message);
        break;
      case 'END_TURN':
        this.handleEndTurn(ws, message);
        break;
      default:
        this.sendError(ws, 'Unknown message type');
    }
  }

  /**
   * Handle player joining a game
   */
  private handleJoinGame(ws: WebSocket, message: any): void {
    let gameId = message.payload.gameId;
    let team: Team;

    if (!gameId) {
      // Create new game
      gameId = this.generateGameId();
      const game = new FootballGame(GameMode.ONLINE);
      this.games.set(gameId, game);
      this.gamePlayers.set(gameId, { playerA: ws });
      team = Team.A;
    } else {
      // Join existing game
      const gamePlayers = this.gamePlayers.get(gameId);
      if (!gamePlayers) {
        this.sendError(ws, 'Game not found');
        return;
      }

      if (!gamePlayers.playerB) {
        gamePlayers.playerB = ws;
        team = Team.B;
      } else {
        this.sendError(ws, 'Game is full');
        return;
      }
    }

    this.playerGames.set(ws, gameId);

    // Send game created/joined confirmation
    this.sendMessage(ws, {
      type: 'GAME_CREATED',
      payload: { gameId, team },
      timestamp: Date.now()
    });

    // Send current game state
    const game = this.games.get(gameId)!;
    this.broadcastGameState(gameId, game.getState());
  }

  /**
   * Handle move player action
   */
  private handleMovePlayer(ws: WebSocket, message: any): void {
    const gameId = this.playerGames.get(ws);
    if (!gameId) {
      this.sendError(ws, 'Not in a game');
      return;
    }

    const game = this.games.get(gameId)!;
    const success = game.movePlayer(message.payload.playerId, message.payload.targetHex);

    if (success) {
      this.broadcastGameState(gameId, game.getState());
    } else {
      this.sendError(ws, 'Invalid move');
    }
  }

  /**
   * Handle kick ball action
   */
  private handleKickBall(ws: WebSocket, message: any): void {
    const gameId = this.playerGames.get(ws);
    if (!gameId) {
      this.sendError(ws, 'Not in a game');
      return;
    }

    const game = this.games.get(gameId)!;
    const success = game.kickBall(message.payload.playerId, message.payload.targetHex);

    if (success) {
      this.broadcastGameState(gameId, game.getState());
    } else {
      this.sendError(ws, 'Invalid kick');
    }
  }

  /**
   * Handle intercept ball action
   */
  private handleInterceptBall(ws: WebSocket, message: any): void {
    const gameId = this.playerGames.get(ws);
    if (!gameId) {
      this.sendError(ws, 'Not in a game');
      return;
    }

    const game = this.games.get(gameId)!;
    const success = game.interceptBall(message.payload.playerId);

    if (success) {
      this.broadcastGameState(gameId, game.getState());
    } else {
      this.sendError(ws, 'Invalid intercept');
    }
  }

  /**
   * Handle select player action
   */
  private handleSelectPlayer(ws: WebSocket, message: any): void {
    const gameId = this.playerGames.get(ws);
    if (!gameId) {
      this.sendError(ws, 'Not in a game');
      return;
    }

    const game = this.games.get(gameId)!;
    const success = game.selectPlayer(message.payload.playerId);

    if (success) {
      this.broadcastGameState(gameId, game.getState());
    } else {
      this.sendError(ws, 'Invalid selection');
    }
  }

  /**
   * Handle end turn action
   */
  private handleEndTurn(ws: WebSocket, message: any): void {
    const gameId = this.playerGames.get(ws);
    if (!gameId) {
      this.sendError(ws, 'Not in a game');
      return;
    }

    const game = this.games.get(gameId)!;
    game.endTurn();
    this.broadcastGameState(gameId, game.getState());
  }

  /**
   * Handle player disconnect
   */
  private handlePlayerDisconnect(ws: WebSocket): void {
    const gameId = this.playerGames.get(ws);
    if (gameId) {
      const gamePlayers = this.gamePlayers.get(gameId);
      if (gamePlayers) {
        if (gamePlayers.playerA === ws) {
          gamePlayers.playerA = undefined;
        } else if (gamePlayers.playerB === ws) {
          gamePlayers.playerB = undefined;
        }

        // If no players left, clean up game
        if (!gamePlayers.playerA && !gamePlayers.playerB) {
          this.games.delete(gameId);
          this.gamePlayers.delete(gameId);
        }
      }

      this.playerGames.delete(ws);
    }
  }

  /**
   * Broadcast game state to all players in a game
   */
  private broadcastGameState(gameId: string, gameState: any): void {
    const gamePlayers = this.gamePlayers.get(gameId);
    if (!gamePlayers) return;

    const message: ServerMessage = {
      type: 'GAME_STATE',
      payload: { gameState },
      timestamp: Date.now()
    };

    if (gamePlayers.playerA) {
      this.sendMessage(gamePlayers.playerA, message);
    }
    if (gamePlayers.playerB) {
      this.sendMessage(gamePlayers.playerB, message);
    }
  }

  /**
   * Send message to a client
   */
  private sendMessage(ws: WebSocket, message: ServerMessage): void {
    if (ws.readyState === WebSocket.OPEN) {
      ws.send(JSON.stringify(message));
    }
  }

  /**
   * Send error message to a client
   */
  private sendError(ws: WebSocket, message: string): void {
    this.sendMessage(ws, {
      type: 'ERROR',
      payload: { message },
      timestamp: Date.now()
    });
  }

  /**
   * Generate unique game ID
   */
  private generateGameId(): string {
    return Math.random().toString(36).substring(2, 15) + 
           Math.random().toString(36).substring(2, 15);
  }
}

// Start server if this file is run directly
if (require.main === module) {
  new GameServer(3001);
}
