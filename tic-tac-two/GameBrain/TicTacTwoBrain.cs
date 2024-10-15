namespace GameBrain
{
    /// <summary>
    /// Represents the brain of the Tic-Tac-Two game, handling game logic,
    /// player turns, piece movements, and win/draw conditions.
    /// </summary>
    public class TicTacTwoBrain
    {
        private readonly EGamePiece[,] _gameBoard; // The game board represented as a 2D array of game pieces.
        private EGamePiece _currentPlayer; // The current player (X or O).

        private readonly GameConfiguration _gameConfiguration; // Configuration settings for the game.

        private int _piecesLeftX; // Remaining pieces for player X.
        private int _piecesLeftO; // Remaining pieces for player O.
        private int _movesMadeX; // Number of moves made by player X.
        private int _movesMadeO; // Number of moves made by player O.

        /// <summary>
        /// Initializes a new instance of the TicTacTwoBrain class with the specified game configuration.
        /// </summary>
        /// <param name="gameConfiguration">The configuration for the game.</param>
        public TicTacTwoBrain(GameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration;

            // Initialize the game board based on the configured size.
            _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
            _currentPlayer = EGamePiece.X; // X starts first.
            _piecesLeftX = _gameConfiguration.PiecesNumber; // Set initial pieces for player X.
            _piecesLeftO = _gameConfiguration.PiecesNumber; // Set initial pieces for player O.
            _movesMadeX = 0; // Initialize moves made by player X.
            _movesMadeO = 0; // Initialize moves made by player O.
            GridSizeWidth = _gameConfiguration.GridSizeWidth; // Set the grid width from configuration.
            GridSizeHeight = _gameConfiguration.GridSizeHeight; // Set the grid height from configuration.
            GridPositionX = _gameConfiguration.GridPositionX; // Set initial grid position X from configuration.
            GridPositionY = _gameConfiguration.GridPositionY; // Set initial grid position Y from configuration.
            UsesGrid = _gameConfiguration.UsesGrid; // Determine if the game uses a grid.
        }

        // Public properties to access game state information.
        public EGamePiece[,] GameBoard => GetBoardCopy(); // Returns a copy of the game board.
        public EGamePiece CurrentPlayer => _currentPlayer; // Returns the current player.

        public int GridSizeWidth { get; } // Width of the grid.
        public int GridSizeHeight { get; } // Height of the grid.

        public int GridPositionX { get; private set; } // Current X position of the grid.
        public int GridPositionY { get; private set; } // Current Y position of the grid.

        public int DimensionX => _gameBoard.GetLength(0); // Width of the game board.
        public int DimensionY => _gameBoard.GetLength(1); // Height of the game board.

        public int PiecesLeftX => _piecesLeftX; // Remaining pieces for player X.
        public int PiecesLeftO => _piecesLeftO; // Remaining pieces for player O.
        public bool UsesGrid { get; } // Indicates if the game uses a grid.

        /// <summary>
        /// Creates a copy of the game board.
        /// </summary>
        /// <returns>A copy of the current game board.</returns>
        private EGamePiece[,] GetBoardCopy()
        {
            var copy = new EGamePiece[DimensionX, DimensionY];
            Array.Copy(_gameBoard, copy, _gameBoard.Length);
            return copy;
        }

        /// <summary>
        /// Handles the player's move, allowing them to place a new piece, move an existing piece, or move the grid.
        /// </summary>
        /// <returns>True if the move was successfully made, otherwise false.</returns>
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

        /// <summary>
        /// Checks if the current player has any pieces left.
        /// </summary>
        /// <returns>True if the current player has pieces left, otherwise false.</returns>
        private bool HasPiecesLeft()
        {
            return _currentPlayer == EGamePiece.X ? _piecesLeftX > 0 : _piecesLeftO > 0;
        }

        /// <summary>
        /// Checks if the current player can move an existing piece.
        /// </summary>
        /// <returns>True if the player can move a piece, otherwise false.</returns>
        private bool CanMovePiece()
        {
            var movesMade = _currentPlayer == EGamePiece.X ? _movesMadeX : _movesMadeO;
            return movesMade >= _gameConfiguration.MovePieceAfterNMove; // Check if the required moves have been made.
        }

        /// <summary>
        /// Checks if the current player can move the grid.
        /// </summary>
        /// <returns>True if the player can move the grid, otherwise false.</returns>
        private bool CanMoveGrid()
        {
            var movesMade = _currentPlayer == EGamePiece.X ? _movesMadeX : _movesMadeO;
            return movesMade >= _gameConfiguration.MoveGridAfterNMove; // Check if the required moves have been made.
        }

        /// <summary>
        /// Handles the scenario when the player has no pieces left.
        /// </summary>
        /// <param name="canMovePiece">Indicates if the player can move a piece.</param>
        /// <param name="canMoveGrid">Indicates if the player can move the grid.</param>
        /// <returns>True if the turn is valid, otherwise false.</returns>
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

        /// <summary>
        /// Handles the player's choice between placing a new piece, moving an existing piece, or moving the grid.
        /// </summary>
        /// <param name="canMovePiece">Indicates if the player can move a piece.</param>
        /// <param name="canMoveGrid">Indicates if the player can move the grid.</param>
        /// <returns>True if a valid action was taken, otherwise false.</returns>
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

        /// <summary>
        /// Places a new piece on the game board at the specified coordinates.
        /// Validates the move to ensure the spot is not already occupied.
        /// Updates the game state and switches to the next player after a successful move.
        /// </summary>
        /// <returns>True if the piece was successfully placed; otherwise, false.</returns>
        private bool PlaceNewPiece()
        {
            // Get coordinates from the player where they want to place their piece
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");

            // Check if the chosen spot is already occupied
            if (_gameBoard[x, y] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Place the current player's piece on the board
            _gameBoard[x, y] = _currentPlayer;

            // Update the player's state after making the move
            UpdatePlayerStateAfterMove();

            // Switch to the next player
            SwitchPlayer();

            return true; // Move was successful
        }

        /// <summary>
        /// Moves an existing piece on the game board from its current position to a new position.
        /// Validates the move to ensure the selected piece belongs to the current player 
        /// and that the destination spot is empty before completing the move.
        /// </summary>
        /// <returns>True if the piece was successfully moved; otherwise, false.</returns>
        private bool MoveExistingPiece()
        {
            // Get the coordinates of the piece the player wants to move
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");

            // Check if the selected piece belongs to the current player
            if (_gameBoard[oldX, oldY] != _currentPlayer)
            {
                Console.WriteLine("Invalid selection. That is not your piece.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Get the new coordinates where the player wants to move the piece
            var (newX, newY) =
                GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");

            // Check if the destination spot is already occupied
            if (_gameBoard[newX, newY] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue(); // Pause to allow the player to read the message
                return false; // Move was not successful
            }

            // Move the piece to the new coordinates
            _gameBoard[newX, newY] = _currentPlayer;

            // Clear the old position of the piece
            _gameBoard[oldX, oldY] = EGamePiece.Empty;

            // Switch to the next player
            SwitchPlayer();

            return true; // Move was successful
        }

        /// <summary>
        /// Moves the grid to new coordinates specified by the player.
        /// Validates the new position to ensure it is within the game board limits.
        /// </summary>
        /// <returns>True if the grid was successfully moved; otherwise, false.</returns>
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

        /// <summary>
        /// Validates whether the given grid position is within the board's dimensions.
        /// </summary>
        /// <param name="x">The x-coordinate of the grid.</param>
        /// <param name="y">The y-coordinate of the grid.</param>
        /// <returns>True if the grid position is valid; otherwise, false.</returns>
        private bool IsValidGridPosition(int x, int y)
        {
            // Check if the grid position is within the dimensions of the board
            return x >= 0 && x + GridSizeWidth <= DimensionX && y >= 0 && y + GridSizeHeight <= DimensionY;
        }

        /// <summary>
        /// Updates the state of the current player after making a move.
        /// Decrements the number of pieces left and increments the number of moves made for the current player.
        /// </summary>
        private void UpdatePlayerStateAfterMove()
        {
            // Update player state based on which piece is currently active
            if (_currentPlayer == EGamePiece.X)
            {
                _piecesLeftX--; // Decrement pieces left for player X
                _movesMadeX++; // Increment moves made for player X
            }
            else
            {
                _piecesLeftO--; // Decrement pieces left for player O
                _movesMadeO++; // Increment moves made for player O
            }
        }


        /// <summary>
        /// Switches the current player from X to O or from O to X.
        /// </summary>
        private void SwitchPlayer()
        {
            // Toggle the current player between X and O
            _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }

        /// <summary>
        /// Prompts the player for coordinates and validates the input.
        /// Repeats the prompt until valid coordinates within the board limits are provided.
        /// </summary>
        /// <param name="prompt">The message to display when asking for coordinates.</param>
        /// <returns>A tuple containing the validated x and y coordinates.</returns>
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
        /// Attempts to parse the input string into x and y coordinates.
        /// </summary>
        /// <param name="input">The input string containing coordinates.</param>
        /// <param name="x">Output parameter for the parsed x coordinate.</param>
        /// <param name="y">Output parameter for the parsed y coordinate.</param>
        /// <returns>True if parsing was successful; otherwise, false.</returns>
        private bool TryParseCoordinates(string? input, out int x, out int y)
        {
            x = y = -1; // Initialize output coordinates
            var parts = input?.Split(','); // Split input by comma
            return parts != null && parts.Length == 2 // Check if there are two parts
                                 && int.TryParse(parts[0], out x) // Try parsing the first part as x
                                 && int.TryParse(parts[1], out y); // Try parsing the second part as y
        }

        /// <summary>
        /// Checks if the given coordinates are within the dimensions of the game board.
        /// </summary>
        /// <param name="x">The x-coordinate to check.</param>
        /// <param name="y">The y-coordinate to check.</param>
        /// <returns>True if the coordinates are within the board; otherwise, false.</returns>
        private bool IsWithinBoard(int x, int y)
        {
            // Validate the coordinates against the board's dimensions
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }

        /// <summary>
        /// Pauses the console application and waits for the user to press any key to continue.
        /// Clears the console afterwards.
        /// </summary>
        private void PauseBeforeContinue()
        {
            Console.WriteLine("Press any key to continue..."); // Prompt the user
            Console.ReadKey(); // Wait for a key press
            Console.Clear(); // Clear the console
        }

        /// <summary>
        /// Prompts the player to choose whether to move an existing piece or the grid.
        /// Repeats the prompt until a valid response is provided.
        /// </summary>
        /// <returns>True if the move was successful; otherwise, false.</returns>
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

        /// <summary>
        /// Checks for a winning condition in the game.
        /// Determines if the game is won on the whole board or within the grid.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
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
        /// Checks for a winning condition across the entire game board.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        private EGamePiece? CheckWinOnWholeBoard()
        {
            return CheckAllLines(_gameBoard, 0, 0, DimensionX, DimensionY); // Check all lines in the whole board
        }

        /// <summary>
        /// Checks for a winning condition within the specified grid.
        /// </summary>
        /// <returns>The winning player, or null if there is no winner.</returns>
        private EGamePiece? CheckWinWithinGrid()
        {
            return CheckAllLines(_gameBoard, GridPositionX, GridPositionY, GridSizeWidth,
                GridSizeHeight); // Check lines within the grid
        }


        /// <summary>
        /// Checks all lines (rows, columns, and diagonals) on the board for a winning condition.
        /// </summary>
        /// <param name="board">The game board represented as a 2D array.</param>
        /// <param name="startX">The starting X-coordinate for the check.</param>
        /// <param name="startY">The starting Y-coordinate for the check.</param>
        /// <param name="width">The width of the section to check.</param>
        /// <param name="height">The height of the section to check.</param>
        /// <returns>The winning player if a win is found; otherwise, null.</returns>
        private EGamePiece? CheckAllLines(EGamePiece[,] board, int startX, int startY, int width, int height)
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
            var diagWinner =
                CheckLine(board, startX, startY, 1, 1, Math.Min(width, height)); // Top-left to bottom-right diagonal
            if (diagWinner != null) return diagWinner; // Return the winner if found

            diagWinner =
                CheckLine(board, startX + width - 1, startY, -1, 1,
                    Math.Min(width, height)); // Top-right to bottom-left diagonal
            if (diagWinner != null) return diagWinner; // Return the winner if found

            return null; // No winner found
        }

        /// <summary>
        /// Checks a single line for a winning condition.
        /// </summary>
        /// <param name="board">The game board represented as a 2D array.</param>
        /// <param name="startX">The starting X-coordinate for the line check.</param>
        /// <param name="startY">The starting Y-coordinate for the line check.</param>
        /// <param name="deltaX">The change in X for each step in the line.</param>
        /// <param name="deltaY">The change in Y for each step in the line.</param>
        /// <param name="length">The length of the line to check.</param>
        /// <returns>The winning player if a win is found; otherwise, null.</returns>
        private EGamePiece? CheckLine(EGamePiece[,] board, int startX, int startY, int deltaX, int deltaY, int length)
        {
            int count = 0; // Count of consecutive pieces
            EGamePiece currentPiece = EGamePiece.Empty; // Currently checked piece

            for (int i = 0; i < length; i++)
            {
                int x = startX + i * deltaX; // Calculate current X
                int y = startY + i * deltaY; // Calculate current Y

                // Break if coordinates are out of board bounds
                if (x >= DimensionX || y >= DimensionY || x < 0 || y < 0)
                    break;

                var piece = board[x, y]; // Get the piece at the current position
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
                if (count >= _gameConfiguration.WinCondition)
                    return currentPiece; // Return the current piece as the winner
            }

            return null; // No winner found
        }

        /// <summary>
        /// Checks if the game has ended in a draw.
        /// </summary>
        /// <returns>True if the game is a draw; otherwise, false.</returns>
        public bool CheckDraw()
        {
            if (IsBoardFull()) // Check if the board is full
            {
                return true; // It's a draw
            }

            // Determine if either player can make a move
            var canMoveX = _piecesLeftX > 0 || CanMovePieceForPlayer(EGamePiece.X);
            var canMoveO = _piecesLeftO > 0 || CanMovePieceForPlayer(EGamePiece.O);

            // It's a draw if neither player can make a move
            return !canMoveX && !canMoveO;
        }

        /// <summary>
        /// Checks if the game board is completely filled with pieces.
        /// </summary>
        /// <returns>True if the board is full; otherwise, false.</returns>
        private bool IsBoardFull()
        {
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (_gameBoard[x, y] == EGamePiece.Empty) // Check for any empty spot
                    {
                        return false; // The board is not full
                    }
                }
            }

            return true; // The board is full
        }

        /// <summary>
        /// Determines if a specified player can move a piece based on the game rules.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>True if the player can move a piece; otherwise, false.</returns>
        private bool CanMovePieceForPlayer(EGamePiece player)
        {
            var movesMade = player == EGamePiece.X ? _movesMadeX : _movesMadeO; // Get the moves made by the player
            var piecesLeft = player == EGamePiece.X ? _piecesLeftX : _piecesLeftO; // Get the pieces left for the player

            // Check if the player has made enough moves and has no pieces left
            return movesMade >= _gameConfiguration.MovePieceAfterNMove && piecesLeft <= 0;
        }
    }
}
