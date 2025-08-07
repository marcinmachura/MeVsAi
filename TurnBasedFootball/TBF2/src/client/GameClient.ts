import { GameRenderer } from './GameRenderer';
import { FootballGame, GameMode, GameState } from '../shared/GameLogic';
import { HexCoordinate } from '../shared/HexGrid';

/**
 * Main game client that handles UI interactions and game state
 */
export class GameClient {
  private game: FootballGame;
  private renderer: GameRenderer;
  private canvas!: HTMLCanvasElement;
  private selectedAction: 'move' | 'kick' | 'intercept' | null = null;

  // UI elements
  private turnIndicator!: HTMLElement;
  private mpCounter!: HTMLElement;
  private endTurnBtn!: HTMLButtonElement;
  private resetBtn!: HTMLButtonElement;
  private gameModeSelect!: HTMLSelectElement;

  constructor() {
    this.initializeDOM();
    this.game = new FootballGame(GameMode.LOCAL);
    this.renderer = new GameRenderer(this.canvas);
    this.setupEventListeners();
    this.updateUI();
    this.render();
  }

  /**
   * Initialize DOM elements
   */
  private initializeDOM(): void {
    this.canvas = document.getElementById('gameCanvas') as HTMLCanvasElement;
    this.turnIndicator = document.getElementById('turn-indicator')!;
    this.mpCounter = document.getElementById('mp-counter')!;
    this.endTurnBtn = document.getElementById('endTurnBtn') as HTMLButtonElement;
    this.resetBtn = document.getElementById('resetBtn') as HTMLButtonElement;
    this.gameModeSelect = document.getElementById('gameModeSelect') as HTMLSelectElement;

    if (!this.canvas || !this.turnIndicator || !this.mpCounter || 
        !this.endTurnBtn || !this.resetBtn || !this.gameModeSelect) {
      throw new Error('Required DOM elements not found');
    }
  }

  /**
   * Setup event listeners
   */
  private setupEventListeners(): void {
    // Canvas mouse events
    this.canvas.addEventListener('click', this.handleCanvasClick.bind(this));
    this.canvas.addEventListener('mousemove', this.handleCanvasMouseMove.bind(this));
    this.canvas.addEventListener('contextmenu', this.handleCanvasRightClick.bind(this));

    // UI button events
    this.endTurnBtn.addEventListener('click', this.handleEndTurn.bind(this));
    this.resetBtn.addEventListener('click', this.handleReset.bind(this));
    this.gameModeSelect.addEventListener('change', this.handleModeChange.bind(this));

    // Keyboard events for actions
    document.addEventListener('keydown', this.handleKeyDown.bind(this));
  }

  /**
   * Check if player is near the ball
   */
  private isPlayerNearBall(player: any, gameState: any): boolean {
    if (gameState.ball.state !== 'ON_GROUND' || !gameState.ball.hexPosition) {
      return false;
    }
    
    const ballHex = gameState.ball.hexPosition;
    const playerHex = player.hexPosition;
    
    // Check if player is adjacent to ball (distance of 1)
    const distance = Math.abs(playerHex.q - ballHex.q) + 
                    Math.abs(playerHex.q + playerHex.r - ballHex.q - ballHex.r) + 
                    Math.abs(playerHex.r - ballHex.r);
    
    return distance <= 2; // Within 1 hex distance
  }

  /**
   * Handle canvas click events
   */
  private handleCanvasClick(event: MouseEvent): void {
    const rect = this.canvas.getBoundingClientRect();
    const mouseX = event.clientX - rect.left;
    const mouseY = event.clientY - rect.top;
    
    const hex = this.renderer.getHexFromMouse(mouseX, mouseY);
    const gameState = this.game.getState();
    
    // Check if clicking on a player
    const clickedPlayer = gameState.players.find(p => 
      p.hexPosition.q === hex.q && p.hexPosition.r === hex.r
    );

    // Check if clicking on the ball
    const ballOnHex = gameState.ball.state === 'ON_GROUND' && 
                     gameState.ball.hexPosition &&
                     gameState.ball.hexPosition.q === hex.q && 
                     gameState.ball.hexPosition.r === hex.r;

    if (clickedPlayer && clickedPlayer.team === gameState.currentTeam) {
      // Select player
      this.game.selectPlayer(clickedPlayer.id);
      this.selectedAction = null;
      
      // Auto-suggest action based on context
      if (clickedPlayer.hasBall) {
        this.updateActionHint("Player has ball - Press K to kick or move normally");
      } else if (this.isPlayerNearBall(clickedPlayer, gameState)) {
        this.updateActionHint("Player near ball - Press I to intercept or move normally");
      }
    } else if (ballOnHex && !this.selectedAction) {
      // Clicking on ball without action selected - suggest intercept
      if (gameState.selectedPlayerId) {
        this.selectedAction = 'intercept';
        this.updateActionHint("Intercept mode activated - Click to confirm");
      } else {
        this.updateActionHint("Select a player first to interact with the ball");
      }
    } else if (gameState.selectedPlayerId) {
      // Perform action with selected player
      this.performAction(hex);
    }

    this.updateUI();
    this.render();
  }

  /**
   * Handle canvas mouse move for hover effects
   */
  private handleCanvasMouseMove(event: MouseEvent): void {
    const rect = this.canvas.getBoundingClientRect();
    const mouseX = event.clientX - rect.left;
    const mouseY = event.clientY - rect.top;
    
    const hex = this.renderer.getHexFromMouse(mouseX, mouseY);
    const hexGrid = this.renderer.getHexGrid();
    
    if (hexGrid.isValidHex(hex)) {
      this.renderer.setHoveredHex(hex);
      this.render();
    }
  }

  /**
   * Handle right-click for action menu
   */
  private handleCanvasRightClick(event: MouseEvent): void {
    event.preventDefault();
    // Future: Show context menu for actions
  }

  /**
   * Handle keyboard shortcuts
   */
  private handleKeyDown(event: KeyboardEvent): void {
    switch (event.key.toLowerCase()) {
      case 'm':
        this.selectedAction = 'move';
        break;
      case 'k':
        this.selectedAction = 'kick';
        break;
      case 'i':
        this.selectedAction = 'intercept';
        break;
      case 'escape':
        this.selectedAction = null;
        break;
    }
    this.updateUI();
  }

  /**
   * Perform action with selected player
   */
  private performAction(targetHex: HexCoordinate): void {
    const gameState = this.game.getState();
    const selectedPlayerId = gameState.selectedPlayerId;
    
    if (!selectedPlayerId) return;

    let success = false;

    switch (this.selectedAction) {
      case 'move':
        success = this.game.movePlayer(selectedPlayerId, targetHex);
        break;
      case 'kick':
        success = this.game.kickBall(selectedPlayerId, targetHex);
        break;
      case 'intercept':
        success = this.game.interceptBall(selectedPlayerId);
        break;
      default:
        // Default action: move if no specific action selected
        success = this.game.movePlayer(selectedPlayerId, targetHex);
        break;
    }

    if (success) {
      this.selectedAction = null;
    } else {
      console.log('Action failed'); // Future: Show user feedback
    }
  }

  /**
   * Handle end turn button
   */
  private handleEndTurn(): void {
    this.game.endTurn();
    this.selectedAction = null;
    this.updateUI();
    this.render();
  }

  /**
   * Handle reset button
   */
  private handleReset(): void {
    this.game.resetGame();
    this.selectedAction = null;
    this.updateUI();
    this.render();
  }

  /**
   * Handle game mode change
   */
  private handleModeChange(): void {
    const newMode = this.gameModeSelect.value as GameMode;
    this.game = new FootballGame(newMode);
    this.selectedAction = null;
    this.updateUI();
    this.render();
  }

  /**
   * Update UI elements
   */
  private updateUI(): void {
    const gameState = this.game.getState();
    
    // Update turn indicator
    const teamName = gameState.currentTeam === 'A' ? 'Team A' : 'Team B';
    this.turnIndicator.textContent = `${teamName}'s Turn`;
    
    // Update movement points
    const currentMP = this.game.getCurrentTeamMovementPoints();
    this.mpCounter.textContent = currentMP.toString();
    
    // Update end turn button
    this.endTurnBtn.disabled = !this.game.canEndTurn();
    
    // Update canvas cursor based on selected action
    if (this.selectedAction) {
      this.canvas.style.cursor = 'crosshair';
    } else if (gameState.selectedPlayerId) {
      this.canvas.style.cursor = 'pointer';
    } else {
      this.canvas.style.cursor = 'default';
    }

    // Show action hints
    const actionHint = this.getActionHint();
    this.updateActionHint(actionHint);
  }

  /**
   * Get current action hint text
   */
  private getActionHint(): string {
    const gameState = this.game.getState();
    
    if (this.selectedAction) {
      switch (this.selectedAction) {
        case 'kick':
          return 'KICK MODE: Click target hex to kick ball (1 MP)';
        case 'intercept':
          return 'INTERCEPT MODE: Click to intercept ball (1 MP, 2 MP for GK)';
        case 'move':
          return 'MOVE MODE: Click target hex to move player';
        default:
          return 'ACTION MODE: Click target location';
      }
    } else if (gameState.selectedPlayerId) {
      const selectedPlayer = gameState.players.find(p => p.id === gameState.selectedPlayerId);
      if (selectedPlayer?.hasBall) {
        return 'Player has ball - Click to move, K to kick, or use arrow keys';
      } else if (selectedPlayer && this.isPlayerNearBall(selectedPlayer, gameState)) {
        return 'Ball nearby - Click to move, I to intercept ball';
      } else {
        return 'Player selected - Click to move, or press K (kick), I (intercept)';
      }
    } else {
      return 'Click a player to select, or click ball to auto-select nearest player';
    }
  }

  /**
   * Update action hint in UI
   */
  private updateActionHint(hint: string): void {
    // Add action hint element if it doesn't exist
    let hintElement = document.getElementById('action-hint');
    if (!hintElement) {
      hintElement = document.createElement('div');
      hintElement.id = 'action-hint';
      hintElement.style.cssText = `
        position: fixed;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        background: rgba(0, 0, 0, 0.8);
        color: white;
        padding: 10px 20px;
        border-radius: 5px;
        font-family: Arial, sans-serif;
        z-index: 1000;
      `;
      document.body.appendChild(hintElement);
    }
    hintElement.textContent = hint;
  }

  /**
   * Render the game
   */
  private render(): void {
    const gameState = this.game.getState();
    this.renderer.render(gameState);
  }

  /**
   * Start the game loop
   */
  start(): void {
    this.updateUI();
    this.render();
    console.log('Football game started!');
  }
}
