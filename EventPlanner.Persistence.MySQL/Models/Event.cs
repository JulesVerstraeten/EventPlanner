using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Persistence.Models;

public class Event
{
    public int Id { get; set; }
    public required Location Location { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
}