namespace GameBrain
{
    public class TicTacTwoBrain
    {
        private readonly GameState _gameState;
        
        public TicTacTwoBrain(GameConfiguration gameConfiguration, string playerX, string playerO,int gridPositionY = -1, int gridPositionX = -1, int movesMadeX = 0, int movesMadeO = 0)
        {
            var gameBoard = new EGamePiece[gameConfiguration.BoardSizeWidth][];
            for (var i = 0; i < gameBoard.Length; i++)
            {
                gameBoard[i] = new EGamePiece[gameConfiguration.BoardSizeHeight];
            }
            
            int finalGridPositionX = gridPositionX == -1 ? gameConfiguration.GridPositionX : gridPositionX;
            int finalGridPositionY = gridPositionY == -1 ? gameConfiguration.GridPositionY : gridPositionY;
            
            Console.WriteLine($"Final Grid Position X: {finalGridPositionX}");
            Console.WriteLine($"Final Grid Position Y: {finalGridPositionY}");
            
            // Initialize the game state with default values first
            _gameState = new GameState(
                gameBoard,
                gameConfiguration, // Pass the configuration here
                EGamePiece.X, // Assuming X starts first
                gameConfiguration.PiecesNumber,
                gameConfiguration.PiecesNumber,
                movesMadeX,  // Moves made by player X
                movesMadeO, // Moves made by player O
                playerX,
                playerO,
                finalGridPositionX, // Use calculated final grid position
                finalGridPositionY  // Use calculated final grid position
            );
            
            GridPositionX = finalGridPositionX; // Set the property
            GridPositionY = finalGridPositionY; // Set the property

            // Initialize the game board dimensions and grid settings
            GridSizeWidth = _gameState.GameConfiguration.GridSizeWidth;
            GridSizeHeight = _gameState.GameConfiguration.GridSizeHeight;
            UsesGrid = _gameState.GameConfiguration.UsesGrid;
        }

        public string GetGameStateJson()
        {
            return _gameState.ToString();
        }

        public string GetGameConfigName()
        {
            return _gameState.GameConfiguration.Name;
        }

        // Public properties to access game state information.
        public EGamePiece[][] GameBoard => GetBoardCopy(); // Returns a copy of the game board.
        
public void SetGameBoard(EGamePiece[][] gameBoard)
{
    // Kontrollime, kas mängulaud on määratud
    if (gameBoard == null || gameBoard.Length == 0 || gameBoard[0] == null)
    {
        throw new ArgumentNullException(nameof(gameBoard), "Provided game board cannot be null or empty.");
    }

    // Võtame dimensioonid otse sisendist, mitte praegusest mängu olekust
    int boardDimensionX = gameBoard.Length;
    int boardDimensionY = gameBoard[0].Length;

    // Kontrollime, kas mängulaud sobib praeguse mänguseadistusega
    if (boardDimensionX != DimensionX || boardDimensionY != DimensionY)
    {
        throw new ArgumentException("The dimensions of the provided game board do not match the game configuration.");
    }

    // Sügava koopia tegemine mängulauast
    _gameState.GameBoard = new EGamePiece[boardDimensionX][];
    for (int i = 0; i < boardDimensionX; i++)
    {
        _gameState.GameBoard[i] = new EGamePiece[boardDimensionY];
        Array.Copy(gameBoard[i], _gameState.GameBoard[i], boardDimensionY); // Kopeerime iga rea
    }

    // Kui grid on kasutusel, siis kopeerime grid'i sisu õigesse kohta mängulaual
    if (UsesGrid)
    {
        for (int i = 0; i < GridSizeWidth; i++)
        {
            for (int j = 0; j < GridSizeHeight; j++)
            {
                // Arvutame grid'i sees olevate nuppude tegelikud positsioonid mängulaual
                int targetX = GridPositionX + i;
                int targetY = GridPositionY + j;

                // Veendume, et koordinaadid jäävad mängulaua piiridesse
                if (targetX >= 0 && targetX < boardDimensionX && targetY >= 0 && targetY < boardDimensionY)
                {
                    // Kopeerime grid'i sees oleva nuppu sisu õigesse kohta mängulaual
                    _gameState.GameBoard[targetX][targetY] = gameBoard[targetX][targetY];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Grid position is out of the game board bounds.");
                }
            }
        }
    }
}

        
        public EGamePiece CurrentPlayer
        {
            get => _gameState.CurrentPlayer; // Returns the current player.
            set => _gameState.CurrentPlayer = value;
        }

        public int GridSizeWidth { get; } // Width of the grid.
        public int GridSizeHeight { get; } // Height of the grid.
        
        public int GridPositionX
        {
            get => _gameState.GridPositionX; // Returns the current grid positon x.
            set => _gameState.GridPositionX = value;
        }
        
        public int GridPositionY
        {
            get => _gameState.GridPositionY; // Returns the current grid positon x.
            set => _gameState.GridPositionY = value;
        }
        
        public int DimensionX => _gameState.GameBoard.Length; // Width of the game board.
        public int DimensionY => _gameState.GameBoard[0].Length; // Height of the game board.

        public int PiecesLeftX
        {
            get => _gameState.PiecesLeftX; // Remaining pieces for player X.
            set => _gameState.PiecesLeftX = value;
        }

        public int PiecesLeftO
        {
            get => _gameState.PiecesLeftO; // Remaining pieces for player O.
            set => _gameState.PiecesLeftO = value;
        }

        public bool UsesGrid { get; } // Indicates if the game uses a grid.
        public int MovesMadeX { get; set; }
        public int MovesMadeO { get; set; }

        private EGamePiece[][] GetBoardCopy()
        {
            var copy = new EGamePiece[DimensionX][];
    
            // Loop through each row and create a copy of each inner array
            for (int i = 0; i < DimensionX; i++)
            {
                copy[i] = new EGamePiece[DimensionY];
                Array.Copy(_gameState.GameBoard[i], copy[i], DimensionY);
            }

            return copy;
        }

        public bool SaveTheGame()
        {
            Console.WriteLine("Do you want to save the game?");

            var options = new List<string> { "yes", "no" }; // Default option.

            Console.WriteLine($"Options: {string.Join("/", options)}");
            var response = Console.ReadLine()?.Trim().ToLower();

            // Handle the player's choice based on their input.
            if (response == "yes")
            {
                return true;
            }

            if (response == "no")
            {
                return false;
            }

            return false;
        }
        
        public bool MakeAMove()
        {
            var canMovePiece = CanMovePiece(); // Check if the player can move a piece.
            var canMoveGrid = CanMoveGrid(); // Check if the player can move the grid.
            var hasPiecesLeft = HasPiecesLeft(); // Check if the player has pieces left.

            // If no pieces left, handle accordingly.
            if (!hasPiecesLeft)
            {
                return HandleNoPiecesLeft(canMovePiece, canMoveGrid);
            }

            // If the player can move a piece or the grid, handle the player's choice.
            if (canMovePiece || canMoveGrid)
            {
                return HandlePlayerChoice(canMovePiece, canMoveGrid);
            }

            // If the player cannot move existing pieces or the grid, place a new piece.
            return PlaceNewPiece();
        }
        
        private bool HasPiecesLeft()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? _gameState.PiecesLeftX > 0 : _gameState.PiecesLeftO > 0;
        }
        
        private bool CanMovePiece()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX :_gameState. MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove; // Check if the required moves have been made.
        }
        
        private bool CanMoveGrid()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MoveGridAfterNMove; // Check if the required moves have been made.
        }
        
        private bool HandleNoPiecesLeft(bool canMovePiece, bool canMoveGrid)
        {
            // If both movements are possible, prompt the player.
            if (canMovePiece && canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece or the grid.");
                return PromptMovePieceOrGrid();
            }

            // If only moving a piece is possible, move it.
            if (canMovePiece)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece.");
                return MoveExistingPiece();
            }

            // If only moving the grid is possible, move it.
            if (canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move the grid.");
                return MoveGrid();
            }

            // If no valid moves, skip the player's turn.
            Console.WriteLine("You have no valid moves. Your turn is skipped.");
            SwitchPlayer();
            return true;
        }
        
        private bool HandlePlayerChoice(bool canMovePiece, bool canMoveGrid)
        {
            while (true)
            {
                // Prompt the player for their choice.
                Console.WriteLine(
                    $"Do you want to place a new piece{(canMovePiece ? ", move an existing piece" : "")}{(canMoveGrid ? ", or move the grid" : "")}?");

                var options = new List<string> { "new" }; // Default option.
                if (canMovePiece) options.Add("old"); // Add option to move an existing piece.
                if (canMoveGrid) options.Add("grid"); // Add option to move the grid.

                Console.WriteLine($"Options: {string.Join("/", options)}");
                var response = Console.ReadLine()?.Trim().ToLower();

                // Handle the player's choice based on their input.
                if (response == "new")
                {
                    return PlaceNewPiece();
                }

                if (response == "old" && canMovePiece)
                {
                    return MoveExistingPiece();
                }

                if (response == "grid" && canMoveGrid)
                {
                    return MoveGrid();
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }
        
        private bool PlaceNewPiece()
        {
            // Get coordinates from the player where they want to place their piece
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");

            // Check if the chosen spot is already occupied
            if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Place the current player's piece on the board
            _gameState.GameBoard[x][y] = _gameState.CurrentPlayer;

            // Update the player's state after making the move
            UpdatePlayerStateAfterMove();

            // Switch to the next player
            SwitchPlayer();

            return true; // Move was successful
        }
        
        private bool MoveExistingPiece()
        {
            // Get the coordinates of the piece the player wants to move
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");

            // Check if the selected piece belongs to the current player
            if (_gameState.GameBoard[oldX][oldY] != _gameState.CurrentPlayer)
            {
                Console.WriteLine("Invalid selection. That is not your piece.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Get the new coordinates where the player wants to move the piece
            var (newX, newY) =
                GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

            // Check if the destination spot is already occupied
            if (_gameState.GameBoard[newX][newY] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Move the piece to the new coordinates
            _gameState.GameBoard[newX][newY] = _gameState.CurrentPlayer;

            // Clear the old position of the piece
            _gameState.GameBoard[oldX][oldY] = EGamePiece.Empty;

            // Switch to the next player
            SwitchPlayer();

            return true; // Move was successful
        }
        
        private bool MoveGrid()
        {
            // Get new coordinates for the grid from the player
            var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");

            // Check if the new grid position is valid
            if (!IsValidGridPosition(newGridX, newGridY))
            {
                Console.WriteLine("Invalid grid position. Please try again.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Update the grid's position
            GridPositionX = newGridX;
            GridPositionY = newGridY;

            // Switch to the next player
            SwitchPlayer();
            return true; // Move was successful
        }
        
        private bool IsValidGridPosition(int x, int y)
        {
            // Check if the grid position is within the dimensions of the board
            return x >= 0 && x + GridSizeWidth <= DimensionX && y >= 0 && y + GridSizeHeight <= DimensionY;
        }
        
        private void UpdatePlayerStateAfterMove()
        {
            // Update player state based on which piece is currently active
            if (_gameState.CurrentPlayer == EGamePiece.X)
            {
                _gameState.PiecesLeftX--; // Decrement pieces left for player X
                _gameState.MovesMadeX++; // Increment moves made for player X
            }
            else
            {
                _gameState.PiecesLeftO--; // Decrement pieces left for player O
                _gameState.MovesMadeO++; // Increment moves made for player O
            }
        }
        
        private void SwitchPlayer()
        {
            // Toggle the current player between X and O
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        
        private (int x, int y) GetCoordinatesFromPlayer(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt); // Display the prompt to the user
                var input = Console.ReadLine(); // Read user input

                // Attempt to parse the input into coordinates and check if they are within board limits
                if (TryParseCoordinates(input, out int x, out int y) && IsWithinBoard(x, y))
                {
                    return (x, y); // Return the valid coordinates
                }

                Console.WriteLine(
                    "Invalid input. Please enter valid coordinates within the board limits."); // Error message for invalid input
            }
        }
        
        private bool TryParseCoordinates(string? input, out int x, out int y)
        {
            x = y = -1; // Initialize output coordinates
            var parts = input?.Split(','); // Split input by comma
            return parts != null && parts.Length == 2 // Check if there are two parts
                                 && int.TryParse(parts[0], out x) // Try parsing the first part as x
                                 && int.TryParse(parts[1], out y); // Try parsing the second part as y
        }
        
        private bool IsWithinBoard(int x, int y)
        {
            // Validate the coordinates against the board's dimensions
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }
        
        private void PauseBeforeContinue()
        {
            Console.WriteLine("Press any key to continue..."); // Prompt the user
            Console.ReadKey(); // Wait for a key press
            Console.Clear(); // Clear the console
        }
        
        private bool PromptMovePieceOrGrid()
        {
            while (true)
            {
                Console.WriteLine(
                    "Do you want to move an existing piece (old) or move the grid (grid)? Options: old/grid");
                var response = Console.ReadLine()?.Trim().ToLower(); // Read and normalize user input

                if (response == "old")
                {
                    return MoveExistingPiece(); // Move an existing piece
                }

                if (response == "grid")
                {
                    return MoveGrid(); // Move the grid
                }

                Console.WriteLine("Invalid input. Please try again."); // Error message for invalid input
            }
        }
        
        public EGamePiece? CheckWin()
        {
            // Check winning condition based on whether the grid is used
            if (GridSizeWidth == 0 || GridSizeHeight == 0)
            {
                return CheckWinOnWholeBoard(); // Check for win on the entire board
            }

            return CheckWinWithinGrid(); // Check for win within the specified grid
        }
        
        private EGamePiece? CheckWinOnWholeBoard()
        {
            return CheckAllLines(_gameState.GameBoard, 0, 0, DimensionX, DimensionY); // Check all lines in the whole board
        }
        
        private EGamePiece? CheckWinWithinGrid()
        {
            return CheckAllLines(_gameState.GameBoard, GridPositionX, GridPositionY, GridSizeWidth,
                GridSizeHeight); // Check lines within the grid
        }
        
        private EGamePiece? CheckAllLines(EGamePiece[][] board, int startX, int startY, int width, int height)
        {
            // Check rows and columns for a winner
            for (int i = 0; i < width; i++)
            {
                var winner = CheckLine(board, startX + i, startY, 0, 1, height); // Check vertical line
                if (winner != null) return winner; // Return the winner if found

                winner = CheckLine(board, startX, startY + i, 1, 0, width); // Check horizontal line
                if (winner != null) return winner; // Return the winner if found
            }

            // Check diagonals for a winner
            var diagWinner = CheckLine(board, startX, startY, 1, 1, Math.Min(width, height)); // Top-left to bottom-right diagonal
            if (diagWinner != null) return diagWinner; // Return the winner if found

            diagWinner = CheckLine(board, startX + width - 1, startY, -1, 1, Math.Min(width, height)); // Top-right to bottom-left diagonal
            return diagWinner; // Return the winner if found or null if no winner
        }
        
        private EGamePiece? CheckLine(EGamePiece[][] board, int startX, int startY, int deltaX, int deltaY, int length)
        {
            int count = 0; // Count of consecutive pieces
            EGamePiece currentPiece = EGamePiece.Empty; // Currently checked piece

            for (int i = 0; i < length; i++)
            {
                int x = startX + i * deltaX; // Calculate current X
                int y = startY + i * deltaY; // Calculate current Y

                // Break if coordinates are out of board bounds
                if (x >= DimensionX || y >= DimensionY || x < 0 || y < 0 || y >= board[x].Length)
                    break;

                var piece = board[x][y]; // Get the piece at the current position
                // Check if the current piece matches the last counted piece
                if (piece == currentPiece && piece != EGamePiece.Empty)
                {
                    count++; // Increment count for consecutive pieces
                }
                else
                {
                    currentPiece = piece; // Update current piece
                    count = piece != EGamePiece.Empty ? 1 : 0; // Reset count or start new count
                }

                // Check if the count meets the win condition
                if (count >= _gameState.GameConfiguration.WinCondition)
                    return currentPiece; // Return the current piece as the winner
            }

            return null; // No winner found
        }
        
        public bool CheckDraw()
        {
            if (IsBoardFull()) // Check if the board is full
            {
                return true; // It's a draw
            }

            // Determine if either player can make a move
            var canMoveX = _gameState.PiecesLeftX > 0 || CanMovePieceForPlayer(EGamePiece.X);
            var canMoveO = _gameState.PiecesLeftO > 0 || CanMovePieceForPlayer(EGamePiece.O);

            // It's a draw if neither player can make a move
            return !canMoveX && !canMoveO;
        }
        
        private bool IsBoardFull()
        {
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (_gameState.GameBoard[x][y] == EGamePiece.Empty) // Check for any empty spot
                    {
                        return false; // The board is not full
                    }
                }
            }

            return true; // The board is full
        }
        
        private bool CanMovePieceForPlayer(EGamePiece player)
        {
            var movesMade = player == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO; // Get the moves made by the player
            var piecesLeft = player == EGamePiece.X ? _gameState.PiecesLeftX : _gameState.PiecesLeftO; // Get the pieces left for the player

            // Check if the player has made enough moves and has no pieces left
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove && piecesLeft <= 0;
        }
        
    }
}
