using ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public class GameController
{
    public void NewGame(GameConfiguration chosenConfig, string playerX, string playerO)
    {
        var gameInstance = new TicTacTwoBrain(chosenConfig);
        var currentPlayerName = playerX;

        do
        {
            DisplayBoard(gameInstance, currentPlayerName, playerX, playerO);

            if (!gameInstance.MakeAMove())
            {
                Console.WriteLine("Invalid move. Try again.");
                continue;
            }

            if (CheckEndGameConditions(gameInstance, playerX, playerO))
            {
                break;
            }

            // Switch players
            currentPlayerName = currentPlayerName == playerX ? playerO : playerX;

        } while (true);
    }

    private void DisplayBoard(TicTacTwoBrain gameInstance, string currentPlayerName, string playerX, string playerO)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Current Player: {currentPlayerName} ({gameInstance.CurrentPlayer})");
        Console.WriteLine($"{playerX} has {gameInstance.PiecesLeftX} pieces left.");
        Console.WriteLine($"{playerO} has {gameInstance.PiecesLeftO} pieces left.");
    }

    private bool CheckEndGameConditions(TicTacTwoBrain gameInstance, string playerX, string playerO)
    {
        var winner = gameInstance.CheckWin();
        if (winner != null)
        {
            DisplayWinner(winner == EGamePiece.X ? playerX : playerO, gameInstance);
            return true;
        }

        if (!gameInstance.CheckDraw()) return false;
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine("It's a draw! Either no more pieces left or the board is full.");
        return true;

    }

    private void DisplayWinner(string winningPlayerName, TicTacTwoBrain gameInstance)
    {
        Console.Clear();
        Visualizer.DrawBoard(gameInstance);
        Console.WriteLine($"Player {winningPlayerName} wins!");
    }
}
