namespace EventPlanner.Dapper.Reporting.Models;

public class Report
{
    public required int EventId { get; set; }
    public required string EventName { get; set; }
    public required int LocationId { get; set; }
    public required string LocationName { get; set; }
    public required double PercentageCompleted { get; set; }
    public required int MustTasksInTodo { get; set; } 
    public required string LatestUpdate { get; set; }
}