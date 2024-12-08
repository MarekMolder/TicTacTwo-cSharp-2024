using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain;

/// <summary>
/// Represents a saved game in the database, containing the game state and metadata.
/// </summary>
public class SaveGame
{
    /// <summary>
    /// Gets or sets the unique identifier for the saved game.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the creation date and time of the saved game.
    /// The date and time are stored as a string in ISO 8601 format.
    /// </summary>
    [MaxLength(128)] 
    public string CreatedAtDateTime { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the serialized state of the game at the time of saving.
    /// This contains the entire game state, which can be deserialized for resumption.
    /// </summary>
    [MaxLength(10240)]
    public string State { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the identifier for the associated game configuration.
    /// This foreign key links the saved game to its specific configuration settings.
    /// </summary>
    public int ConfigurationId { get; set; }
    
    /// <summary>
    /// Gets or sets the game configuration associated with the saved game.
    /// This represents the settings of the game such as board size, win condition, etc.
    /// </summary>
    [ForeignKey("ConfigurationId")]
    public GameConfiguration? GameConfiguration { get; set; }
}