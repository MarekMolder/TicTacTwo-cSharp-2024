using Domain;


namespace MenuSystem;
public static class CustomInput
{
    /// <summary>
    /// Prompts the user to enter names for both players (Player X and Player O).
    /// Validates that the names are not empty and that they are different from each other.
    /// If the names are invalid, the user will be prompted to re-enter them.
    /// </summary>
    public static (string playerX, string playerO) InputPlayerNames()
    {
        string playerX = GetValidPlayerName("Player X");
        string playerO = GetValidPlayerName("Player O", playerX);
        return (playerX, playerO);
    }

    /// <summary>
    /// Prompts the user to enter a valid player name.
    /// Validates that the name is not empty and, if provided, is different from the other player's name.
    /// If the name is invalid, the user will be prompted to re-enter it.
    /// </summary>
    private static string GetValidPlayerName(string playerLabel, string? otherPlayerName = null)
    {
        string playerName;
        while (true)
        {
            Console.Write($"Enter name for {playerLabel}: ");
            playerName = Console.ReadLine()!;
            
            if (!string.IsNullOrWhiteSpace(playerName) &&
                (otherPlayerName == null || 
                 string.Equals(playerName, otherPlayerName, StringComparison.OrdinalIgnoreCase) ||
                 !string.Equals(playerName, otherPlayerName, StringComparison.OrdinalIgnoreCase)))
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

    /// <summary>
    /// Prompts the user for custom game configuration settings.
    /// Validates all inputs (e.g., board dimensions, number of pieces, win condition, grid settings) 
    /// and returns a GameConfiguration object with the specified settings.
    /// </summary>
    public static GameConfiguration InputCustomConfiguration()
    {
        int boardWidth = GetValidatedInput("Enter board width (min 2, max 20): ", 2, 20);
        int boardHeight = GetValidatedInput("Enter board height (min 2, max 20): ", 2, 20);
        int piecesNumber = GetValidatedInput("Enter pieces number (min 2, max 100): ", 2, 100);
        int winCondition = GetValidatedInput($"Enter win condition (min 2, max {piecesNumber}): ", 2, piecesNumber);
        int movePieceAfterNMove = GetValidatedInput("After number of steps, you can move pieces: ", 0, null);
        
        bool usesGrid = GetUserConfirmation("Do you want a grid for your game? (yes/no): ");

        int gridWidth = 0, gridHeight = 0, gridPositionX = 0, gridPositionY = 0, moveGridAfterNMove = 100000;
        
        if (usesGrid)
        {
            gridWidth = GetValidatedInput($"Enter grid width (min 2, max {boardWidth}): ", winCondition, boardWidth);
            gridHeight = GetValidatedInput($"Enter grid height (min 2, max {boardHeight}): ", winCondition, boardHeight);
            
            moveGridAfterNMove = GetValidatedInput("After number of steps, you can move grid: ", 0, null);
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

    /// <summary>
    /// Prompts the user for confirmation with a yes or no question.
    /// If the user provides an invalid response, they will be asked again.
    /// </summary>
    private static bool GetUserConfirmation(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim().ToLower()!;
            if (input == "yes") return true;
            if (input == "no") return false;
            Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
        }
    }
    
    /// <summary>
    /// Prompts the user to enter the position for the grid and validates the input.
    /// Ensures the grid's position does not exceed the board's dimensions.
    /// </summary>
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
        return (gridPositionX, gridPositionY);
    }

    /// <summary>
    /// Checks if the specified grid position is valid within the board dimensions.
    /// Ensures that the grid fits within the board without overflowing its edges.
    /// </summary>
    private static bool IsValidGridPosition(int x, int y, int gridWidth, int gridHeight, int boardWidth, int boardHeight)
    {
        return x >= 0 && x + gridWidth <= boardWidth && y >= 0 && y + gridHeight <= boardHeight;
    }

    /// <summary>
    /// Prompts the user for a validated integer input within specified bounds.
    /// Ensures the entered value is within the provided minimum and maximum range.
    /// If the input is invalid, the user will be prompted again.
    /// </summary>
    private static int GetValidatedInput(string prompt, int minValue, int? maxValue)
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
}
