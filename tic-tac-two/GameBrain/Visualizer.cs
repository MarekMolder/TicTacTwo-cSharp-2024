namespace GameBrain;

public class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance) 
    {
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
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x, y]) + " ");
                if (x != gameInstance.DimensionX - 1)
                {
                    Console.Write("|");
                }
            }

            Console.WriteLine();

            // Draw the separator line between rows
            if (y == gameInstance.DimensionY - 1) continue;
            Console.Write("  +");
            for (var x = 0; x < gameInstance.DimensionX; x++)
            {
                Console.Write("---");
                if (x != gameInstance.DimensionX - 1)
                {
                    Console.Write("+");
                }
            }
            Console.WriteLine();
        }
    }

    
    private static string DrawGamePiece(EGamePiece piece) => 
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}