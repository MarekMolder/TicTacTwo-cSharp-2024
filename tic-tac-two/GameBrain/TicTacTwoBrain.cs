namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    private EGamePiece _currentPlayer;

    private GameConfiguration _gameConfiguration;

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _currentPlayer = EGamePiece.X;
    }

    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }
    public EGamePiece CurrentPlayer => _currentPlayer;

    public int DimensionX => _gameBoard.GetLength(0);
    public int DimensionY => _gameBoard.GetLength(1);

    private EGamePiece[,] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var x = 0; x < _gameBoard.GetLength(0); x++)
        {
            for (var y = 0; y < _gameBoard.GetLength(1); y++)
            {
                copyOfBoard[x, y] = _gameBoard[x, y];
            }
        }
        return copyOfBoard;
    }

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x, y] = _currentPlayer;
        SwitchPlayer();
        return true;
    }
    
    private void SwitchPlayer()
    {
        _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X; // Toggle current player
    }

    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _currentPlayer = EGamePiece.X;
    }

    public bool CheckWin()
    {
        // Check rows
        for (int i = 0; i < DimensionX; i++)
        {
            if (CheckLine(_gameBoard, i, 0, 0, 1)) return true; // Horizontal check
        }

        // Check columns
        for (int i = 0; i < DimensionY; i++)
        {
            if (CheckLine(_gameBoard, 0, i, 1, 0)) return true; // Vertical check
        }

        // Check diagonals (top-left to bottom-right)
        for (int i = 0; i <= DimensionX - _gameConfiguration.WinCondition; i++)
        {
            for (int j = 0; j <= DimensionY - _gameConfiguration.WinCondition; j++)
            {
                if (CheckLine(_gameBoard, i, j, 1, 1)) return true; // Diagonal check (top-left to bottom-right)
            }
        }

        // Check diagonals (top-right to bottom-left)
        for (int i = 0; i <= DimensionX - _gameConfiguration.WinCondition; i++)
        {
            for (int j = _gameConfiguration.WinCondition - 1; j < DimensionY; j++)
            {
                if (CheckLine(_gameBoard, i, j, 1, -1)) return true; // Diagonal check (top-right to bottom-left)
            }
        }

        return false; // No winner found
    }

    
    private bool CheckLine(EGamePiece[,] board, int startX, int startY, int deltaX, int deltaY)
    {
        int count = 0;
        EGamePiece currentPiece = EGamePiece.Empty;

        for (int i = 0; i < Math.Max(DimensionX, DimensionY); i++)
        {
            int x = startX + i * deltaX;
            int y = startY + i * deltaY;

            // Check if the position is within the board limits
            if (x >= DimensionX || y >= DimensionY || x < 0 || y < 0)
                break;

            if (board[x, y] == currentPiece && currentPiece != EGamePiece.Empty)
            {
                count++;
            }
            else
            {
                currentPiece = board[x, y];
                count = (currentPiece == EGamePiece.Empty) ? 0 : 1; // Reset count if current piece is empty
            }

            // Check if the current count has reached the win condition
            if (count >= _gameConfiguration.WinCondition)
                return true;
        }
        return false; // No winning line found
    }
    
    // Method to get valid coordinates from the user
    public (int x, int y) GetValidCoordinates()
    {
        while (true)
        {
            Console.Write("Give me coordinates <x,y>: ");
            var input = Console.ReadLine()!;
            var inputSplit = input.Split(',');

            // Check if input is valid
            if (inputSplit.Length != 2 || 
                !int.TryParse(inputSplit[0], out int x) || 
                !int.TryParse(inputSplit[1], out int y) || 
                x < 0 || x >= DimensionX || 
                y < 0 || y >= DimensionY)
            {
                Console.WriteLine("Invalid input. Please enter coordinates as <x,y> within the board limits.");
                continue; // Ask for input again
            }

            return (x, y); // Return valid coordinates
        }
    }
}

