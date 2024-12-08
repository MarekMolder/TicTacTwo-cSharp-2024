using Domain;

namespace GameBrain
{
    /// <summary>
    /// The <see cref="TicTacTwoBrain"/> class represents the core game logic for a Tic-Tac-Toe game
    /// with additional features like grid manipulation and piece movements. It manages the game state,
    /// player turns, and actions like moving pieces or grids.
    /// </summary>
    public class TicTacTwoBrain
    {
        private GameState _gameState;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacTwoBrain"/> class with the specified game configuration,
        /// players, and initial settings for grid and pieces.
        /// </summary>
        /// <param name="gameConfiguration">The game configuration that defines the board size and other settings.</param>
        /// <param name="playerX">The name of the player who plays as 'X'.</param>
        /// <param name="playerO">The name of the player who plays as 'O'.</param>
        /// <param name="gridPositionY">The initial Y position for the grid (default is -1).</param>
        /// <param name="gridPositionX">The initial X position for the grid (default is -1).</param>
        /// <param name="movesMadeX">The number of moves made by player X (default is 0).</param>
        /// <param name="movesMadeO">The number of moves made by player O (default is 0).</param>
        public TicTacTwoBrain(GameConfiguration gameConfiguration, string playerX = "Player-X", string playerO = "Player-0",int gridPositionY = -1, int gridPositionX = -1, int movesMadeX = 0, int movesMadeO = 0)
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

        /// <summary>
        /// Gets the current game state as a JSON string.
        /// </summary>
        /// <returns>A JSON representation of the game state.</returns>
        public string GetGameStateJson()
        {
            return _gameState.ToString();
        }

        /// <summary>
        /// Gets the name of the current game configuration.
        /// </summary>
        /// <returns>The name of the game configuration.</returns>
        public string GetGameConfigName()
        {
            return _gameState.GameConfiguration.Name;
        }
        
        /// <summary>
        /// Gets the name of the current game configuration.
        /// </summary>
        /// <returns>The name of the game configuration.</returns>
        public GameConfiguration GetGameConfig()
        {
            return _gameState.GameConfiguration;
        }

        /// <summary>
        /// Gets a copy of the current game board.
        /// </summary>
        public EGamePiece[][] GameBoard => GetBoardCopy(); // Returns a copy of the game board.
        
        /// <summary>
        /// Sets the game board to the provided state.
        /// </summary>
        /// <param name="gameBoard">The game board to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided game board is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the provided game board dimensions do not match the game configuration.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the grid position exceeds game board bounds.</exception>    
        public void SetGameBoard(EGamePiece[][] gameBoard)
        {
            // Validates and sets the game board to the provided one.
            if (gameBoard == null || gameBoard.Length == 0 || gameBoard[0] == null)
            {
                throw new ArgumentNullException(nameof(gameBoard), "Provided game board cannot be null or empty.");
            }
            
            int boardDimensionX = gameBoard.Length;
            int boardDimensionY = gameBoard[0].Length;
            
            if (boardDimensionX != DimensionX || boardDimensionY != DimensionY)
            {
                throw new ArgumentException("The dimensions of the provided game board do not match the game configuration.");
            }
            
            _gameState.GameBoard = new EGamePiece[boardDimensionX][];
            for (int i = 0; i < boardDimensionX; i++)
            {
                _gameState.GameBoard[i] = new EGamePiece[boardDimensionY];
                Array.Copy(gameBoard[i], _gameState.GameBoard[i], boardDimensionY);
            }
            
            if (UsesGrid)
            {
                for (int i = 0; i < GridSizeWidth; i++)
                {
                    for (int j = 0; j < GridSizeHeight; j++)
                    {
                        int targetX = GridPositionX + i;
                        int targetY = GridPositionY + j;
                        
                        if (targetX >= 0 && targetX < boardDimensionX && targetY >= 0 && targetY < boardDimensionY)
                        {
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

        /// <summary>
        /// Gets or sets the current player (either 'X' or 'O').
        /// </summary>
        public EGamePiece CurrentPlayer
        {
            get => _gameState.CurrentPlayer;
            set => _gameState.CurrentPlayer = value;
        }

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int GridSizeWidth { get; }
        
        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int GridSizeHeight { get; }
        
        /// <summary>
        /// Gets or sets the X position of the grid.
        /// </summary>
        public int GridPositionX
        {
            get => _gameState.GridPositionX;
            set => _gameState.GridPositionX = value;
        }
        
        /// <summary>
        /// Gets or sets the Y position of the grid.
        /// </summary>
        public int GridPositionY
        {
            get => _gameState.GridPositionY;
            set => _gameState.GridPositionY = value;
        }
        
        /// <summary>
        /// Gets the width of the game board.
        /// </summary>
        public int DimensionX => _gameState.GameBoard.Length; 
        
        /// <summary>
        /// Gets the height of the game board.
        /// </summary>
        public int DimensionY => _gameState.GameBoard[0].Length;

        /// <summary>
        /// Gets or sets the number of remaining pieces for player 'X'.
        /// </summary>
        public int PiecesLeftX
        {
            get => _gameState.PiecesLeftX;
            set => _gameState.PiecesLeftX = value;
        }

        /// <summary>
        /// Gets or sets the number of remaining pieces for player 'O'.
        /// </summary>
        public int PiecesLeftO
        {
            get => _gameState.PiecesLeftO;
            set => _gameState.PiecesLeftO = value;
        }

        /// <summary>
        /// Indicates if the game uses a grid.
        /// </summary>
        public bool UsesGrid { get; }
        
        /// <summary>
        /// Gets or sets the number of moves made by player 'X'.
        /// </summary>
        public int MovesMadeX { get; set; }
        
        /// <summary>
        /// Gets or sets the number of moves made by player 'O'.
        /// </summary>
        public int MovesMadeO { get; set; }

        /// <summary>
        /// Creates a deep copy of the current game board. This method copies the board's dimensions
        /// and its content to a new 2D array, ensuring that changes to the copy do not affect the
        /// original game board.
        /// </summary>
        /// <returns>
        /// A 2D array of type <see cref="EGamePiece"/> representing the current state of the game board.
        /// </returns>
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
        
        /// <summary>
        /// Checks if the current player has any remaining pieces to place.
        /// </summary>
        /// <returns>True if the current player has pieces left, otherwise false.</returns>
        public bool HasPiecesLeft()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? _gameState.PiecesLeftX > 0 : _gameState.PiecesLeftO > 0;
        }
        
        /// <summary>
        /// Checks if the current player can move a piece based on the number of moves made.
        /// </summary>
        /// <returns>True if the player can move a piece, otherwise false.</returns>
        public bool CanMovePiece()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX :_gameState. MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove; // Check if the required moves have been made.
        }
        
        /// <summary>
        /// Checks if the current player can move the grid based on the number of moves made.
        /// </summary>
        /// <returns>True if the player can move the grid, otherwise false.</returns>
        public bool CanMoveGrid()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MoveGridAfterNMove; // Check if the required moves have been made.
        }
        
        public bool PlaceNewPiece(int x, int y)
        {
            // Check if the chosen spot is already occupied
            if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
            {
                return false;
            }

            // Place the current player's piece on the board
            _gameState.GameBoard[x][y] = _gameState.CurrentPlayer;

            // Update the player's state after making the move
            UpdatePlayerStateAfterMove();

            // Switch to the next player
            SwitchPlayer();

            return true; // Move was successful
        }
        
        /// <summary>
        /// Handles moving an existing piece on the game board. The method validates the selected piece,
        /// checks if the destination is available, and moves the piece accordingly.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the result of moving the piece (successful, invalid move, etc.).
        /// </returns>
        public bool MoveExistingPiece(int oldX, int oldY, int newX, int newY)
        {
            
            if (_gameState.GameBoard[oldX][oldY] != _gameState.CurrentPlayer || _gameState.GameBoard[newX][newY] != EGamePiece.Empty)
            {
                return false;
            }
            
            // Move the piece to the new coordinates
            _gameState.GameBoard[newX][newY] = _gameState.CurrentPlayer;

            // Clear the old position of the piece
            _gameState.GameBoard[oldX][oldY] = EGamePiece.Empty;

            // Switch to the next player
            SwitchPlayer();

            return true;
        }
        
        /// <summary>
        /// Handles moving the grid to new coordinates as specified by the player.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the result of moving the grid (successful, invalid position, etc.).
        /// </returns>
        public bool MoveGrid(int newGridX, int newGridY)
        {
            // Check if the new grid position is valid
            if (!IsValidGridPosition(newGridX, newGridY))
            {
                return false;
            }

            // Update the grid's position
            GridPositionX = newGridX;
            GridPositionY = newGridY;

            // Switch to the next player
            SwitchPlayer();
            return true;
        }
        
        /// <summary>
        /// Checks if the given grid position is valid, ensuring that the grid remains within the bounds of the game board.
        /// </summary>
        /// <param name="x">The new x-coordinate of the grid.</param>
        /// <param name="y">The new y-coordinate of the grid.</param>
        /// <returns>
        /// <c>true</c> if the new grid position is valid; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidGridPosition(int x, int y)
        {
            // Check if the grid position is within the dimensions of the board
            return x >= 0 && x + GridSizeWidth <= DimensionX && y >= 0 && y + GridSizeHeight <= DimensionY;
        }
        
        /// <summary>
        /// Updates the state of the player who made the last move, adjusting the number of pieces left
        /// and the number of moves made by the current player.
        /// </summary>
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
        
        /// <summary>
        /// Switches the current player between X and O.
        /// </summary>
        public void SwitchPlayer()
        {
            // Toggle the current player between X and O
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        
        
        /// <summary>
        /// Checks if the given coordinates (x, y) are within the bounds of the game board.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>
        /// <c>true</c> if the coordinates are within the board's dimensions; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWithinBoard(int x, int y)
        {
            // Validate the coordinates against the board's dimensions
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }
        
        /// <summary>
        /// Checks if there is a winner based on the current state of the game board or grid.
        /// If no winner is found, <c>null</c> is returned.
        /// </summary>
        /// <returns>
        /// An <see cref="EGamePiece"/> representing the winning player (X or O), or <c>null</c> if no winner.
        /// </returns>
        public EGamePiece? CheckWin()
        {
            // Check winning condition based on whether the grid is used
            if (GridSizeWidth == 0 || GridSizeHeight == 0)
            {
                return CheckWinOnWholeBoard(); // Check for win on the entire board
            }

            return CheckWinWithinGrid(); // Check for win within the specified grid
        }
        
        /// <summary>
        /// Checks if there is a winner on the entire game board.
        /// </summary>
        /// <returns>
        /// An <see cref="EGamePiece"/> representing the winner (X or O), or <c>null</c> if no winner.
        /// </returns>
        private EGamePiece? CheckWinOnWholeBoard()
        {
            return CheckAllLines(_gameState.GameBoard, 0, 0, DimensionX, DimensionY); // Check all lines in the whole board
        }
        
        /// <summary>
        /// Checks if there is a winner within the current grid boundaries.
        /// </summary>
        /// <returns>
        /// An <see cref="EGamePiece"/> representing the winner (X or O), or <c>null</c> if no winner.
        /// </returns>
        private EGamePiece? CheckWinWithinGrid()
        {
            return CheckAllLines(_gameState.GameBoard, GridPositionX, GridPositionY, GridSizeWidth,
                GridSizeHeight); // Check lines within the grid
        }
        
        /// <summary>
        /// Checks all lines (rows, columns, and diagonals) within a specific area of the board to find a winner.
        /// </summary>
        /// <param name="board">The game board to check.</param>
        /// <param name="startX">The starting x-coordinate for the check.</param>
        /// <param name="startY">The starting y-coordinate for the check.</param>
        /// <param name="width">The width of the area to check.</param>
        /// <param name="height">The height of the area to check.</param>
        /// <returns>
        /// An <see cref="EGamePiece"/> representing the winner (X or O), or <c>null</c> if no winner.
        /// </returns>
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
        
        /// <summary>
        /// Checks a single line (row, column, or diagonal) for consecutive pieces of the same player.
        /// </summary>
        /// <param name="board">The game board to check.</param>
        /// <param name="startX">The starting x-coordinate for the check.</param>
        /// <param name="startY">The starting y-coordinate for the check.</param>
        /// <param name="deltaX">The x-direction delta for each step (1 for right, -1 for left).</param>
        /// <param name="deltaY">The y-direction delta for each step (1 for down, -1 for up).</param>
        /// <param name="length">The number of cells in the line to check.</param>
        /// <returns>
        /// An <see cref="EGamePiece"/> representing the winner (X or O), or <c>null</c> if no winner.
        /// </returns>
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
        
        /// <summary>
        /// Checks if the game is a draw by evaluating whether the board is full or if neither player can move.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the game is a draw; otherwise, <c>false</c>.
        /// </returns>
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
        
        /// <summary>
        /// Checks if the game board is completely filled with pieces (no empty spots).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the board is full; otherwise, <c>false</c>.
        /// </returns>
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
        
        /// <summary>
        /// Determines if the specified player can move a piece based on their current game state (pieces left and moves made).
        /// </summary>
        /// <param name="player">The player to check (X or O).</param>
        /// <returns>
        /// <c>true</c> if the player is able to move a piece; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMovePieceForPlayer(EGamePiece player)
        {
            var movesMade = player == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO; // Get the moves made by the player
            var piecesLeft = player == EGamePiece.X ? _gameState.PiecesLeftX : _gameState.PiecesLeftO; // Get the pieces left for the player

            // Check if the player has made enough moves and has no pieces left
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove && piecesLeft <= 0;
        }

        public void SetGameStateJson(string state)
        {
            _gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(state);
        }
    }
}
