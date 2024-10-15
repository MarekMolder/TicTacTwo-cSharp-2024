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

            EGamePiece? winner = gameInstance.CheckWin();
            string? winningPlayerName = null; // Initialize variable to hold the winning player name

            if (winner == EGamePiece.X)
            {
                winningPlayerName = playerX; // Assign player X's name to the winning player
            }
            else if (winner == EGamePiece.O)
            {
                winningPlayerName = playerO; // Assign player O's name to the winning player
            }

            if (winner != null)
            {
                Console.Clear();
                Visualizer.DrawBoard(gameInstance);
                Console.WriteLine($"Player {winningPlayerName} wins!"); // Display the winning player's name
                break; // End game
            }
            
            if (winner != null)
            {
                Console.Clear();
                Visualizer.DrawBoard(gameInstance);
                Console.WriteLine($"Player {winner} wins!"); // Update to reflect the winning player
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