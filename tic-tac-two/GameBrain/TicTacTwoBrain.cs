using Domain;

namespace GameBrain
{
    public class TicTacTwoBrain
    {
        private GameState _gameState;
        
        /// <summary>
        /// Initializes the game state, game board, and grid position based on the configuration and provided parameters.
        /// </summary>
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
            
            _gameState = new GameState(
                gameBoard,
                gameConfiguration,
                EGamePiece.X,
                gameConfiguration.PiecesNumber,
                gameConfiguration.PiecesNumber,
                movesMadeX,
                movesMadeO,
                playerX,
                playerO,
                finalGridPositionX,
                finalGridPositionY
            );
            
            GridPositionX = finalGridPositionX;
            GridPositionY = finalGridPositionY;
            
            GridSizeWidth = _gameState.GameConfiguration.GridSizeWidth;
            GridSizeHeight = _gameState.GameConfiguration.GridSizeHeight;
            UsesGrid = _gameState.GameConfiguration.UsesGrid;
        }
        
        /// <summary>
        /// Gets a copy of the current game board.
        /// </summary>
        public EGamePiece[][] GameBoard => GetBoardCopy();
        
        /// <summary>
        /// Creates a copy of the current game board to ensure immutability.
        /// </summary>
        private EGamePiece[][] GetBoardCopy()
        {
            var copy = new EGamePiece[DimensionX][];
            
            for (int i = 0; i < DimensionX; i++)
            {
                copy[i] = new EGamePiece[DimensionY];
                Array.Copy(_gameState.GameBoard[i], copy[i], DimensionY);
            }

            return copy;
        }
        
        /// <summary>
        /// Sets the game board to the provided board, ensuring dimensions match the configuration.
        /// </summary>
        public void SetGameBoard(EGamePiece[][] gameBoard)
        {
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
                        
                        if (IsWithinBoard(targetX, targetY))
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
        /// Returns the game state as a JSON string.
        /// </summary>
        public string GetGameStateJson()
        {
            return _gameState.ToString();
        }
        
        /// <summary>
        /// Retrieves the game configuration.
        /// </summary>
        public GameConfiguration GetGameConfig()
        {
            return _gameState.GameConfiguration;
        }
        
        /// <summary>
        /// Gets or sets the current player.
        /// </summary>
        public EGamePiece CurrentPlayer
        {
            get => _gameState.CurrentPlayer;
            set => _gameState.CurrentPlayer = value;
        }
        
        /// <summary>
        /// Returns the width of the grid.
        /// </summary>
        public int GridSizeWidth { get; }
        
        /// <summary>
        /// Returns the height of the grid.
        /// </summary>
        public int GridSizeHeight { get; }
        
        /// <summary>
        /// Updates the grid position X within the game state.
        /// </summary>
        public int GridPositionX
        {
            get => _gameState.GridPositionX;
            set => _gameState.GridPositionX = value;
        }
        
        /// <summary>
        /// Updates the grid position Y within the game state.
        /// </summary>
        public int GridPositionY
        {
            get => _gameState.GridPositionY;
            set => _gameState.GridPositionY = value;
        }
        
        /// <summary>
        /// Returns the X dimension (width) of the game board.
        /// </summary>
        public int DimensionX => _gameState.GameBoard.Length; 
        
        /// <summary>
        /// Returns the Y dimension (height) of the game board.
        /// </summary>
        public int DimensionY => _gameState.GameBoard[0].Length;
        
        /// <summary>
        /// Gets or sets the Player X pieces number.
        /// </summary>
        public int PiecesLeftX
        {
            get => _gameState.PiecesLeftX;
            set => _gameState.PiecesLeftX = value;
        }
        
        /// <summary>
        /// Gets or sets the Player O pieces number.
        /// </summary>
        public int PiecesLeftO
        {
            get => _gameState.PiecesLeftO;
            set => _gameState.PiecesLeftO = value;
        }
        
        /// <summary>
        /// Indicates whether the game uses a grid system for gameplay.
        /// </summary>
        public bool UsesGrid { get; }
        
        /// <summary>
        /// Tracks the number of moves made by Player X.
        /// </summary>
        public int MovesMadeX { get; set; }
        
        /// <summary>
        /// Tracks the number of moves made by Player O.
        /// </summary>
        public int MovesMadeO { get; set; }
        
        /// <summary>
        /// Checks if there are pieces left for the current player.
        /// </summary>
        public bool HasPiecesLeft()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? _gameState.PiecesLeftX > 0 : _gameState.PiecesLeftO > 0;
        }
        
        /// <summary>
        /// Checks whether the player can move a piece based on the number of moves made.
        /// </summary>
        public bool CanMovePiece()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX :_gameState. MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove;
        }
        
        /// <summary>
        /// Checks whether the player can move a grid based on the number of moves made.
        /// </summary>
        public bool CanMoveGrid()
        {
            var movesMade = _gameState.CurrentPlayer == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO;
            return movesMade >= _gameState.GameConfiguration.MoveGridAfterNMove;
        }
        
        /// <summary>
        /// Places a new game piece at the given coordinates.
        /// </summary>
        public bool PlaceNewPiece(int x, int y)
        {
            if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
            {
                return false;
            }
            
            _gameState.GameBoard[x][y] = _gameState.CurrentPlayer;
            
            UpdatePlayerStateAfterMove();
            
            SwitchPlayer();

            return true;
        }
        
        /// <summary>
        /// Moves an existing piece from one position to another.
        /// </summary>
        public bool MoveExistingPiece(int oldX, int oldY, int newX, int newY)
        {
            
            if (_gameState.GameBoard[oldX][oldY] != _gameState.CurrentPlayer || _gameState.GameBoard[newX][newY] != EGamePiece.Empty)
            {
                return false;
            }
            
            _gameState.GameBoard[newX][newY] = _gameState.CurrentPlayer;
            
            _gameState.GameBoard[oldX][oldY] = EGamePiece.Empty;

            SwitchPlayer();

            return true;
        }
        
        /// <summary>
        /// Moves the game grid to a new position.
        /// </summary>
        public bool MoveGrid(int newGridX, int newGridY)
        {
            if (!IsValidGridPosition(newGridX, newGridY, GridPositionX, GridPositionY))
            {
                return false;
            }

            GridPositionX = newGridX;
            GridPositionY = newGridY;
            
            SwitchPlayer();
            return true;
        }
        
        /// <summary>
        /// Checks if inserted coordinates are valid to position grid.
        /// </summary>
        private bool IsValidGridPosition(int x, int y, int oldGridX, int oldGridY)
        {
            bool isWithinBoard = x >= 0 && x + GridSizeWidth <= DimensionX && y >= 0 && y + GridSizeHeight <= DimensionY;
            bool isOneUnitMove = Math.Abs(x - oldGridX) <= 1 && Math.Abs(y - oldGridY) <= 1;
            
            return isWithinBoard && isOneUnitMove;
        }
        
        /// <summary>
        /// Updates the current player's state after a move. 
        /// Reduces the remaining pieces for the current player and increments their move count.
        /// </summary>
        private void UpdatePlayerStateAfterMove()
        {
            if (_gameState.CurrentPlayer == EGamePiece.X)
            {
                _gameState.PiecesLeftX--; 
                _gameState.MovesMadeX++;
            }
            else
            {
                _gameState.PiecesLeftO--;
                _gameState.MovesMadeO++;
            }
        }

        /// <summary>
        /// Switches the current player to the next player.
        /// </summary>
        private void SwitchPlayer()
        {
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
        
        /// <summary>
        /// Checks whether the given coordinates (x, y) are within the bounds of the game board.
        /// </summary>
        public bool IsWithinBoard(int x, int y)
        {
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }
        
        /// <summary>
        /// Checks if a win condition is met on the game board.
        /// Determines whether to check the entire board or within a specific grid.
        /// </summary>
        public EGamePiece? CheckWin()
        {
            if (GridSizeWidth == 0 || GridSizeHeight == 0)
            {
                return CheckWinOnWholeBoard();
            }

            return CheckWinWithinGrid();
        }
        
        /// <summary>
        /// Checks if a win condition is met on the entire game board.
        /// </summary>
        private EGamePiece? CheckWinOnWholeBoard()
        {
            return CheckAllLines(_gameState.GameBoard, 0, 0, DimensionX, DimensionY);
        }
        
        /// <summary>
        /// Checks if a win condition is met within a specific grid area of the board.
        /// </summary>
        private EGamePiece? CheckWinWithinGrid()
        {
            return CheckAllLines(_gameState.GameBoard, GridPositionX, GridPositionY, GridSizeWidth,
                GridSizeHeight); 
        }
        
        /// <summary>
        /// Checks all rows, columns, and diagonals within a specified area of the board for a win condition.
        /// </summary>
        private EGamePiece? CheckAllLines(EGamePiece[][] board, int startX, int startY, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                var winner = CheckLine(board, startX + i, startY, 0, 1, height);
                if (winner != null) return winner;

                winner = CheckLine(board, startX, startY + i, 1, 0, width);
                if (winner != null) return winner;
            }
            
            for (int i = 0; i < width; i++)
            {
                var diagWinner1 = CheckLine(board, startX + i, startY, 1, 1, Math.Min(width - i, height));
                if (diagWinner1 != null) return diagWinner1;

                var diagWinner2 = CheckLine(board, startX + i, startY + height - 1, 1, -1, Math.Min(width - i, height));
                if (diagWinner2 != null) return diagWinner2;
            }

            for (int j = 0; j < height; j++)
            {
                var diagWinner1 = CheckLine(board, startX, startY + j, 1, 1, Math.Min(width, height - j));
                if (diagWinner1 != null) return diagWinner1;

                var diagWinner2 = CheckLine(board, startX + width - 1, startY + j, -1, 1, Math.Min(width, height - j));
                if (diagWinner2 != null) return diagWinner2;
            }
            
            return null;
        }
        
        /// <summary>
        /// Checks a single line (row, column, or diagonal) for a win condition.
        /// </summary>
        private EGamePiece? CheckLine(EGamePiece[][] board, int startX, int startY, int deltaX, int deltaY, int length)
        {
            int count = 0;
            EGamePiece currentPiece = EGamePiece.Empty;

            for (int i = 0; i < length; i++)
            {
                int x = startX + i * deltaX;
                int y = startY + i * deltaY;
                
                if (x >= DimensionX || y >= DimensionY || x < 0 || y < 0 || y >= board[x].Length)
                    break;

                var piece = board[x][y];
                if (piece == currentPiece && piece != EGamePiece.Empty)
                {
                    count++;
                }
                else
                {
                    currentPiece = piece;
                    count = piece != EGamePiece.Empty ? 1 : 0;
                }
                
                if (count >= _gameState.GameConfiguration.WinCondition)
                    return currentPiece;
            }

            return null;
        }
        
        /// <summary>
        /// Checks if the game has reached a draw state.
        /// </summary>
        public bool CheckDraw()
        {
            if (IsBoardFull()) 
            {
                return true;
            }
            
            var canMoveX = _gameState.PiecesLeftX > 0 || CanMovePiece(EGamePiece.X);
            var canMoveO = _gameState.PiecesLeftO > 0 || CanMovePiece(EGamePiece.O);
            
            return !canMoveX && !canMoveO;
        }
        
        /// <summary>
        /// Checks if the game board is completely filled with pieces.
        /// </summary>
        private bool IsBoardFull()
        {
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (IsCellEmpty(x,y))
                    {
                        return false;
                    }
                }
            }
            return true; 
        }
        
        /// <summary>
        /// Determines whether the specified player can move an existing game piece.
        /// </summary>
        private bool CanMovePiece(EGamePiece player)
        {
            var movesMade = player == EGamePiece.X ? _gameState.MovesMadeX : _gameState.MovesMadeO;
            var piecesLeft = player == EGamePiece.X ? _gameState.PiecesLeftX : _gameState.PiecesLeftO;
            
            return movesMade >= _gameState.GameConfiguration.MovePieceAfterNMove && piecesLeft <= 0;
        }

        /// <summary>
        /// Sets the game state from a JSON string.
        /// </summary>
        public void SetGameStateJson(string state)
        {
            _gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(state)!;
        }

        /// <summary>
        /// Executes an AI move based on blocking or random logic.
        /// </summary>
        public void AiMove()
        {
            var blockingMove = FindBlockingMove();
            if (blockingMove != null)
            {
                PlaceNewPiece(blockingMove.Value.x, blockingMove.Value.y);
                return;
            }
            
            GetRandomMoveWithinGrid();
        }

        /// <summary>
        /// Finds a move that blocks the opponent from winning.
        /// </summary>
        private (int x, int y)? FindBlockingMove()
        {
            int startX = UsesGrid ? GridPositionX : 0;
            int endX = UsesGrid ? GridPositionX + GridSizeWidth : DimensionX;
            int startY = UsesGrid ? GridPositionY : 0;
            int endY = UsesGrid ? GridPositionY + GridSizeHeight : DimensionY;
            
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (IsCellEmpty(x,y))
                    {
                        _gameState.GameBoard[x][y] = GetOpponentPlayer();
                        
                        if (CheckWinOnWholeBoard() == GetOpponentPlayer())
                        {
                            _gameState.GameBoard[x][y] = EGamePiece.Empty; 
                            return (x, y);
                        }
                        
                        _gameState.GameBoard[x][y] = EGamePiece.Empty;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the current opponent's game piece.
        /// </summary>
        private EGamePiece GetOpponentPlayer()
        {
            return _gameState.CurrentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }

        /// <summary>
        /// Finds and places a random valid move within the current grid or the whole board.
        /// </summary>
        private void GetRandomMoveWithinGrid()
        {
            if (UsesGrid)
            {
                for (int x = GridPositionX; x < GridPositionX + GridSizeWidth; x++)
                {
                    for (int y = GridPositionY; y < GridPositionY + GridSizeHeight; y++)
                    {
                        if (x < DimensionX && y < DimensionY && IsCellEmpty(x,y))
                        {
                            PlaceNewPiece(x, y);
                            return;
                        }
                    }
                }
            }
            
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (IsCellEmpty(x, y))
                    {
                        PlaceNewPiece(x, y);
                        return;
                    }
                }
            }
            throw new Exception("No valid moves available.");
        }
        
        /// <summary>
        /// Tries to parse the given input string into X and Y coordinates.
        /// </summary>
        public static bool TryParseCoordinates(string? input, out int x, out int y)
        {
            x = y = -1;
            var parts = input?.Split(',');

            if (parts != null && parts.Length == 2)
            {
                if (!TryParseCoordinate(parts[0], out x)) return false;
            
                if (!TryParseCoordinate(parts[1], out y)) return false;

                return true;
            }

            return false;
        }
    
        /// <summary>
        /// Tries to parse a single coordinate from a string input.
        /// </summary>
        private static bool TryParseCoordinate(string coordinate, out int parsedCoordinate)
        {
            parsedCoordinate = -1;
        
            if (int.TryParse(coordinate, out parsedCoordinate))
            {
                return true;
            }
        
            if (coordinate.Length == 1 && char.IsLetter(coordinate[0]))
            {
                parsedCoordinate = char.ToUpper(coordinate[0]) - 'A' + 10;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Waits until the user presses the Enter key.
        /// </summary>
        public static void WaitForEnter()
        {
            ConsoleKey keyPressed;
            do
            {
                keyPressed = Console.ReadKey(true).Key;
            } while (keyPressed != ConsoleKey.Enter);
        }
        
        /// <summary>
        /// Determines if a specified cell (X, Y) is within the current grid boundaries.
        /// </summary>
        public bool IsCellInGrid(int x, int y)
        {
            if (!_gameState.GameConfiguration.UsesGrid)
            {
                return false;
            }

            var gridPositionX = _gameState.GridPositionX.ToString();
            var gridPositionY = _gameState.GridPositionY.ToString();

            int gridStartX = int.TryParse(gridPositionX, out var startX) ? startX : -1;
            int gridStartY = int.TryParse(gridPositionY, out var startY) ? startY : -1;

            int gridEndX = gridStartX + _gameState.GameConfiguration.GridSizeWidth - 1;
            int gridEndY = gridStartY + _gameState.GameConfiguration.GridSizeHeight - 1;

            return x >= gridStartX && x <= gridEndX && y >= gridStartY && y <= gridEndY;
        }
        
        /// <summary>
        /// Checks if Cell is empty
        /// </summary>
        private bool IsCellEmpty(int x, int y) => 
            _gameState.GameBoard[x][y] == EGamePiece.Empty;
        
    }
    
}
