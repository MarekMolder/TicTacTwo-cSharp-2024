using System.Text.Json;
using Domain;

namespace GameBrain;

public class GetConfiguration
{
    public static GameConfiguration LoadGameConfiguration (string gameStateJson)
    {
        // Deserialize the JSON string to a dynamic object
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var gameData = JsonSerializer.Deserialize<JsonElement>(gameStateJson, options);

        // Extract the GameSettings values from the JSON object
        var gameConfigJson = gameData.GetProperty("GameConfiguration");

        // Create a new GameSettings object with extracted values
        var gameConfig = new GameConfiguration()
        {
            Name = gameConfigJson.GetProperty("Name").GetString() ?? "Default",
            BoardSizeWidth = gameConfigJson.GetProperty("BoardSizeWidth").GetInt32(),
            BoardSizeHeight = gameConfigJson.GetProperty("BoardSizeHeight").GetInt32(),
            PiecesNumber = gameConfigJson.GetProperty("PiecesNumber").GetInt32(),
            WinCondition = gameConfigJson.GetProperty("WinCondition").GetInt32(),
            UsesGrid = gameConfigJson.GetProperty("UsesGrid").GetBoolean(),
            GridSizeWidth = gameConfigJson.GetProperty("GridSizeWidth").GetInt32(),
            GridSizeHeight = gameConfigJson.GetProperty("GridSizeHeight").GetInt32(),
            MovePieceAfterNMove = gameConfigJson.GetProperty("MovePieceAfterNMove").GetInt32(),
            MoveGridAfterNMove = gameConfigJson.GetProperty("MoveGridAfterNMove").GetInt32(),
            GridPositionY = gameConfigJson.GetProperty("GridPositionY").GetInt32(),
            GridPositionX = gameConfigJson.GetProperty("GridPositionX").GetInt32(),
        };

        return gameConfig;
    }
    
    public static GameState LoadGameState(string gameStateJson, GameConfiguration gameConfig)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var gameData = JsonSerializer.Deserialize<JsonElement>(gameStateJson, options);

        var gameBoardJson = gameData.GetProperty("GameBoard");
        var gameBoard = new EGamePiece[gameBoardJson.GetArrayLength()][];
        for (int i = 0; i < gameBoardJson.GetArrayLength(); i++)
        {
            var row = gameBoardJson[i];
            var boardRow = new EGamePiece[row.GetArrayLength()];
            for (int j = 0; j < row.GetArrayLength(); j++)
            {
                boardRow[j] = (EGamePiece)row[j].GetInt32();
            }
            gameBoard[i] = boardRow;
        }

        // Create the GameState object
        var gameState = new GameState(gameBoard, gameConfig)
        {
            GameBoard = gameBoard,
            GameConfiguration = gameConfig,
            CurrentPlayer = (EGamePiece)gameData.GetProperty("CurrentPlayer").GetInt32(),
            PiecesLeftX = gameData.GetProperty("PiecesLeftX").GetInt32(),
            PiecesLeftO = gameData.GetProperty("PiecesLeftO").GetInt32(),
            MovesMadeX = gameData.GetProperty("MovesMadeX").GetInt32(),
            MovesMadeO = gameData.GetProperty("MovesMadeO").GetInt32(),
            PlayerO = gameData.GetProperty("PlayerO").GetString(),
            PlayerX = gameData.GetProperty("PlayerX").GetString(),
            GridPositionX = gameData.GetProperty("GridPositionX").GetInt32(),
            GridPositionY = gameData.GetProperty("GridPositionY").GetInt32(),
        };

        return gameState;
    }
}