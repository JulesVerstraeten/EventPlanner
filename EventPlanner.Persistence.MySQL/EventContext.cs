using EventPlanner.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Task = EventPlanner.Persistence.Models.Task;

namespace EventPlanner.Persistence;

public class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options) : base(options)
    {
    }
    
    public DbSet<Location> Locations { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    private readonly string _connectionString = "Server=127.0.0.1;Port=3307;Database=event_planner_db;User Id=root;Password=root;";
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(
            _connectionString,
            new MySqlServerVersion(ServerVersion.AutoDetect(_connectionString))
        );
    }
}