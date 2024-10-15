using ConsoleUI;
using GameBrain;

namespace ConsoleApp;

/// <summary>
/// The GameController class manages the flow of the Tic-Tac-Two game.
/// </summary>
public class GameController
{
    /// <summary>
    /// Starts a new game with the given configuration and player names.
    /// </summary>
    /// <param name="chosenConfig">The configuration settings for the game.</param>
    /// <param name="playerX">The name of player X.</param>
    /// <param name="playerO">The name of player O.</param>
    public void NewGame(GameConfiguration chosenConfig, string playerX, string playerO)
    {
        // Create a new instance of the game brain with the chosen configuration
        var gameInstance = new TicTacTwoBrain(chosenConfig);
        var currentPlayerName = playerX;

        // Main game loop
        do
        {
            // Display the current state of the game board
            DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);

            // Attempt to make a move
            if (!gameInstance.MakeAMove())
            {
                Console.WriteLine("Invalid move. Try again.");
                continue; // Retry if the move was invalid
            }

            // Check if the game has ended
            if (CheckEndGameConditions(gameInstance, playerX, playerO))
            {
                break; // Exit the loop if the game has ended
            }

            // Switch players
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true); // Repeat until the game ends
    }

    /// <summary>
    /// Displays the current game board and player information.
    /// </summary>
    /// <param name="gameInstance">The current game instance.</param>
    /// <param name="currentPlayerName">The name of the current player.</param>
    /// <param name="playerX">The name of player X.</param>
    /// <param name="playerO">The name of player O.</param>
    private void DisplayBoard(TicTacTwoBrain gameInstance, string currentPlayerName, string playerX, string playerO)
    {
        Console.Clear(); // Clear the console for a fresh display
        Visualizer.DrawBoard(gameInstance); // Draw the current game board
        Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})");
        Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left."); // Show remaining pieces for player X
        Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left."); // Show remaining pieces for player O
    }

    /// <summary>
    /// Checks the end game conditions to see if a player has won or if the game is a draw.
    /// </summary>
    /// <param name="gameInstance">The current game instance.</param>
    /// <param name="playerX">The name of player X.</param>
    /// <param name="playerO">The name of player O.</param>
    /// <returns>True if the game has ended; otherwise, false.</returns>
    private bool CheckEndGameConditions(TicTacTwoBrain gameInstance, string playerX, string playerO)
    {
        var winner = gameInstance.CheckWin(); // Check for a winner
        if (winner != null)
        {
            DisplayWinner(winner == EGamePiece.X ? playerX : playerO, gameInstance); // Display the winner
            return true; // Game has ended
        }

        // Check for a draw
        if (!gameInstance.CheckDraw()) return false;
        
        Console.Clear(); // Clear the console
        Visualizer.DrawBoard(gameInstance); // Draw the final board
        Console.WriteLine("It's a draw! Either no more pieces left or the board is full."); // Inform players of the draw
        return true; // Game has ended
    }

    /// <summary>
    /// Displays the winner of the game.
    /// </summary>
    /// <param name="winningPlayerName">The name of the winning player.</param>
    /// <param name="gameInstance">The current game instance.</param>
    private void DisplayWinner(string winningPlayerName, TicTacTwoBrain gameInstance)
    {
        Console.Clear(); // Clear the console
        Visualizer.DrawBoard(gameInstance); // Draw the final board
        Console.WriteLine($"Player {winningPlayerName} wins!"); // Announce the winner
    }
}
