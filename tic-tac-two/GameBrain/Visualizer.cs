namespace GameBrain;

public class Visualizer
{
     public static void DrawBoard(TicTacTwoBrain gameInstance)
        {
            // Get grid parameters
            var gridStartX = gameInstance.GridPositionX;
            var gridStartY = gameInstance.GridPositionY;

            var gridWidth = gameInstance.GridSizeWidth;
            var gridHeight = gameInstance.GridSizeHeight;
            
            int gridEndX = gridStartX + gridWidth;
            int gridEndY = gridStartY + gridHeight;

            // Draw the column numbers
            Console.Write("   "); // Space for row numbers
            for (var x = 0; x < gameInstance.DimensionX; x++)
            {
                Console.Write(" " + x + " "); // Column numbers
                if (x != gameInstance.DimensionX - 1)
                {
                    Console.Write("|");
                }
            }
            Console.WriteLine();

            // Draw the board
            for (var y = 0; y < gameInstance.DimensionY; y++)
            {
                Console.Write(y + " |"); // Row number
                for (var x = 0; x < gameInstance.DimensionX; x++)
                {
                    // Set the background color for the grid area
                    if (gameInstance.UsesGrid)
                    {
                        if (x >= gridStartX && x < gridEndX && y >= gridStartY && y < gridEndY)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray; // Set background color to gray for the grid
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black; // Reset to black for other areas
                    }
                    EGamePiece pieceToDraw = gameInstance.GameBoard[x, y];
                    
                    if (pieceToDraw == EGamePiece.X)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (pieceToDraw == EGamePiece.O)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(" " + DrawGamePiece(pieceToDraw) + " ");
                    Console.ResetColor(); // Reset color after drawing the piece

                    if (x == gameInstance.DimensionX - 1) continue; // Don't write the right border
                    if (gameInstance.UsesGrid)
                    {
                        if (x >= gridStartX - 1 && x < gridEndX && y >= gridStartY && y < gridEndY)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen; // Set background color to red for grid borders
                        }
                    }
                    Console.Write("|");
                    Console.ResetColor();
                }
                Console.WriteLine();
                if (y == gameInstance.DimensionY - 1) continue; // Don't write the bottom border
                Console.Write("  +");
                for (var x = 0; x < gameInstance.DimensionX; x++)
                {
                    if (gameInstance.UsesGrid)
                    {
                        if (x >= gridStartX && x < gridEndX && y >= gridStartY - 1 && y < gridEndY)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen; // Set color for horizontal grid border
                        }
                    }
                    Console.Write("---");

                    if (x != gameInstance.DimensionX - 1)
                    {
                        if (gameInstance.UsesGrid)
                        {
                            if (x >= gridStartX - 1 && x < gridEndX - 1 && y >= gridStartY - 1 && y < gridEndY)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            }
                        }
                        Console.Write("+");
                    }
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
            Console.ResetColor(); // Reset all console colors at the end

        }

    
    private static string DrawGamePiece(EGamePiece piece) => 
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}