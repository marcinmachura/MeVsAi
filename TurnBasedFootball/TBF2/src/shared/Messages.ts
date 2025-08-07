export interface GameMessage {
  type: string;
  payload: any;
  timestamp: number;
  playerId?: string;
}

export interface MovePlayerMessage extends GameMessage {
  type: 'MOVE_PLAYER';
  payload: {
    playerId: string;
    targetHex: { q: number; r: number };
  };
}

export interface KickBallMessage extends GameMessage {
  type: 'KICK_BALL';
  payload: {
    playerId: string;
    targetHex: { q: number; r: number };
  };
}

export interface InterceptBallMessage extends GameMessage {
  type: 'INTERCEPT_BALL';
  payload: {
    playerId: string;
  };
}

export interface SelectPlayerMessage extends GameMessage {
  type: 'SELECT_PLAYER';
  payload: {
    playerId: string;
  };
}

export interface EndTurnMessage extends GameMessage {
  type: 'END_TURN';
  payload: {};
}

export interface GameStateMessage extends GameMessage {
  type: 'GAME_STATE';
  payload: {
    gameState: any; // GameState from GameLogic
  };
}

export interface ErrorMessage extends GameMessage {
  type: 'ERROR';
  payload: {
    message: string;
    code?: string;
  };
}

export interface JoinGameMessage extends GameMessage {
  type: 'JOIN_GAME';
  payload: {
    gameId?: string;
    playerName: string;
  };
}

export interface GameCreatedMessage extends GameMessage {
  type: 'GAME_CREATED';
  payload: {
    gameId: string;
    team: 'A' | 'B';
  };
}

export interface PlayerJoinedMessage extends GameMessage {
  type: 'PLAYER_JOINED';
  payload: {
    playerName: string;
    team: 'A' | 'B';
  };
}

export type ClientMessage = 
  | MovePlayerMessage 
  | KickBallMessage 
  | InterceptBallMessage 
  | SelectPlayerMessage 
  | EndTurnMessage 
  | JoinGameMessage;

export type ServerMessage = 
  | GameStateMessage 
  | ErrorMessage 
  | GameCreatedMessage 
  | PlayerJoinedMessage;
