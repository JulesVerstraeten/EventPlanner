namespace EventPlanner.Api.Contracts.Location;

public class LocationRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required double GpsLat { get; set; }
    public required double GpsLon { get; set; }
}