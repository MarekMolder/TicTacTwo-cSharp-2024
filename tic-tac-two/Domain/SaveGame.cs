using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameBrain;

namespace Domain;

public class SaveGame
{
    public int Id { get; set; }

    [MaxLength(128)] 
    public string CreatedAtDateTime { get; set; } = default!;

    [MaxLength(10240)]
    public string State { get; set; } = default!;
    
    // Expose the Foreign Key
    public int ConfigurationId { get; set; }
    
    [ForeignKey("ConfigurationId")]
    public GameConfiguration? GameConfiguration { get; set; }
}