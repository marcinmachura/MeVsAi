/**
 * Hexagonal coordinate system using axial coordinates (q, r)
 * Based on Red Blob Games hex grid guide
 */
export interface HexCoordinate {
  q: number; // column
  r: number; // row
}

export interface Point {
  x: number;
  y: number;
}

/**
 * HexGrid class handles the hexagonal grid system for the football field
 * Field dimensions: 60x45 hexes (width x height) - rotated for left-right play
 */
export class HexGrid {
  public readonly width: number = 60;
  public readonly height: number = 45;
  public readonly hexSize: number;
  private readonly pixelWidth: number;
  private readonly pixelHeight: number;

  constructor(canvasWidth: number, canvasHeight: number) {
    // Calculate hex size for rectangular layout with flat-top hexes
    const widthForHexes = canvasWidth * 0.9; // Leave 10% margin
    const heightForHexes = canvasHeight * 0.9; // Leave 10% margin
    
    // For rectangular hex layout with flat-top orientation
    const hexWidthSpacing = 1.5;
    const hexHeightSpacing = Math.sqrt(3);
    
    const hexSizeByWidth = widthForHexes / ((this.width - 1) * hexWidthSpacing + 1);
    const hexSizeByHeight = heightForHexes / (this.height * hexHeightSpacing);
    
    this.hexSize = Math.min(hexSizeByWidth, hexSizeByHeight);
    
    // Ensure minimum viable hex size
    if (this.hexSize < 3) {
      this.hexSize = 3;
    }
    
    this.pixelWidth = canvasWidth;
    this.pixelHeight = canvasHeight;
  }

  /**
   * Convert hex coordinates to pixel coordinates
   * Using flat-top orientation with true rectangular layout (offset rows)
   */
  hexToPixel(hex: HexCoordinate): Point {
    // For true rectangular layout with flat-top hexes
    // Even rows (r % 2 == 0) are normal, odd rows are offset by half hex width
    const offsetForOddRow = (hex.r % 2) * (this.hexSize * 3/4);
    
    const x = this.hexSize * (3/2 * hex.q) + offsetForOddRow;
    const y = this.hexSize * Math.sqrt(3) * hex.r;
    
    // Calculate the actual field dimensions for rectangular layout
    const fieldWidth = this.hexSize * 3/2 * (this.width - 1) + this.hexSize + this.hexSize * 3/4;
    const fieldHeight = this.hexSize * Math.sqrt(3) * this.height;
    
    // Center the grid in the canvas
    const offsetX = (this.pixelWidth - fieldWidth) / 2;
    const offsetY = (this.pixelHeight - fieldHeight) / 2;
    
    return {
      x: x + offsetX,
      y: y + offsetY
    };
  }

  /**
   * Convert pixel coordinates to hex coordinates
   */
  pixelToHex(point: Point): HexCoordinate {
    // Calculate the actual field dimensions for rectangular layout
    const fieldWidth = this.hexSize * 3/2 * (this.width - 1) + this.hexSize + this.hexSize * 3/4;
    const fieldHeight = this.hexSize * Math.sqrt(3) * this.height;
    
    // Remove offset
    const offsetX = (this.pixelWidth - fieldWidth) / 2;
    const offsetY = (this.pixelHeight - fieldHeight) / 2;
    
    const x = point.x - offsetX;
    const y = point.y - offsetY;
    
    // Convert to fractional hex coordinates for rectangular layout
    const r = y / (this.hexSize * Math.sqrt(3));
    const offsetForOddRow = (Math.floor(r) % 2) * (this.hexSize * 3/4);
    const q = (x - offsetForOddRow) / (this.hexSize * 3/2);
    
    return this.roundHex({ q, r });
  }

  /**
   * Round fractional hex coordinates to nearest hex
   */
  private roundHex(hex: HexCoordinate): HexCoordinate {
    const s = -hex.q - hex.r;
    
    let rq = Math.round(hex.q);
    let rr = Math.round(hex.r);
    let rs = Math.round(s);
    
    const qDiff = Math.abs(rq - hex.q);
    const rDiff = Math.abs(rr - hex.r);
    const sDiff = Math.abs(rs - s);
    
    if (qDiff > rDiff && qDiff > sDiff) {
      rq = -rr - rs;
    } else if (rDiff > sDiff) {
      rr = -rq - rs;
    }
    
    return { q: rq, r: rr };
  }

  /**
   * Check if hex coordinates are within the field bounds
   */
  isValidHex(hex: HexCoordinate): boolean {
    return hex.q >= 0 && hex.q < this.width && 
           hex.r >= 0 && hex.r < this.height;
  }

  /**
   * Get the six neighboring hexes of a given hex
   */
  getNeighbors(hex: HexCoordinate): HexCoordinate[] {
    const directions = [
      { q: 1, r: 0 }, { q: 1, r: -1 }, { q: 0, r: -1 },
      { q: -1, r: 0 }, { q: -1, r: 1 }, { q: 0, r: 1 }
    ];
    
    return directions
      .map(dir => ({ q: hex.q + dir.q, r: hex.r + dir.r }))
      .filter(neighbor => this.isValidHex(neighbor));
  }

  /**
   * Calculate distance between two hexes
   */
  distance(hexA: HexCoordinate, hexB: HexCoordinate): number {
    return (Math.abs(hexA.q - hexB.q) + 
            Math.abs(hexA.q + hexA.r - hexB.q - hexB.r) + 
            Math.abs(hexA.r - hexB.r)) / 2;
  }

  /**
   * Get all hexes within a certain distance (movement range)
   */
  getHexesInRange(center: HexCoordinate, range: number): HexCoordinate[] {
    const results: HexCoordinate[] = [];
    
    for (let q = -range; q <= range; q++) {
      const r1 = Math.max(-range, -q - range);
      const r2 = Math.min(range, -q + range);
      
      for (let r = r1; r <= r2; r++) {
        const hex = { q: center.q + q, r: center.r + r };
        if (this.isValidHex(hex)) {
          results.push(hex);
        }
      }
    }
    
    return results;
  }

  /**
   * Draw the hex grid on canvas
   */
  drawGrid(ctx: CanvasRenderingContext2D): void {
    // Draw subtle grid lines
    ctx.strokeStyle = 'rgba(255, 255, 255, 0.2)';
    ctx.lineWidth = 0.5;
    
    for (let q = 0; q < this.width; q++) {
      for (let r = 0; r < this.height; r++) {
        const hex = { q, r };
        this.drawHex(ctx, hex, 'transparent', 'rgba(255, 255, 255, 0.2)');
      }
    }
    
    // Draw field markings
    this.drawFieldMarkings(ctx);
  }

  /**
   * Draw a single hexagon
   */
  drawHex(ctx: CanvasRenderingContext2D, hex: HexCoordinate, fillColor: string, strokeColor: string): void {
    const center = this.hexToPixel(hex);
    const vertices = this.getHexVertices(center);
    
    ctx.beginPath();
    ctx.moveTo(vertices[0].x, vertices[0].y);
    for (let i = 1; i < vertices.length; i++) {
      ctx.lineTo(vertices[i].x, vertices[i].y);
    }
    ctx.closePath();
    
    ctx.fillStyle = fillColor;
    ctx.fill();
    ctx.strokeStyle = strokeColor;
    ctx.stroke();
  }

  /**
   * Get the six vertices of a hexagon (flat-top orientation)
   */
  private getHexVertices(center: Point): Point[] {
    const vertices: Point[] = [];
    for (let i = 0; i < 6; i++) {
      const angle = (Math.PI / 180) * (60 * i); // No offset for flat-top
      vertices.push({
        x: center.x + this.hexSize * Math.cos(angle),
        y: center.y + this.hexSize * Math.sin(angle)
      });
    }
    return vertices;
  }

  /**
   * Draw football field markings (goals, center circle, etc.)
   * Rotated 90 degrees for left-right play
   */
  private drawFieldMarkings(ctx: CanvasRenderingContext2D): void {
    ctx.strokeStyle = '#ffffff';
    ctx.lineWidth = 2;
    
    // Calculate field bounds for rectangular layout with flat-top hexes
    const fieldWidth = this.hexSize * 3/2 * (this.width - 1) + this.hexSize + this.hexSize * 3/4;
    const fieldHeight = this.hexSize * Math.sqrt(3) * this.height;
    const offsetX = (this.pixelWidth - fieldWidth) / 2;
    const offsetY = (this.pixelHeight - fieldHeight) / 2;
    
    // Draw field boundary rectangle
    ctx.beginPath();
    ctx.rect(offsetX - 10, offsetY - 10, fieldWidth + 20, fieldHeight + 20);
    ctx.stroke();
    
    // Center line (vertical for left-right play)
    const centerX = offsetX + fieldWidth / 2;
    ctx.beginPath();
    ctx.moveTo(centerX, offsetY - 10);
    ctx.lineTo(centerX, offsetY + fieldHeight + 10);
    ctx.stroke();
    
    // Center circle
    const centerY = offsetY + fieldHeight / 2;
    ctx.beginPath();
    ctx.arc(centerX, centerY, this.hexSize * 3, 0, 2 * Math.PI);
    ctx.stroke();
    
    // Center spot
    ctx.beginPath();
    ctx.arc(centerX, centerY, 3, 0, 2 * Math.PI);
    ctx.fillStyle = '#ffffff';
    ctx.fill();
    
    // Goals (now on left and right sides)
    const goalWidth = this.hexSize * 1.5;
    const goalHeight = fieldHeight * 0.2;
    
    // Left goal
    ctx.strokeRect(
      offsetX - goalWidth - 10, 
      centerY - goalHeight / 2, 
      goalWidth, 
      goalHeight
    );
    
    // Right goal
    ctx.strokeRect(
      offsetX + fieldWidth + 10, 
      centerY - goalHeight / 2, 
      goalWidth, 
      goalHeight
    );
  }
}
