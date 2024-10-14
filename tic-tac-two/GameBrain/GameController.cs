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

            // Get valid coordinates from the user
            var (inputX, inputY) = gameInstance.GetValidCoordinates();

            // Attempt to make a move
            if (!gameInstance.MakeAMove(inputX, inputY))
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

            // Switch current player for the next turn
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
    }
}