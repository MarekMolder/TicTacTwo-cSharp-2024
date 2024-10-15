using System;
using DAL;
using GameBrain;

namespace MenuSystem;

/// <summary>
/// Manages the menu operations, including displaying the main menu.
/// </summary>
public class CustomInput
{
    /// <summary>
    /// Displays the main menu and handles user selection.
    /// </summary>
    public static (string playerX, string playerO) InputPlayerNames()
    {
        string playerX = GetValidPlayerName("Player X");
        string playerO = GetValidPlayerName("Player O", playerX);
        return (playerX, playerO);
    }

    private static string GetValidPlayerName(string playerLabel, string? otherPlayerName = null)
    {
        string playerName;
        while (true)
        {
            Console.Write($"Enter name for {playerLabel}: ");
            playerName = Console.ReadLine()!;

            if (!string.IsNullOrWhiteSpace(playerName) && 
                (otherPlayerName == null || playerName != otherPlayerName))
            {
                break;
            }

            string errorMessage = string.IsNullOrWhiteSpace(playerName) 
                ? $"{playerLabel} name must be at least 1 character long."
                : $"{playerLabel} name must be different from {otherPlayerName}.";
            Console.WriteLine(errorMessage + " Please try again.");
        }
        return playerName;
    }

    public static GameConfiguration InputCustomConfiguration()
    {
        int boardWidth = GetValidatedInput("Enter board width (min 2, max 20): ", 2, 20);
        int boardHeight = GetValidatedInput("Enter board height (min 2, max 20): ", 2, 20);
        int piecesNumber = GetValidatedInput("Enter pieces number (min 2, max 100): ", 2, 100);
        int winCondition = GetValidatedInput("Enter win condition (min 2, max 10): ", 2, 10);
        int movePieceAfterNMove = GetValidatedInput("After number of steps, you can move pieces: ", 0, null); // No upper limit

        bool usesGrid = GetUserConfirmation("Do you want a grid for your game? (yes/no): ");

        int gridWidth = 0, gridHeight = 0, gridPositionX = 0, gridPositionY = 0, moveGridAfterNMove = 100000;

        if (usesGrid)
        {
            gridWidth = GetGridDimensions("width", boardWidth);
            gridHeight = GetGridDimensions("height", boardHeight);
            moveGridAfterNMove = GetValidatedInput("After number of steps, you can move grid: ", 0, null); // No upper limit
            (gridPositionX, gridPositionY) = GetGridPosition(gridWidth, gridHeight, boardWidth, boardHeight);
        }

        return new GameConfiguration
        {
            Name = "Custom",
            BoardSizeWidth = boardWidth,
            BoardSizeHeight = boardHeight,
            PiecesNumber = piecesNumber,
            GridSizeWidth = usesGrid ? gridWidth : 0,
            GridSizeHeight = usesGrid ? gridHeight : 0,
            WinCondition = winCondition,
            UsesGrid = usesGrid,
            MovePieceAfterNMove = movePieceAfterNMove,
            MoveGridAfterNMove = usesGrid ? moveGridAfterNMove : 10000,
            GridPositionX = usesGrid ? gridPositionX : 0,
            GridPositionY = usesGrid ? gridPositionY : 0
        };
    }

    private static bool GetUserConfirmation(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "yes") return true;
            if (input == "no") return false;
            Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
        }
    }

    private static int GetGridDimensions(string dimension, int maxDimension)
    {
        return GetValidatedInput($"Enter grid {dimension} (min 2, max {maxDimension}): ", 2, maxDimension);
    }

    private static (int x, int y) GetGridPosition(int gridWidth, int gridHeight, int boardWidth, int boardHeight)
    {
        int gridPositionX, gridPositionY;
        while (true)
        {
            gridPositionX = GetValidatedInput("Grid X Coordinate: ", 0, null);
            gridPositionY = GetValidatedInput("Grid Y Coordinate: ", 0, null);

            if (IsValidGridPosition(gridPositionX, gridPositionY, gridWidth, gridHeight, boardWidth, boardHeight))
                break;

            Console.WriteLine("Invalid grid position. Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        return (gridPositionX, gridPositionY);
    }

    private static bool IsValidGridPosition(int x, int y, int gridWidth, int gridHeight, int boardWidth, int boardHeight)
    {
        return x >= 0 && x + gridWidth <= boardWidth && y >= 0 && y + gridHeight <= boardHeight;
    }

    public static int GetValidatedInput(string prompt, int minValue, int? maxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= minValue && (maxValue == null || value <= maxValue))
            {
                return value;
            }
            Console.WriteLine(maxValue == null
                ? $"Invalid input. Please enter a number greater than or equal to {minValue}."
                : $"Invalid input. Please enter a number between {minValue} and {maxValue}.");
        }
    }

    /// <summary>
    /// Loads a previously saved game. (Implement this method as needed.)
    /// </summary>
    public static void LoadGame()
    {
        // Implement load game logic here
    }
}
