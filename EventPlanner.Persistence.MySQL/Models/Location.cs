using System.ComponentModel.DataAnnotations;

namespace EventPlanner.Persistence.Models;

public class Location
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(250)]
    public string? Description { get; set; }
    [Range(-90, 90)]
    public required double GpsLat { get; set; }
    [Range(-180, 180)]
    public required double GpsLon { get; set; }
    
}