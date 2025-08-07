import { HexGrid, HexCoordinate, Point } from '../shared/HexGrid';
import { FootballGame, GameState, Player, Team, BallState } from '../shared/GameLogic';

/**
 * Game renderer handles drawing the game state on canvas
 */
export class GameRenderer {
  private canvas: HTMLCanvasElement;
  private ctx: CanvasRenderingContext2D;
  private hexGrid: HexGrid;
  private hoveredHex: HexCoordinate | null = null;

  constructor(canvas: HTMLCanvasElement) {
    this.canvas = canvas;
    this.ctx = canvas.getContext('2d')!;
    this.hexGrid = new HexGrid(canvas.width, canvas.height);
  }

  /**
   * Render the complete game state
   */
  render(gameState: GameState): void {
    // Clear canvas
    this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
    
    // Draw field background
    this.ctx.fillStyle = '#4a8a4a';
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);
    
    // Draw hex grid
    this.hexGrid.drawGrid(this.ctx);
    
    // Draw valid moves for selected player
    if (gameState.selectedPlayerId) {
      this.drawValidMoves(gameState);
    }
    
    // Draw hover highlight
    if (this.hoveredHex) {
      this.drawHexHighlight(this.hoveredHex, 'rgba(255, 255, 0, 0.3)');
    }
    
    // Draw players
    this.drawPlayers(gameState.players);
    
    // Draw ball
    this.drawBall(gameState.ball);
  }

  /**
   * Draw all players on the field
   */
  private drawPlayers(players: Player[]): void {
    players.forEach(player => {
      const pixelPos = this.hexGrid.hexToPixel(player.hexPosition);
      
      // Player circle
      this.ctx.beginPath();
      this.ctx.arc(pixelPos.x, pixelPos.y, 15, 0, 2 * Math.PI);
      
      // Team colors
      if (player.team === Team.A) {
        this.ctx.fillStyle = player.isSelected ? '#ff0000' : '#0066cc';
      } else {
        this.ctx.fillStyle = player.isSelected ? '#ff6666' : '#cc0000';
      }
      
      this.ctx.fill();
      this.ctx.strokeStyle = '#ffffff';
      this.ctx.lineWidth = 2;
      this.ctx.stroke();
      
      // Player ID
      this.ctx.fillStyle = '#ffffff';
      this.ctx.font = '12px Arial';
      this.ctx.textAlign = 'center';
      this.ctx.fillText(player.id, pixelPos.x, pixelPos.y + 4);
      
      // Movement points indicator
      if (player.movementPoints > 0) {
        this.ctx.fillStyle = '#ffff00';
        this.ctx.font = '10px Arial';
        this.ctx.fillText(player.movementPoints.toString(), pixelPos.x + 20, pixelPos.y - 10);
      }
      
      // Ball indicator
      if (player.hasBall) {
        this.ctx.beginPath();
        this.ctx.arc(pixelPos.x + 18, pixelPos.y - 18, 6, 0, 2 * Math.PI);
        this.ctx.fillStyle = '#000000';
        this.ctx.fill();
        this.ctx.strokeStyle = '#ffffff';
        this.ctx.lineWidth = 1;
        this.ctx.stroke();
      }
    });
  }

  /**
   * Draw the ball
   */
  private drawBall(ball: any): void {
    if (ball.state === BallState.WITH_PLAYER) {
      return; // Ball is drawn with player
    }

    let pixelPos: Point;
    
    if (ball.state === BallState.ON_GROUND && ball.hexPosition) {
      pixelPos = this.hexGrid.hexToPixel(ball.hexPosition);
    } else if (ball.state === BallState.FLYING && ball.pixelPosition) {
      pixelPos = ball.pixelPosition;
    } else {
      return;
    }

    // Draw ball with glow effect when it's interactive
    if (ball.state === BallState.ON_GROUND) {
      // Glow effect for ground ball
      this.ctx.beginPath();
      this.ctx.arc(pixelPos.x, pixelPos.y, 12, 0, 2 * Math.PI);
      this.ctx.fillStyle = 'rgba(255, 255, 0, 0.3)';
      this.ctx.fill();
    }

    // Draw ball
    this.ctx.beginPath();
    this.ctx.arc(pixelPos.x, pixelPos.y, 8, 0, 2 * Math.PI);
    this.ctx.fillStyle = '#000000';
    this.ctx.fill();
    this.ctx.strokeStyle = '#ffffff';
    this.ctx.lineWidth = 2;
    this.ctx.stroke();
    
    // Ball pattern
    this.ctx.strokeStyle = '#ffffff';
    this.ctx.lineWidth = 1;
    this.ctx.beginPath();
    this.ctx.moveTo(pixelPos.x - 6, pixelPos.y);
    this.ctx.lineTo(pixelPos.x + 6, pixelPos.y);
    this.ctx.moveTo(pixelPos.x, pixelPos.y - 6);
    this.ctx.lineTo(pixelPos.x, pixelPos.y + 6);
    this.ctx.stroke();
    
    // Add "BALL" text below for clarity
    if (ball.state === BallState.ON_GROUND) {
      this.ctx.fillStyle = '#ffffff';
      this.ctx.font = '8px Arial';
      this.ctx.textAlign = 'center';
      this.ctx.fillText('BALL', pixelPos.x, pixelPos.y + 20);
    }
  }

  /**
   * Draw valid moves for selected player
   */
  private drawValidMoves(gameState: GameState): void {
    const selectedPlayer = gameState.players.find(p => p.id === gameState.selectedPlayerId);
    if (!selectedPlayer) return;

    const validHexes = this.hexGrid.getHexesInRange(
      selectedPlayer.hexPosition, 
      selectedPlayer.movementPoints
    );

    validHexes.forEach(hex => {
      // Check if hex is occupied
      const occupied = gameState.players.some(p => 
        p.id !== selectedPlayer.id &&
        p.hexPosition.q === hex.q && 
        p.hexPosition.r === hex.r
      );

      if (!occupied) {
        const distance = this.hexGrid.distance(selectedPlayer.hexPosition, hex);
        const alpha = Math.max(0.1, 1 - (distance / selectedPlayer.movementPoints));
        this.drawHexHighlight(hex, `rgba(0, 255, 0, ${alpha})`);
        
        // Show movement cost
        if (distance > 0) {
          const pixelPos = this.hexGrid.hexToPixel(hex);
          this.ctx.fillStyle = '#ffffff';
          this.ctx.font = '10px Arial';
          this.ctx.textAlign = 'center';
          this.ctx.fillText(distance.toString(), pixelPos.x, pixelPos.y);
        }
      }
    });
  }

  /**
   * Draw hex highlight
   */
  private drawHexHighlight(hex: HexCoordinate, color: string): void {
    this.hexGrid.drawHex(this.ctx, hex, color, 'transparent');
  }

  /**
   * Convert mouse coordinates to hex coordinates
   */
  getHexFromMouse(mouseX: number, mouseY: number): HexCoordinate {
    return this.hexGrid.pixelToHex({ x: mouseX, y: mouseY });
  }

  /**
   * Set hovered hex for visual feedback
   */
  setHoveredHex(hex: HexCoordinate | null): void {
    this.hoveredHex = hex;
  }

  /**
   * Get hex grid instance
   */
  getHexGrid(): HexGrid {
    return this.hexGrid;
  }
}
