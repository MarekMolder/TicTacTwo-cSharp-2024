using Domain;

namespace GameBrain
{
    public class TicTacTwoBrain
    {
        private GameState _gameState;
        
        public TicTacTwoBrain(GameConfiguration gameConfiguration, string playerX = "Player-X", string playerO = "Player-0",int gridPositionY = -1, int gridPositionX = -1, int movesMadeX = 0, int movesMadeO = 0)
        {
            var gameBoard = new EGamePiece[gameConfiguration.BoardSizeWidth][];
            for (var i = 0; i < gameBoard.Length; i++)
            {
                gameBoard[i] = new EGamePiece[gameConfiguration.BoardSizeHeight];
            }
            
            int finalGridPositionX = gridPositionX == -1 ? gameConfiguration.GridPositionX : gridPositionX;
            int finalGridPositionY = gridPositionY == -1 ? gameConfiguration.GridPositionY : gridPositionY;
            
            int gridSizeWidth = gridPositionX == -1 ? gameConfiguration.GridPositionX : gridPositionX;
            int gridSizeHeight = gridPositionY == -1 ? gameConfiguration.GridPositionY : gridPositionY;
            int usesGrid = gridPositionY == -1 ? gameConfiguration.GridPositionY : gridPositionY;
            
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
        
        public bool IsCellInGrid(int x, int y)
        {
            if (!_gameState.GameConfiguration.UsesGrid)
            {
                return false;
            }

            var GridStartX = _gameState.GridPositionX.ToString();
            var GridStartY = _gameState.GridPositionY.ToString();

            int gridStartX = int.TryParse(GridStartX, out var startX) ? startX : -1;
            int gridStartY = int.TryParse(GridStartY, out var startY) ? startY : -1;

            int gridEndX = gridStartX + _gameState.GameConfiguration.GridSizeWidth - 1;
            int gridEndY = gridStartY + _gameState.GameConfiguration.GridSizeHeight - 1;

            return x >= gridStartX && x <= gridEndX && y >= gridStartY && y <= gridEndY;
        }
        
        public GameConfiguration GetGameConfig()
        {
            return _gameState.GameConfiguration;
        }
        
        public EGamePiece[][] GameBoard => GetBoardCopy(); // Returns a copy of the game board.
        
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
        
        public EGamePiece CurrentPlayer
        {
            get => _gameState.CurrentPlayer;
            set => _gameState.CurrentPlayer = value;
        }
        
        public int GridSizeWidth { get; }
        
        public int GridSizeHeight { get; }
        
        public int GridPositionX
        {
            get => _gameState.GridPositionX;
            set => _gameState.GridPositionX = value;
        }
        
        public int GridPositionY
        {
            get => _gameState.GridPositionY;
            set => _gameState.GridPositionY = value;
        }
        
        public int DimensionX => _gameState.GameBoard.Length; 
        
        public int DimensionY => _gameState.GameBoard[0].Length;
        
        public int PiecesLeftX
        {
            get => _gameState.PiecesLeftX;
            set => _gameState.PiecesLeftX = value;
        }
        
        public int PiecesLeftO
        {
            get => _gameState.PiecesLeftO;
            set => _gameState.PiecesLeftO = value;
        }
        
        public bool UsesGrid { get; }
        
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
        
        public bool HasPiecesLeft()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? _gameState.PiecesLeftX > 0 : _gameState.PiecesLeftO > 0;
        }
        
        public bool CanMovePiece()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX :_gameState. MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove; // Check if the required moves have been made.
        }
        
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
        
        public void SwitchPlayer()
        {
            // Toggle the current player between X and O
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        
        public bool IsWithinBoard(int x, int y)
        {
            // Validate the coordinates against the board's dimensions
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
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

        public void SetGameStateJson(string state)
        {
            _gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(state);
        }

        public void AiMove()
        {
            // 1. Try to block the opponent from winning
            var blockingMove = FindBlockingMove();
            if (blockingMove != null)
            {
                // Block the move by placing the piece at the blocking position
                PlaceNewPiece(blockingMove.Value.x, blockingMove.Value.y);
                return;  // Return after blocking the move
            }

            // 2. If no blocking move, place the piece in a random empty spot or within the grid if it's enabled
            GetRandomMoveWithinGrid();
        }

        private (int x, int y)? FindBlockingMove()
        {
            // Define the grid boundaries, based on the current grid position and size
            int startX = UsesGrid ? GridPositionX : 0;
            int endX = UsesGrid ? GridPositionX + GridSizeWidth : DimensionX;
            int startY = UsesGrid ? GridPositionY : 0;
            int endY = UsesGrid ? GridPositionY + GridSizeHeight : DimensionY;

            // Try to block the opponent's winning move within the grid area (if grid is used)
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (_gameState.GameBoard[x][y] == EGamePiece.Empty)
                    {
                        // Temporarily place opponent's piece to see if it forms a winning line
                        _gameState.GameBoard[x][y] = GetOpponentPlayer();

                        // Check if placing the opponent's piece would lead to a win
                        if (CheckWinOnWholeBoard() == GetOpponentPlayer())
                        {
                            // If the opponent would win, return this position to block
                            _gameState.GameBoard[x][y] = EGamePiece.Empty; // Revert the move
                            return (x, y); // Return the blocking position
                        }

                        // Revert the move
                        _gameState.GameBoard[x][y] = EGamePiece.Empty;
                    }
                }
            }

            // No blocking move found within the grid boundaries
            return null;
        }

        private EGamePiece GetOpponentPlayer()
        {
            // If the current player is X, the opponent is O, and vice versa
            return _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }

        private void GetRandomMoveWithinGrid()
        {
            // If the grid is enabled, we prioritize the grid area
            if (UsesGrid)
            {
                for (int x = GridPositionX; x < GridPositionX + GridSizeWidth; x++)
                {
                    for (int y = GridPositionY; y < GridPositionY + GridSizeHeight; y++)
                    {
                        if (x < DimensionX && y < DimensionY && _gameState.GameBoard[x][y] == EGamePiece.Empty)
                        {
                            PlaceNewPiece(x, y);
                            return;
                        }
                    }
                }
            }

            // If no grid or grid is full, select a random empty spot on the entire board
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (_gameState.GameBoard[x][y] == EGamePiece.Empty)
                    {
                        PlaceNewPiece(x, y);
                        return;
                    }
                }
            }

            // If no empty spots available (shouldn't happen if the game logic is correct)
            throw new Exception("No valid moves available.");
        }

        
        
    }
    
}
