using EventPlanner.Api.Contracts.Event;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Api.Contracts.Task;

public class TaskResponse
{
    public required int Id { get; set; }
    public required EventResponse Event { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Importance Importance { get; set; }
    public required Status Status { get; set; }
    public required DateTime DeadlineDateTime { get; set; }
}