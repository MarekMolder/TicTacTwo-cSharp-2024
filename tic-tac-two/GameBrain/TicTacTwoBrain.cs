namespace GameBrain
{
    /// <summary>
    /// The <see cref="TicTacTwoBrain"/> class represents the core game logic for a Tic-Tac-Toe game
    /// with additional features like grid manipulation and piece movements. It manages the game state,
    /// player turns, and actions like moving pieces or grids.
    /// </summary>
    public class TicTacTwoBrain
    {
        private readonly GameState _gameState;
        
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
        /// Makes a move for the current player and returns the result of the move.
        /// </summary>
        /// <returns>The result of the move.</returns>
        public MoveResult MakeAMove()
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
            return SaveOrContinue();
        }
        
        /// <summary>
        /// Checks if the current player has any remaining pieces to place.
        /// </summary>
        /// <returns>True if the current player has pieces left, otherwise false.</returns>
        private bool HasPiecesLeft()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? _gameState.PiecesLeftX > 0 : _gameState.PiecesLeftO > 0;
        }
        
        /// <summary>
        /// Checks if the current player can move a piece based on the number of moves made.
        /// </summary>
        /// <returns>True if the player can move a piece, otherwise false.</returns>
        private bool CanMovePiece()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX :_gameState. MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove; // Check if the required moves have been made.
        }
        
        /// <summary>
        /// Checks if the current player can move the grid based on the number of moves made.
        /// </summary>
        /// <returns>True if the player can move the grid, otherwise false.</returns>
        private bool CanMoveGrid()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MoveGridAfterNMove; // Check if the required moves have been made.
        }
        
        /// <summary>
        /// Handles the scenario where the current player has no pieces left. Based on what movements
        /// are available (moving a piece, moving the grid, or saving the game), prompts the player accordingly.
        /// </summary>
        /// <param name="canMovePiece">Indicates if the player can move an existing piece.</param>
        /// <param name="canMoveGrid">Indicates if the player can move the grid.</param>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the next step to take (continue, save game, etc.).
        /// </returns>
        private MoveResult HandleNoPiecesLeft(bool canMovePiece, bool canMoveGrid)
        {
            // If both movements are possible, prompt the player.
            if (canMovePiece && canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece, move the grid or save the game.");
                return PromptMovePieceOrGrid();
            }

            // If only moving a piece is possible, move it.
            if (canMovePiece)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece or save the game.");
                return MoveExistingPiece();
            }

            // If only moving the grid is possible, move it.
            if (canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move the grid or save the game.");
                return MoveGrid();
            }

            // If no valid moves, skip the player's turn.
            Console.WriteLine("You have no valid moves. Your turn is skipped.");
            SwitchPlayer();
            return MoveResult.Continue;
        }
        
        /// <summary>
        /// Handles the player's choice of what action to take when they can move a piece or grid,
        /// or place a new piece. The method prompts the player for their choice and executes the chosen action.
        /// </summary>
        /// <param name="canMovePiece">Indicates if the player can move an existing piece.</param>
        /// <param name="canMoveGrid">Indicates if the player can move the grid.</param>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the result of the player's action (place new piece, move existing piece, move grid, or save game).
        /// </returns>
        private MoveResult HandlePlayerChoice(bool canMovePiece, bool canMoveGrid)
        {
            while (true)
            {
                // Prompt the player for their choice.
                Console.WriteLine(
                    $"Do you want to place a new piece{(canMovePiece ? ", move an existing piece" : "")}{(canMoveGrid ? ",  move the grid" : "")}, or save the game?");

                var options = new List<string> { "new" }; // Default option.
                if (canMovePiece) options.Add("old"); // Add option to move an existing piece.
                if (canMoveGrid) options.Add("grid"); // Add option to move the grid.
                options.Add("save"); // Add option to save the game

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
                
                if (response == "save")
                {
                    return MoveResult.SaveGame;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }
        
        /// <summary>
        /// Prompts the player for coordinates to place a new piece on the game board, validates
        /// the placement, and updates the game state accordingly.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating whether the move was successful, invalid, or the game was saved.
        /// </returns>
        private MoveResult PlaceNewPiece()
        {
            // Get coordinates from the player where they want to place their piece
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");

            // Check if the chosen spot is already occupied
            if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return MoveResult.InvalidMove; // Move was not successful
            }

            // Place the current player's piece on the board
            _gameState.GameBoard[x][y] = _gameState.CurrentPlayer;

            // Update the player's state after making the move
            UpdatePlayerStateAfterMove();

            // Switch to the next player
            SwitchPlayer();

            return MoveResult.Continue; // Move was successful
        }
        
        /// <summary>
        /// Asks the player if they want to place a new piece or save the game when no pieces are left.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating whether the player opted to place a new piece or save the game.
        /// </returns>
        private MoveResult SaveOrContinue()
        {
            while (true)
            {
                Console.WriteLine("Do you want to place a new piece, or save the game?");
                var options = new List<string> { "new" }; // Default option.
                options.Add("save"); // Add option to move an existing piece.

                Console.WriteLine($"Options: {string.Join("/", options)}");
                var response = Console.ReadLine()?.Trim().ToLower();

                // Handle the player's choice based on their input.
                if (response == "new")
                {
                    return PlaceNewPiece();
                }

                if (response == "save")
                {
                    return MoveResult.SaveGame;
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }
        
        /// <summary>
        /// Handles moving an existing piece on the game board. The method validates the selected piece,
        /// checks if the destination is available, and moves the piece accordingly.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the result of moving the piece (successful, invalid move, etc.).
        /// </returns>
        private MoveResult MoveExistingPiece()
        {
            // Get the coordinates of the piece the player wants to move
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");

            // Check if the selected piece belongs to the current player
            if (_gameState.GameBoard[oldX][oldY] != _gameState.CurrentPlayer)
            {
                Console.WriteLine("Invalid selection. That is not your piece.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return MoveResult.InvalidMove; // Move was not successful
            }

            // Get the new coordinates where the player wants to move the piece
            var (newX, newY) =
                GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

            // Check if the destination spot is already occupied
            if (_gameState.GameBoard[newX][newY] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return MoveResult.InvalidMove; // Move was not successful
            }

            // Move the piece to the new coordinates
            _gameState.GameBoard[newX][newY] = _gameState.CurrentPlayer;

            // Clear the old position of the piece
            _gameState.GameBoard[oldX][oldY] = EGamePiece.Empty;

            // Switch to the next player
            SwitchPlayer();

            return MoveResult.Continue; // Move was successful
        }
        
        /// <summary>
        /// Handles moving the grid to new coordinates as specified by the player.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the result of moving the grid (successful, invalid position, etc.).
        /// </returns>
        private MoveResult MoveGrid()
        {
            // Get new coordinates for the grid from the player
            var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");

            // Check if the new grid position is valid
            if (!IsValidGridPosition(newGridX, newGridY))
            {
                Console.WriteLine("Invalid grid position. Please try again.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return MoveResult.InvalidMove;; // Move was not successful
            }

            // Update the grid's position
            GridPositionX = newGridX;
            GridPositionY = newGridY;

            // Switch to the next player
            SwitchPlayer();
            return MoveResult.Continue; // Move was successful
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
        private void SwitchPlayer()
        {
            // Toggle the current player between X and O
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        
        /// <summary>
        /// Prompts the player to enter coordinates for a move and validates them.
        /// Continues prompting the user until valid coordinates are entered.
        /// </summary>
        /// <param name="prompt">The prompt message displayed to the player.</param>
        /// <returns>
        /// A tuple representing the coordinates (x, y) entered by the player.
        /// </returns>
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
        
        /// <summary>
        /// Attempts to parse the player's input into two integer coordinates (x, y).
        /// </summary>
        /// <param name="input">The raw input string from the player.</param>
        /// <param name="x">The parsed x-coordinate.</param>
        /// <param name="y">The parsed y-coordinate.</param>
        /// <returns>
        /// <c>true</c> if the input can be parsed successfully into two integers; otherwise, <c>false</c>.
        /// </returns>
        private bool TryParseCoordinates(string? input, out int x, out int y)
        {
            x = y = -1; // Initialize output coordinates
            var parts = input?.Split(','); // Split input by comma
            return parts != null && parts.Length == 2 // Check if there are two parts
                                 && int.TryParse(parts[0], out x) // Try parsing the first part as x
                                 && int.TryParse(parts[1], out y); // Try parsing the second part as y
        }
        
        /// <summary>
        /// Checks if the given coordinates (x, y) are within the bounds of the game board.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>
        /// <c>true</c> if the coordinates are within the board's dimensions; otherwise, <c>false</c>.
        /// </returns>
        private bool IsWithinBoard(int x, int y)
        {
            // Validate the coordinates against the board's dimensions
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }
        
        /// <summary>
        /// Pauses the game and waits for the player to press any key to continue.
        /// Clears the console after a key is pressed.
        /// </summary>
        private void PauseBeforeContinue()
        {
            Console.WriteLine("Press any key to continue..."); // Prompt the user
            Console.ReadKey(); // Wait for a key press
            Console.Clear(); // Clear the console
        }
        
        /// <summary>
        /// Prompts the player to decide whether to move an existing piece, move the grid, or save the game.
        /// </summary>
        /// <returns>
        /// A <see cref="MoveResult"/> indicating the next action to take (move piece, move grid, save game).
        /// </returns>
        private MoveResult PromptMovePieceOrGrid()
        {
            while (true)
            {
                Console.WriteLine(
                    "Do you want to move an existing piece (old), move the grid (grid) or save the game (save)? Options: old/grid/save");
                var response = Console.ReadLine()?.Trim().ToLower(); // Read and normalize user input

                if (response == "old")
                {
                    return MoveExistingPiece(); // Move an existing piece
                }

                if (response == "grid")
                {
                    return MoveGrid(); // Move the grid
                }

                if (response == "save")
                {
                    return MoveResult.SaveGame;
                }

                Console.WriteLine("Invalid input. Please try again."); // Error message for invalid input
            }
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
        
    }
}
