using GameBrain;

namespace ConsoleUI;

/// <summary>
/// Responsible for visualizing the Tic-Tac-Two game board in the console.
/// </summary>
public class Visualizer
{
    /// <summary>
    /// Draws the game board based on the current game instance.
    /// </summary>
    /// <param name="gameInstance">The instance of the game containing the current state and configuration.</param>
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        int gridStartX = gameInstance.GridPositionX; // Starting X coordinate of the grid
        int gridStartY = gameInstance.GridPositionY; // Starting Y coordinate of the grid
        int gridWidth = gameInstance.GridSizeWidth; // Width of the grid
        int gridHeight = gameInstance.GridSizeHeight; // Height of the grid
        int gridEndX = gridStartX + gridWidth; // Ending X coordinate of the grid
        int gridEndY = gridStartY + gridHeight; // Ending Y coordinate of the grid

        // Draw column numbers
        Console.Write("   ");
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            Console.Write($" {x} "); // Print column index
            if (x != gameInstance.DimensionX - 1) Console.Write("|"); // Draw separator if not the last column
        }
        Console.WriteLine();

        // Draw the game board
        for (var y = 0; y < gameInstance.DimensionY; y++)
        {
            Console.Write($"{y} |"); // Print row index
            for (var x = 0; x < gameInstance.DimensionX; x++)
            {
                // Set the background color based on the grid position
                SetBackgroundColorForGrid(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY);
                // Set the foreground color based on the game piece
                SetForegroundColorForPiece(gameInstance.GameBoard[x, y]);

                Console.Write($" {DrawGamePiece(gameInstance.GameBoard[x, y])} "); // Draw the game piece
                Console.ResetColor(); // Reset colors to default

                if (x != gameInstance.DimensionX - 1) 
                    DrawVerticalSeparator(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY); // Draw vertical separator
            }
            Console.WriteLine(); // Move to the next line

            if (y != gameInstance.DimensionY - 1) 
                DrawHorizontalSeparator(gameInstance, y, gridStartX, gridEndX, gridStartY, gridEndY); // Draw horizontal separator
        }

        Console.ResetColor(); // Restore original console colors
    }

    /// <summary>
    /// Sets the background color for the grid cells.
    /// </summary>
    /// <param name="gameInstance">The current game instance.</param>
    /// <param name="x">The X coordinate of the cell.</param>
    /// <param name="y">The Y coordinate of the cell.</param>
    /// <param name="gridStartX">The starting X coordinate of the grid.</param>
    /// <param name="gridEndX">The ending X coordinate of the grid.</param>
    /// <param name="gridStartY">The starting Y coordinate of the grid.</param>
    /// <param name="gridEndY">The ending Y coordinate of the grid.</param>
    private static void SetBackgroundColorForGrid(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        // Set background color for cells within the grid
        if (gameInstance.UsesGrid && x >= gridStartX && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray; // Color for grid cells
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black; // Default color for non-grid cells
        }
    }

    /// <summary>
    /// Sets the foreground color for game pieces based on their type.
    /// </summary>
    /// <param name="piece">The game piece to set the color for.</param>
    private static void SetForegroundColorForPiece(EGamePiece piece)
    {
        // Set foreground color for X and O pieces
        switch (piece)
        {
            case EGamePiece.X:
                Console.ForegroundColor = ConsoleColor.Red; // Color for X
                break;
            case EGamePiece.O:
                Console.ForegroundColor = ConsoleColor.Yellow; // Color for O
                break;
            default:
                Console.ResetColor(); // Default color for empty spaces
                break;
        }
    }

    /// <summary>
    /// Draws vertical separators between cells in the grid.
    /// </summary>
    /// <param name="gameInstance">The current game instance.</param>
    /// <param name="x">The X coordinate of the cell.</param>
    /// <param name="y">The Y coordinate of the cell.</param>
    /// <param name="gridStartX">The starting X coordinate of the grid.</param>
    /// <param name="gridEndX">The ending X coordinate of the grid.</param>
    /// <param name="gridStartY">The starting Y coordinate of the grid.</param>
    /// <param name="gridEndY">The ending Y coordinate of the grid.</param>
    private static void DrawVerticalSeparator(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        // Draw vertical separators within the grid
        if (gameInstance.UsesGrid && x >= gridStartX - 1 && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen; // Color for vertical separators
        }
        Console.Write("|"); // Print vertical separator
        Console.ResetColor(); // Reset color
    }

    /// <summary>
    /// Draws horizontal separators between rows in the grid.
    /// </summary>
    /// <param name="gameInstance">The current game instance.</param>
    /// <param name="y">The Y coordinate of the row.</param>
    /// <param name="gridStartX">The starting X coordinate of the grid.</param>
    /// <param name="gridEndX">The ending X coordinate of the grid.</param>
    /// <param name="gridStartY">The starting Y coordinate of the grid.</param>
    /// <param name="gridEndY">The ending Y coordinate of the grid.</param>
    private static void DrawHorizontalSeparator(TicTacTwoBrain gameInstance, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        Console.Write("  +"); // Start the horizontal separator
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            // Draw horizontal separators within the grid
            if (gameInstance.UsesGrid && x >= gridStartX && x < gridEndX && y >= gridStartY - 1 && y < gridEndY)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen; // Color for horizontal separators
            }
            Console.Write("---"); // Print horizontal separator

            if (x != gameInstance.DimensionX - 1) Console.Write("+"); // Draw intersection if not the last column
            Console.ResetColor(); // Reset color
        }
        Console.WriteLine(); // Move to the next line
    }

    /// <summary>
    /// Returns the string representation of the game piece.
    /// </summary>
    /// <param name="piece">The game piece to convert to a string.</param>
    /// <returns>A string representing the game piece.</returns>
    private static string DrawGamePiece(EGamePiece piece) => 
        piece switch
        {
            EGamePiece.X => "X", // Return "X" for X pieces
            EGamePiece.O => "O", // Return "O" for O pieces
            _ => " " // Return space for empty cells
        };
}
