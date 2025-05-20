using System.ComponentModel.DataAnnotations;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Persistence.Models;

public class Task
{
    public int Id { get; set; }
    public required Event Event { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(250)]
    public string? Description { get; set; }
    public required Importance Importance { get; set; }
    public required Status Status { get; set; }
    public required DateTime DeadlineDateTime { get; set; }
}