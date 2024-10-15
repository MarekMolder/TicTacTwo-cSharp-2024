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
    private int _gridWidth;
    private int _gridHeight;
    private int _gridPositionX;
    private int _gridPositionY;
    private bool _usesGrid;

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _currentPlayer = EGamePiece.X;
        _piecesLeftX = _gameConfiguration.PiecesNumber;
        _piecesLeftO = _gameConfiguration.PiecesNumber;
        _movesMadeX = 0;
        _movesMadeO = 0;
        _gridWidth = _gameConfiguration.GridSizeWidth;
        _gridHeight = _gameConfiguration.GridSizeHeight; 
        _gridPositionX = _gameConfiguration.GridPositionX;
        _gridPositionY = _gameConfiguration.GridPositionY;
        _usesGrid = _gameConfiguration.UsesGrid;
    }

    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }
    public EGamePiece CurrentPlayer => _currentPlayer;
    
    public int GridSizeWidth => _gridWidth;
    public int GridSizeHeight => _gridHeight;

    public int GridPositionX => _gridPositionX;
    public int GridPositionY => _gridPositionY;

    public int DimensionX => _gameBoard.GetLength(0);
    public int DimensionY => _gameBoard.GetLength(1);
    
    public int PiecesLeftX => _piecesLeftX;
    public int PiecesLeftO => _piecesLeftO;
    public bool UsesGrid => _usesGrid;

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
    
    bool canMoveExistingGrid = _currentPlayer == EGamePiece.X
        ? _movesMadeX >= _gameConfiguration.MoveGridAfterNMove
        : _movesMadeO >= _gameConfiguration.MoveGridAfterNMove;

    // If the player has no pieces left, they can only move existing pieces
    if (_currentPlayer == EGamePiece.X && _piecesLeftX <= 0)
    {
        if (canMoveExistingPiece && canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an existing piece or grid.");
            return moveGridAndPiece();
        }
        else if (canMoveExistingPiece && !canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an existing piece.");
            return MoveExistingPiece(); // and can move existing grid
        }
        else if (!canMoveExistingPiece && canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an grid.");
            return MoveGrid(); // and can move existing grid
        }
        else
        {
            Console.WriteLine("You have no pieces left and cannot move any existing pieces or grid. Your turn is skipped.");
            return true; // End turn without a move
        }
    }
    else if (_currentPlayer == EGamePiece.O && _piecesLeftO <= 0)
    {
        if (canMoveExistingPiece && canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an existing piece or grid.");
            return MoveExistingPiece(); // and can move existing grid
        }
        else if (canMoveExistingPiece && !canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an existing piece.");
            return MoveExistingPiece(); // and can move existing grid
        }
        else if (!canMoveExistingPiece && canMoveExistingGrid)
        {
            Console.WriteLine("You have no pieces left to place. You can only move an grid.");
            return MoveGrid(); // and can move existing grid
        }
        else
        {
            Console.WriteLine("You have no pieces left and cannot move any existing pieces or grid. Your turn is skipped.");
            return true; // End turn without a move
        }
    }

    // If the player has pieces left, ask if they want to move an existing piece
    if (canMoveExistingPiece && canMoveExistingGrid)
    {
        string response;
        do
        {
            Console.WriteLine("Do you want to move a new piece (new), old piece (old), grid (grid) (new/old/grid)");
            response = Console.ReadLine();

            // Check for valid input
            if (response != null && response.ToLower() == "old")
            {
                // Player chooses to move an existing piece
                return MoveExistingPiece();
            }
            else if (response != null && response.ToLower() == "new")
            {
                // Player chooses to place a new piece
                break;
            }
            else if (response != null && response.ToLower() == "grid")
            {
                // Player chooses to place a grid
                return MoveGrid();
            }
            else
            {
                // Invalid input, prompt again
                Console.WriteLine("Invalid input. Please enter 'new' or 'old' or 'grid'.");
            }
        } while (true);
    }
    else if (!canMoveExistingPiece && canMoveExistingGrid)
    {
        string response;
        do
        {
            Console.WriteLine("Do you want to move a new piece (new), grid (grid) (new/grid)");
            response = Console.ReadLine();

            // Check for valid input
             if (response != null && response.ToLower() == "new")
            {
                // Player chooses to place a new piece
                break;
            }
            else if (response != null && response.ToLower() == "grid")
            {
                // Player chooses to place a grid
                return MoveGrid();
            }
            else
            {
                // Invalid input, prompt again
                Console.WriteLine("Invalid input. Please enter 'new' or 'grid'.");
            }
        } while (true);
    }
    else if (canMoveExistingPiece && !canMoveExistingGrid)
    {
        string response;
        do
        {
            Console.WriteLine("Do you want to move a new piece (new), old piece (old) (new/old)");
            response = Console.ReadLine();

            // Check for valid input
            if (response != null && response.ToLower() == "new")
            {
                // Player chooses to place a new piece
                break;
            }
            else if (response != null && response.ToLower() == "old")
            {
                // Player chooses to place a grid
                return MoveExistingPiece();
            }
            else
            {
                // Invalid input, prompt again
                Console.WriteLine("Invalid input. Please enter 'new' or 'old'.");
            }
        } while (true);
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

private bool moveGridAndPiece()
{
    string response;
    do
    {
        Console.WriteLine("Do you want to move a old piece (old) or grid (grid) (old/grid)");
        response = Console.ReadLine();

        // Check for valid input
        if (response != null && response.ToLower() == "old")
        {
            // Player chooses to place a new piece
            return MoveExistingPiece();
        }
        else if (response != null && response.ToLower() == "grid")
        {
            // Player chooses to place a grid
            return MoveGrid();
        }
        else
        {
            // Invalid input, prompt again
            Console.WriteLine("Invalid input. Please enter 'old' or 'grid'.");
        }
    } while (true);
}
   
private bool MoveGrid()
{
    // Get the coordinates of the new grid position
    var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");
    
    // Check if the new grid position is valid
    if (newGridX < 0 || newGridX + _gridWidth > DimensionX || newGridY < 0 || newGridY + _gridHeight > DimensionY)
    {
        Console.WriteLine("Invalid grid position. Please try again.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(); // Waits for a key press
        return MoveGrid(); // Retry moving the grid
    }

    // Move the grid
    _gridPositionX = newGridX;
    _gridPositionY = newGridY;

    // Optionally switch players or perform other logic after moving the grid
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
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(); // Waits for a key press
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

public EGamePiece? CheckWin()
{
    // If grid size is 0, check the whole board
    if (_gridWidth == 0 || _gridHeight == 0)
    {
        // Check rows
        for (int i = 0; i < DimensionX; i++)
        {
            EGamePiece? winner = CheckLine(_gameBoard, i, 0, 0, 1); // Horizontal check
            if (winner != null) return winner; // Return the winner if found
        }

        // Check columns
        for (int i = 0; i < DimensionY; i++)
        {
            EGamePiece? winner = CheckLine(_gameBoard, 0, i, 1, 0); // Vertical check
            if (winner != null) return winner; // Return the winner if found
        }

        // Check diagonals
        for (int i = 0; i <= DimensionX - _gameConfiguration.WinCondition; i++)
        {
            for (int j = 0; j <= DimensionY - _gameConfiguration.WinCondition; j++)
            {
                EGamePiece? winner = CheckLine(_gameBoard, i, j, 1, 1); // Diagonal check (top-left to bottom-right)
                if (winner != null) return winner; // Return the winner if found
            }
        }

        for (int i = 0; i <= DimensionX - _gameConfiguration.WinCondition; i++)
        {
            for (int j = _gameConfiguration.WinCondition - 1; j < DimensionY; j++)
            {
                EGamePiece? winner = CheckLine(_gameBoard, i, j, 1, -1); // Diagonal check (top-right to bottom-left)
                if (winner != null) return winner; // Return the winner if found
            }
        }
    }
    else
    {
        // Check for win within the grid
        return CheckWinWithinGrid();
    }

    return null; // No winner found
}

private EGamePiece? CheckWinWithinGrid()
{
    // Check rows within the grid
    for (int x = _gridPositionX; x < _gridPositionX + _gridWidth; x++)
    {
        for (int y = _gridPositionY; y < _gridPositionY + _gridHeight; y++)
        {
            // Each CheckLine call now returns a nullable EGamePiece
            EGamePiece? winner = CheckLine(_gameBoard, x, y, 0, 1) // Horizontal check
                                 ?? CheckLine(_gameBoard, x, y, 1, 0) // Vertical check
                                 ?? CheckLine(_gameBoard, x, y, 1, 1) // Diagonal check (top-left to bottom-right)
                                 ?? CheckLine(_gameBoard, x, y, 1, -1); // Diagonal check (top-right to bottom-left)

            if (winner != null) return winner; // Win found within the grid
        }
    }

    return null; // No win found within the grid
}

private EGamePiece? CheckLine(EGamePiece[,] board, int startX, int startY, int deltaX, int deltaY)
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
            return currentPiece; // Return the current piece as the winner
    }
    return null; // No winning line found
}
    
    public bool CheckDraw()
    {
        bool isBoardFull = true;
        bool canMoveExistingPieceX = _movesMadeX >= _gameConfiguration.MovePieceAfterNMove && _piecesLeftX <= 0;
        bool canMoveExistingPieceO = _movesMadeO >= _gameConfiguration.MovePieceAfterNMove && _piecesLeftO <= 0;

        // Check if the board is full
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

        // A draw occurs if:
        // 1. The board is full
        // 2. Both players are out of pieces AND neither player can make a move
        return isBoardFull || 
               (_piecesLeftX <= 0 && _piecesLeftO <= 0 && !canMoveExistingPieceX && !canMoveExistingPieceO);
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
