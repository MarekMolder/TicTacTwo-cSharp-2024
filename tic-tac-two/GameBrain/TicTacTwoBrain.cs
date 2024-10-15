namespace GameBrain
{
    public class TicTacTwoBrain
    {
        private readonly EGamePiece[,] _gameBoard;
        private EGamePiece _currentPlayer;

        private readonly GameConfiguration _gameConfiguration;

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
            GridSizeWidth = _gameConfiguration.GridSizeWidth;
            GridSizeHeight = _gameConfiguration.GridSizeHeight;
            GridPositionX = _gameConfiguration.GridPositionX;
            GridPositionY = _gameConfiguration.GridPositionY;
            UsesGrid = _gameConfiguration.UsesGrid;
        }

        public EGamePiece[,] GameBoard => GetBoardCopy();
        public EGamePiece CurrentPlayer => _currentPlayer;

        public int GridSizeWidth { get; }
        public int GridSizeHeight { get; }

        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }

        public int DimensionX => _gameBoard.GetLength(0);
        public int DimensionY => _gameBoard.GetLength(1);

        public int PiecesLeftX => _piecesLeftX;
        public int PiecesLeftO => _piecesLeftO;
        public bool UsesGrid { get; }

        private EGamePiece[,] GetBoardCopy()
        {
            var copy = new EGamePiece[DimensionX, DimensionY];
            Array.Copy(_gameBoard, copy, _gameBoard.Length);
            return copy;
        }

        public bool MakeAMove()
        {
            var canMovePiece = CanMovePiece();
            var canMoveGrid = CanMoveGrid();
            var hasPiecesLeft = HasPiecesLeft();

            if (!hasPiecesLeft)
            {
                return HandleNoPiecesLeft(canMovePiece, canMoveGrid);
            }

            if (canMovePiece || canMoveGrid)
            {
                return HandlePlayerChoice(canMovePiece, canMoveGrid);
            }

            return PlaceNewPiece();
        }

        private bool HasPiecesLeft()
        {
            return _currentPlayer == EGamePiece.X ? _piecesLeftX > 0 : _piecesLeftO > 0;
        }

        private bool CanMovePiece()
        {
            var movesMade = _currentPlayer == EGamePiece.X ? _movesMadeX : _movesMadeO;
            return movesMade >= _gameConfiguration.MovePieceAfterNMove;
        }

        private bool CanMoveGrid()
        {
            var movesMade = _currentPlayer == EGamePiece.X ? _movesMadeX : _movesMadeO;
            return movesMade >= _gameConfiguration.MoveGridAfterNMove;
        }

        private bool HandleNoPiecesLeft(bool canMovePiece, bool canMoveGrid)
        {
            if (canMovePiece && canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece or the grid.");
                return PromptMovePieceOrGrid();
            }
            if (canMovePiece)
            {
                Console.WriteLine("You have no pieces left. You can move an existing piece.");
                return MoveExistingPiece();
            }
            if (canMoveGrid)
            {
                Console.WriteLine("You have no pieces left. You can move the grid.");
                return MoveGrid();
            }

            Console.WriteLine("You have no valid moves. Your turn is skipped.");
            SwitchPlayer();
            return true;
        }

        private bool HandlePlayerChoice(bool canMovePiece, bool canMoveGrid)
        {
            while (true)
            {
                Console.WriteLine($"Do you want to place a new piece{(canMovePiece ? ", move an existing piece" : "")}{(canMoveGrid ? ", or move the grid" : "")}?");

                var options = new List<string> { "new" };
                if (canMovePiece) options.Add("old");
                if (canMoveGrid) options.Add("grid");

                Console.WriteLine($"Options: {string.Join("/", options)}");
                var response = Console.ReadLine()?.Trim().ToLower();

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
            var (x, y) = GetCoordinatesFromPlayer("Enter the coordinates where you want to place your piece <x,y>:");
            if (_gameBoard[x, y] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue();
                return false;
            }

            _gameBoard[x, y] = _currentPlayer;
            UpdatePlayerStateAfterMove();
            SwitchPlayer();
            return true;
        }

        private bool MoveExistingPiece()
        {
            var (oldX, oldY) = GetCoordinatesFromPlayer("Enter the coordinates of the piece you want to move <x,y>:");
            if (_gameBoard[oldX, oldY] != _currentPlayer)
            {
                Console.WriteLine("Invalid selection. That is not your piece.");
                PauseBeforeContinue();
                return false;
            }

            var (newX, newY) = GetCoordinatesFromPlayer("Enter the new coordinates where you want to move the piece <x,y>:");
            if (_gameBoard[newX, newY] != EGamePiece.Empty)
            {
                Console.WriteLine("Invalid move. The spot is already occupied.");
                PauseBeforeContinue();
                return false;
            }

            _gameBoard[newX, newY] = _currentPlayer;
            _gameBoard[oldX, oldY] = EGamePiece.Empty;
            SwitchPlayer();
            return true;
        }

        private bool MoveGrid()
        {
            var (newGridX, newGridY) = GetCoordinatesFromPlayer("Enter new coordinates for the grid <x,y>:");
            if (!IsValidGridPosition(newGridX, newGridY))
            {
                Console.WriteLine("Invalid grid position. Please try again.");
                PauseBeforeContinue();
                return false;
            }

            GridPositionX = newGridX;
            GridPositionY = newGridY;
            SwitchPlayer();
            return true;
        }

        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x + GridSizeWidth <= DimensionX && y >= 0 && y + GridSizeHeight <= DimensionY;
        }

        private void UpdatePlayerStateAfterMove()
        {
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
        }

        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }

        private (int x, int y) GetCoordinatesFromPlayer(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                var input = Console.ReadLine();

                if (TryParseCoordinates(input, out int x, out int y) && IsWithinBoard(x, y))
                {
                    return (x, y);
                }

                Console.WriteLine("Invalid input. Please enter valid coordinates within the board limits.");
            }
        }

        private bool TryParseCoordinates(string? input, out int x, out int y)
        {
            x = y = -1;
            var parts = input?.Split(',');
            return parts != null && parts.Length == 2
                   && int.TryParse(parts[0], out x)
                   && int.TryParse(parts[1], out y);
        }

        private bool IsWithinBoard(int x, int y)
        {
            return x >= 0 && x < DimensionX && y >= 0 && y < DimensionY;
        }

        private void PauseBeforeContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        private bool PromptMovePieceOrGrid()
        {
            while (true)
            {
                Console.WriteLine("Do you want to move an existing piece (old) or move the grid (grid)? Options: old/grid");
                var response = Console.ReadLine()?.Trim().ToLower();

                if (response == "old")
                {
                    return MoveExistingPiece();
                }
                if (response == "grid")
                {
                    return MoveGrid();
                }

                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        public EGamePiece? CheckWin()
        {
            if (GridSizeWidth == 0 || GridSizeHeight == 0)
            {
                return CheckWinOnWholeBoard();
            }

            return CheckWinWithinGrid();
        }

        private EGamePiece? CheckWinOnWholeBoard()
        {
            return CheckAllLines(_gameBoard, 0, 0, DimensionX, DimensionY);
        }

        private EGamePiece? CheckWinWithinGrid()
        {
            return CheckAllLines(_gameBoard, GridPositionX, GridPositionY, GridSizeWidth, GridSizeHeight);
        }

        private EGamePiece? CheckAllLines(EGamePiece[,] board, int startX, int startY, int width, int height)
        {
            // Check rows and columns
            for (int i = 0; i < width; i++)
            {
                var winner = CheckLine(board, startX + i, startY, 0, 1, height); // Vertical
                if (winner != null) return winner;

                winner = CheckLine(board, startX, startY + i, 1, 0, width); // Horizontal
                if (winner != null) return winner;
            }

            // Check diagonals
            var diagWinner = CheckLine(board, startX, startY, 1, 1, Math.Min(width, height)); // Top-left to bottom-right
            if (diagWinner != null) return diagWinner;

            diagWinner = CheckLine(board, startX + width - 1, startY, -1, 1, Math.Min(width, height)); // Top-right to bottom-left
            if (diagWinner != null) return diagWinner;

            return null;
        }

        private EGamePiece? CheckLine(EGamePiece[,] board, int startX, int startY, int deltaX, int deltaY, int length)
        {
            int count = 0;
            EGamePiece currentPiece = EGamePiece.Empty;

            for (int i = 0; i < length; i++)
            {
                int x = startX + i * deltaX;
                int y = startY + i * deltaY;

                if (x >= DimensionX || y >= DimensionY || x < 0 || y < 0)
                    break;

                var piece = board[x, y];
                if (piece == currentPiece && piece != EGamePiece.Empty)
                {
                    count++;
                }
                else
                {
                    currentPiece = piece;
                    count = piece != EGamePiece.Empty ? 1 : 0;
                }

                if (count >= _gameConfiguration.WinCondition)
                    return currentPiece;
            }
            return null;
        }

        public bool CheckDraw()
        {
            if (IsBoardFull())
            {
                return true;
            }

            var canMoveX = _piecesLeftX > 0 || CanMovePieceForPlayer(EGamePiece.X);
            var canMoveO = _piecesLeftO > 0 || CanMovePieceForPlayer(EGamePiece.O);

            return !canMoveX && !canMoveO;
        }

        private bool IsBoardFull()
        {
            for (int x = 0; x < DimensionX; x++)
            {
                for (int y = 0; y < DimensionY; y++)
                {
                    if (_gameBoard[x, y] == EGamePiece.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CanMovePieceForPlayer(EGamePiece player)
        {
            var movesMade = player == EGamePiece.X ? _movesMadeX : _movesMadeO;
            var piecesLeft = player == EGamePiece.X ? _piecesLeftX : _piecesLeftO;

            return movesMade >= _gameConfiguration.MovePieceAfterNMove && piecesLeft <= 0;
        }
    }
}
