using GameBrain;

namespace ConsoleUI;

/// <summary>
/// Provides methods for drawing and rendering the game board for a TicTacTwo game.
/// </summary>
public class Visualizer
{
    /// <summary>
    /// Draws the entire game board to the console, displaying the grid and the game pieces.
    /// </summary>
    /// <param name="gameInstance">The game instance containing the board configuration and state.</param>
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        int gridStartX = gameInstance.GridPositionX;
        int gridStartY = gameInstance.GridPositionY;
        int gridWidth = gameInstance.GridSizeWidth;
        int gridHeight = gameInstance.GridSizeHeight;
        int gridEndX = gridStartX + gridWidth;
        int gridEndY = gridStartY + gridHeight;
        
        Console.Write("   ");
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            Console.Write($" {x} ");
            if (x != gameInstance.DimensionX - 1) Console.Write("|");
        }
        Console.WriteLine();
        
        for (var y = 0; y < gameInstance.DimensionY; y++)
        {
            Console.Write($"{y} |");
            for (var x = 0; x < gameInstance.DimensionX; x++)
            {
                SetBackgroundColorForGrid(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY);
                SetForegroundColorForPiece(gameInstance.GameBoard[x][y]);

                Console.Write($" {DrawGamePiece(gameInstance.GameBoard[x][y])} ");
                Console.ResetColor();

                if (x != gameInstance.DimensionX - 1) 
                    DrawVerticalSeparator(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY);
            }
            Console.WriteLine();

            if (y != gameInstance.DimensionY - 1) 
                DrawHorizontalSeparator(gameInstance, y, gridStartX, gridEndX, gridStartY, gridEndY);
        }

        Console.ResetColor();
    }
    
    /// <summary>
    /// Sets the background color for the grid based on the current grid position.
    /// </summary>
    /// <param name="gameInstance">The game instance containing the board and grid configuration.</param>
    /// <param name="x">The x-coordinate of the current cell.</param>
    /// <param name="y">The y-coordinate of the current cell.</param>
    /// <param name="gridStartX">The starting x-coordinate of the grid.</param>
    /// <param name="gridEndX">The ending x-coordinate of the grid.</param>
    /// <param name="gridStartY">The starting y-coordinate of the grid.</param>
    /// <param name="gridEndY">The ending y-coordinate of the grid.</param>
    private static void SetBackgroundColorForGrid(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        if (gameInstance.UsesGrid && x >= gridStartX && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    
    /// <summary>
    /// Sets the foreground color for the game piece based on its type (X or O).
    /// </summary>
    /// <param name="piece">The game piece (either X or O).</param>
    private static void SetForegroundColorForPiece(EGamePiece piece)
    {
        switch (piece)
        {
            case EGamePiece.X:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case EGamePiece.O:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            default:
                Console.ResetColor();
                break;
        }
    }
    
    /// <summary>
    /// Draws a vertical separator for the grid, if applicable.
    /// </summary>
    /// <param name="gameInstance">The game instance containing the grid configuration.</param>
    /// <param name="x">The x-coordinate of the current cell.</param>
    /// <param name="y">The y-coordinate of the current cell.</param>
    /// <param name="gridStartX">The starting x-coordinate of the grid.</param>
    /// <param name="gridEndX">The ending x-coordinate of the grid.</param>
    /// <param name="gridStartY">The starting y-coordinate of the grid.</param>
    /// <param name="gridEndY">The ending y-coordinate of the grid.</param>
    private static void DrawVerticalSeparator(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        if (gameInstance.UsesGrid && x >= gridStartX - 1 && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.Write("|");
        Console.ResetColor();
    }
    
    /// <summary>
    /// Draws a horizontal separator for the grid, if applicable.
    /// </summary>
    /// <param name="gameInstance">The game instance containing the grid configuration.</param>
    /// <param name="y">The y-coordinate of the current row.</param>
    /// <param name="gridStartX">The starting x-coordinate of the grid.</param>
    /// <param name="gridEndX">The ending x-coordinate of the grid.</param>
    /// <param name="gridStartY">The starting y-coordinate of the grid.</param>
    /// <param name="gridEndY">The ending y-coordinate of the grid.</param>
    private static void DrawHorizontalSeparator(TicTacTwoBrain gameInstance, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        Console.Write("  +");
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            if (gameInstance.UsesGrid && x >= gridStartX && x < gridEndX && y >= gridStartY - 1 && y < gridEndY)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            Console.Write("---");

            if (x != gameInstance.DimensionX - 1) Console.Write("+");
            Console.ResetColor();
        }
        Console.WriteLine();
    }
    
    /// <summary>
    /// Converts a game piece to its string representation for display on the console.
    /// </summary>
    /// <param name="piece">The game piece (either X or O).</param>
    /// <returns>A string representing the game piece.</returns>
    public static string DrawGamePiece(EGamePiece piece) => 
        piece switch
        {
            EGamePiece.X => "X",
            EGamePiece.O => "O",
            _ => " "
        };
}
