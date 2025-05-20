using EventPlanner.Api.Contracts.Event;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Api.Contracts.Task;

public class TaskRequest
{
    public required int EventId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string Importance { get; set; } = "Must";
    public string Status { get; set; } = "Todo";
    public DateTime DeadlineDateTime { get; set; } = DateTime.Now;
}