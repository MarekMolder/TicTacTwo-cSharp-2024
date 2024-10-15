namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; }
    public int BoardSizeHeight { get; set; }
    
    public int PiecesNumber { get; set; }
    public int WinCondition { get; set; }
    
    public bool UsesGrid { get; set; }
    
    public int GridSizeWidth { get; set; }
    public int GridSizeHeight { get; set; }
    public int MovePieceAfterNMove { get; set; }
    public int MoveGridAfterNMove { get; set; }

    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    public override string ToString() =>
        $"Name - {Name}" +
        $"| Board {BoardSizeWidth}x{BoardSizeHeight} " +
        $"| Uses grid {UsesGrid} " +
        $"| Grid {GridSizeWidth}x{GridSizeHeight} " +
        $"| grid position: {GridPositionX},{GridPositionY} " +
        $"| to win: {WinCondition} " +
        $"| can move pieces after {MovePieceAfterNMove} moves " +
        $"| can move grid after {MovePieceAfterNMove} moves ";
    
}