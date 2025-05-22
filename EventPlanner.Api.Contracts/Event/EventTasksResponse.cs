using EventPlanner.Api.Contracts.Location;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Api.Contracts.Event;

public class EventTasksResponse
{
    public required EventResponse Event { get; set; }
    public required List<EventTask> EventTasks { get; set; }
}

public class EventTask
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Importance Importance { get; set; }
    public required Status Status { get; set; }
    public required DateTime DeadlineDateTime { get; set; }
}