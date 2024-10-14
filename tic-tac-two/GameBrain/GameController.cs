using System.Data;

namespace GameBrain;

public class GameController
{
    public void NewGame(GameConfiguration chosenConfig, string playerX, string playerO)
    {
    var gameInstance = new TicTacTwoBrain(chosenConfig); // Create new game instance

        // Set player names (for display purposes)
        var currentPlayerName = playerX; // Start with player x

        do
        {
            // Display the board
            Console.Clear();
            Visualizer.DrawBoard(gameInstance);
            Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})"); // Show the current player
            
            Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left.");
            Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left.");
            

            // Attempt to make a move
            if (!gameInstance.MakeAMove())
            {
                Console.WriteLine("Invalid move. Try again."); // If the move is invalid
                continue; // Ask for input again
            }

            // Check for a win condition
            if (gameInstance.CheckWin())
            {
                Console.Clear();
                Visualizer.DrawBoard(gameInstance);
                Console.WriteLine($"Player {currentPlayerName} wins!"); // Update to reflect current player
                break; // End game
            }
            
            if (gameInstance.CheckDraw())
            {
                Console.Clear();
                Visualizer.DrawBoard(gameInstance);
                Console.WriteLine("It's a draw!  Either no more pieces left or the board is full."); // Display draw message
                break; // End game
            }

            // Switch current player for the next turn
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
    }
}