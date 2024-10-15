using GameBrain;

namespace ConsoleUI;

public class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        int gridStartX = gameInstance.GridPositionX, gridStartY = gameInstance.GridPositionY;
        int gridWidth = gameInstance.GridSizeWidth, gridHeight = gameInstance.GridSizeHeight;
        int gridEndX = gridStartX + gridWidth, gridEndY = gridStartY + gridHeight;

        // Joonista veergude numbrid
        Console.Write("   ");
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            Console.Write($" {x} ");
            if (x != gameInstance.DimensionX - 1) Console.Write("|");
        }
        Console.WriteLine();

        // Joonista mängulaud
        for (var y = 0; y < gameInstance.DimensionY; y++)
        {
            Console.Write($"{y} |"); // Rea numbrid
            for (var x = 0; x < gameInstance.DimensionX; x++)
            {
                SetBackgroundColorForGrid(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY);
                SetForegroundColorForPiece(gameInstance.GameBoard[x, y]);

                Console.Write($" {DrawGamePiece(gameInstance.GameBoard[x, y])} ");
                Console.ResetColor();

                if (x != gameInstance.DimensionX - 1) DrawVerticalSeparator(gameInstance, x, y, gridStartX, gridEndX, gridStartY, gridEndY);
            }
            Console.WriteLine();

            if (y != gameInstance.DimensionY - 1) DrawHorizontalSeparator(gameInstance, y, gridStartX, gridEndX, gridStartY, gridEndY);
        }

        Console.ResetColor(); // Taasta algseaded
    }

    private static void SetBackgroundColorForGrid(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        // Seadista taustavärv gridi piires
        if (gameInstance.UsesGrid && x >= gridStartX && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    private static void SetForegroundColorForPiece(EGamePiece piece)
    {
        // Seadista esiplaani värv X ja O jaoks
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

    private static void DrawVerticalSeparator(TicTacTwoBrain gameInstance, int x, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        // Joonista vertikaaljooned gridi piires
        if (gameInstance.UsesGrid && x >= gridStartX - 1 && x < gridEndX && y >= gridStartY && y < gridEndY)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        Console.Write("|");
        Console.ResetColor();
    }

    private static void DrawHorizontalSeparator(TicTacTwoBrain gameInstance, int y, int gridStartX, int gridEndX, int gridStartY, int gridEndY)
    {
        Console.Write("  +");
        for (var x = 0; x < gameInstance.DimensionX; x++)
        {
            // Joonista horisontaaljooned gridi piires
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

    private static string DrawGamePiece(EGamePiece piece) => 
        piece switch
        {
            EGamePiece.X => "X",
            EGamePiece.O => "O",
            _ => " "
        };
}
