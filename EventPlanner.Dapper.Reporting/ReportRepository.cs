using Dapper;
using EventPlanner.Dapper.Reporting.Interfaces;
using EventPlanner.Dapper.Reporting.Models;
using MongoDB.Bson;
using MySqlConnector;

namespace EventPlanner.Dapper.Reporting;

public class ReportRepository : IReportRepository
{
    private readonly string _connectionString = "Server=127.0.0.1;Port=3307;Database=event_planner_db;User Id=root;Password=root;";

    public async Task<List<Report>> GetReportOverviewAsync()
    {
        string sql = "SELECT \ne.Id as EventId, \ne.Name as EventName, \nl.Id as LocationId, \nl.Name as LocationName,\nROUND(100 / COUNT(t.Id) * (SELECT COUNT(t2.Id) FROM Tasks t2 WHERE t2.Status = 2), 0) AS PercentageCompleted,\n(SELECT COUNT(*) FROM Tasks t3 WHERE t3.Importance = 0 AND t3.Status = 0) AS MustTasksInTodo,\n(SELECT t4.Name FROM Tasks t4 ORDER BY t4.Id desc LIMIT 1) AS LatestUpdate\nFROM Tasks t \nJOIN Events e on t.EventId = e.Id \nJOIN Locations l ON e.LocationId = l.Id\nGROUP BY e.Id";
        await using var connection = new MySqlConnection(_connectionString);
        var multi = await connection.QueryMultipleAsync(sql);
        var tasks = await multi.ReadAsync<Report>();
        
        return tasks.ToList();
    }
}