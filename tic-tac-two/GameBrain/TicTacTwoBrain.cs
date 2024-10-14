namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    private EGamePiece _currentPlayer;

    private GameConfiguration _gameConfiguration;
    
    private int _piecesLeftX;
    private int _piecesLeftO;
    private int _movesMadeX;
    private int _movesMadeO;

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _currentPlayer = EGamePiece.X;
        _piecesLeftX = _gameConfiguration.PiecesNumber;
        _piecesLeftO = _gameConfiguration.PiecesNumber;
        _movesMadeX = 0;
        _movesMadeO = 0;
    }

    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }
    public EGamePiece CurrentPlayer => _currentPlayer;

    public int DimensionX => _gameBoard.GetLength(0);
    public int DimensionY => _gameBoard.GetLength(1);
    
    public int PiecesLeftX => _piecesLeftX;
    public int PiecesLeftO => _piecesLeftO;

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

   public bool MakeAMove()
{
    bool canMoveExistingPiece = _currentPlayer == EGamePiece.X
        ? _movesMadeX >= _gameConfiguration.MovePieceAfterNMove
        : _movesMadeO >= _gameConfiguration.MovePieceAfterNMove;

    // Ask player if they want to move an existing piece after the set number of moves
    if (canMoveExistingPiece)
    {
        Console.WriteLine("Do you want to move an existing piece? (y/n)");
        var response = Console.ReadLine();
        if (response != null && response.ToLower() == "y")
        {
            // Player chooses to move an existing piece
            return MoveExistingPiece();
        }
    }

    // Proceed with placing a new piece
    var (inputX, inputY) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");

    if (_gameBoard[inputX, inputY] != EGamePiece.Empty)
    {
        // Tell the user why their move is invalid
        Console.WriteLine("Invalid move. The spot is already occupied by another piece.");
        PauseBeforeClear();
        return false;
    }

    // Place the piece
    _gameBoard[inputX, inputY] = _currentPlayer;

    // Update pieces left and move count
    if (_currentPlayer == EGamePiece.X)
    {
        _piecesLeftX--;
        _movesMadeX++;
    }
    else
    {
        _piecesLeftO--;
        _movesMadeO++;
    }

    SwitchPlayer();
    return true;
}

private bool MoveExistingPiece()
{
    // Get the coordinates of the piece to move
    var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");

    // Check if the selected piece is the player's own piece
    if (_gameBoard[oldX, oldY] != _currentPlayer)
    {
        Console.WriteLine("Invalid selection. That is not your piece. Please try again.");
        PauseBeforeClear();
        return false;
    }

    // Get the coordinates for the new position
    var (newX, newY) = GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

    // Check if the new position is already occupied
    if (_gameBoard[newX, newY] != EGamePiece.Empty)
    {
        if (_gameBoard[newX, newY] == _currentPlayer)
        {
            Console.WriteLine("Invalid move. You cannot place your piece on top of your own piece. Please try again.");
        }
        else
        {
            Console.WriteLine("Invalid move. You cannot place your piece on top of the opponent's piece. Please try again.");
        }
        PauseBeforeClear();
        return false;
    }

    // Move the piece
    _gameBoard[newX, newY] = _currentPlayer;
    _gameBoard[oldX, oldY] = EGamePiece.Empty;

    SwitchPlayer();
    return true;
}

private void PauseBeforeClear()
{
    // Wait for the player to press a key before clearing the console
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(); // Waits for a key press
    Console.Clear();   // Clear the console after the player presses a key
}
    
    private (int, int) GetCoordinatesFromPlayer(string promptMessage)
    {
        Console.WriteLine(promptMessage);
        var input = Console.ReadLine();

        // Assuming input is in the format "x,y", split it into two values
        var coordinates = input?.Split(',');
        if (coordinates == null || coordinates.Length != 2 ||
            !int.TryParse(coordinates[0], out int x) || !int.TryParse(coordinates[1], out int y))
        {
            Console.WriteLine("Invalid input. Please enter valid coordinates in the format 'x,y'.");
            return GetCoordinatesFromPlayer(promptMessage); // Recursively prompt again
        }

        return (x, y);
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
    
    public bool CheckDraw()
    {
        bool isBoardFull = true;
        for (int x = 0; x < DimensionX; x++)
        {
            for (int y = 0; y < DimensionY; y++)
            {
                if (_gameBoard[x, y] == EGamePiece.Empty)
                {
                    isBoardFull = false;
                    break; // Exit the loop early if we find an empty spot
                }
            }
            if (!isBoardFull) break;
        }

        // A draw occurs if either the board is full or both players are out of pieces and there's no winner
        return (isBoardFull || (_piecesLeftX <= 0 && _piecesLeftO <= 0)) && !CheckWin();
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

