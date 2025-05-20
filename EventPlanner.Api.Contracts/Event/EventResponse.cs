using EventPlanner.Api.Contracts.Location;

namespace EventPlanner.Api.Contracts.Event;

public class EventResponse
{
    public required int Id { get; set; }
    public required LocationResponse Location { get; set; }
    public required string Name { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
}