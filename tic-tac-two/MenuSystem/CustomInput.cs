using System;
using DAL;
using GameBrain;

namespace MenuSystem;

/// <summary>
/// Manages the menu operations, including displaying the main menu and handling user input.
/// </summary>
public class CustomInput
{
    /// <summary>
    /// Prompts the user to enter names for both players (Player X and Player O).
    /// Validates that the names are not empty and that they are different from each other.
    /// </summary>
    /// <returns>A tuple containing the names of Player X and Player O.</returns>
    public static (string playerX, string playerO) InputPlayerNames()
    {
        string playerX = GetValidPlayerName("Player X");
        string playerO = GetValidPlayerName("Player O", playerX);
        return (playerX, playerO);
    }

    /// <summary>
    /// Prompts the user to enter a valid player name.
    /// Validates that the name is not empty and, if provided, is different from the other player's name.
    /// </summary>
    /// <param name="playerLabel">The label for the player (e.g., "Player X").</param>
    /// <param name="otherPlayerName">The name of the other player, if applicable.</param>
    /// <returns>The validated player name.</returns>
    private static string GetValidPlayerName(string playerLabel, string? otherPlayerName = null)
    {
        string playerName;
        while (true)
        {
            Console.Write($"Enter name for {playerLabel}: ");
            playerName = Console.ReadLine()!;

            // Check if the player name is valid
            if (!string.IsNullOrWhiteSpace(playerName) && 
                (otherPlayerName == null || playerName != otherPlayerName))
            {
                break; // Exit loop if valid
            }

            // Error message for invalid input
            string errorMessage = string.IsNullOrWhiteSpace(playerName) 
                ? $"{playerLabel} name must be at least 1 character long."
                : $"{playerLabel} name must be different from {otherPlayerName}.";
            Console.WriteLine(errorMessage + " Please try again.");
        }
        return playerName; // Return the valid player name
    }

    /// <summary>
    /// Prompts the user for custom game configuration settings.
    /// Validates all inputs and returns a GameConfiguration object with the specified settings.
    /// </summary>
    /// <returns>A GameConfiguration object containing the custom game settings.</returns>
    public static GameConfiguration InputCustomConfiguration()
    {
        // Prompt for board dimensions
        int boardWidth = GetValidatedInput("Enter board width (min 2, max 20): ", 2, 20);
        int boardHeight = GetValidatedInput("Enter board height (min 2, max 20): ", 2, 20);
        int piecesNumber = GetValidatedInput("Enter pieces number (min 2, max 100): ", 2, 100);
        int winCondition = GetValidatedInput("Enter win condition (min 2, max 10): ", 2, 10);
        int movePieceAfterNMove = GetValidatedInput("After number of steps, you can move pieces: ", 0, null); // No upper limit

        // Ask user if they want to use a grid
        bool usesGrid = GetUserConfirmation("Do you want a grid for your game? (yes/no): ");

        int gridWidth = 0, gridHeight = 0, gridPositionX = 0, gridPositionY = 0, moveGridAfterNMove = 100000;

        // If grid is used, prompt for grid dimensions and position
        if (usesGrid)
        {
            gridWidth = GetGridDimensions("width", boardWidth);
            gridHeight = GetGridDimensions("height", boardHeight);
            moveGridAfterNMove = GetValidatedInput("After number of steps, you can move grid: ", 0, null); // No upper limit
            (gridPositionX, gridPositionY) = GetGridPosition(gridWidth, gridHeight, boardWidth, boardHeight);
        }

        // Create and return a new GameConfiguration object
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

    /// <summary>
    /// Prompts the user for confirmation with a yes or no question.
    /// </summary>
    /// <param name="prompt">The question to ask the user.</param>
    /// <returns>True if the user answers "yes", false if "no".</returns>
    private static bool GetUserConfirmation(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "yes") return true; // Return true for yes
            if (input == "no") return false; // Return false for no
            Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
        }
    }

    /// <summary>
    /// Prompts the user to enter the dimensions for the grid and validates the input.
    /// </summary>
    /// <param name="dimension">The dimension type (e.g., "width" or "height").</param>
    /// <param name="maxDimension">The maximum allowed value for the dimension.</param>
    /// <returns>The validated dimension value.</returns>
    private static int GetGridDimensions(string dimension, int maxDimension)
    {
        return GetValidatedInput($"Enter grid {dimension} (min 2, max {maxDimension}): ", 2, maxDimension);
    }

    /// <summary>
    /// Prompts the user to enter the position for the grid and validates the input.
    /// </summary>
    /// <param name="gridWidth">The width of the grid.</param>
    /// <param name="gridHeight">The height of the grid.</param>
    /// <param name="boardWidth">The width of the board.</param>
    /// <param name="boardHeight">The height of the board.</param>
    /// <returns>A tuple containing the X and Y coordinates of the grid position.</returns>
    private static (int x, int y) GetGridPosition(int gridWidth, int gridHeight, int boardWidth, int boardHeight)
    {
        int gridPositionX, gridPositionY;
        while (true)
        {
            gridPositionX = GetValidatedInput("Grid X Coordinate: ", 0, null);
            gridPositionY = GetValidatedInput("Grid Y Coordinate: ", 0, null);

            // Validate the grid position
            if (IsValidGridPosition(gridPositionX, gridPositionY, gridWidth, gridHeight, boardWidth, boardHeight))
                break;

            Console.WriteLine("Invalid grid position. Please try again.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        return (gridPositionX, gridPositionY); // Return the valid grid position
    }

    /// <summary>
    /// Checks if the specified grid position is valid within the board dimensions.
    /// </summary>
    /// <param name="x">The X coordinate of the grid position.</param>
    /// <param name="y">The Y coordinate of the grid position.</param>
    /// <param name="gridWidth">The width of the grid.</param>
    /// <param name="gridHeight">The height of the grid.</param>
    /// <param name="boardWidth">The width of the board.</param>
    /// <param name="boardHeight">The height of the board.</param>
    /// <returns>True if the position is valid, otherwise false.</returns>
    private static bool IsValidGridPosition(int x, int y, int gridWidth, int gridHeight, int boardWidth, int boardHeight)
    {
        return x >= 0 && x + gridWidth <= boardWidth && y >= 0 && y + gridHeight <= boardHeight;
    }

    /// <summary>
    /// Prompts the user for a validated integer input within specified bounds.
    /// </summary>
    /// <param name="prompt">The prompt to display to the user.</param>
    /// <param name="minValue">The minimum acceptable value.</param>
    /// <param name="maxValue">The maximum acceptable value, or null for no upper limit.</param>
    /// <returns>The validated integer input from the user.</returns>
    public static int GetValidatedInput(string prompt, int minValue, int? maxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= minValue && (maxValue == null || value <= maxValue))
            {
                return value; // Return the valid input
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
