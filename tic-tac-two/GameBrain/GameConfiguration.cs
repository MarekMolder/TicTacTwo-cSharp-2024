namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name {get; set;} = default!;

    public int BoardSizeWidth { get; set; }
    public int BoardSizeHeight { get; set; }
    
    public int PiecesNumber { get; set; }
    public int WinCondition { get; set; }
    
    public int GridSizeWidth { get; set; }
    
    public int GridSizeHeight { get; set; }
    public int MovePieceAfterNMove { get; set; }
    public int MoveGridAfterNMove { get; set; }
    
    

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight} | to win: {WinCondition} | can move pieces after ";
    
}