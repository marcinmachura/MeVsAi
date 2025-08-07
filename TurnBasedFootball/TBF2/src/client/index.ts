import { GameClient } from './GameClient';

/**
 * Main entry point for the client application
 */
document.addEventListener('DOMContentLoaded', () => {
  console.log('Starting Turn-Based Football Game...');
  
  try {
    const gameClient = new GameClient();
    gameClient.start();
  } catch (error) {
    console.error('Failed to start game:', error);
    
    // Show error message to user
    const errorDiv = document.createElement('div');
    errorDiv.style.cssText = `
      position: fixed;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      background: #ff0000;
      color: white;
      padding: 20px;
      border-radius: 10px;
      text-align: center;
      z-index: 10000;
    `;
    errorDiv.innerHTML = `
      <h2>Game Failed to Start</h2>
      <p>Please check the console for more details.</p>
      <p>Error: ${error}</p>
    `;
    document.body.appendChild(errorDiv);
  }
});
