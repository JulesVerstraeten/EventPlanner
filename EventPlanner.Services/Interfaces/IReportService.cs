using EventPlanner.Dapper.Reporting.Models;

namespace EventPlanner.Services.Interfaces;

public interface IReportService
{
    Task<List<Report>> GetReportOverviewAsync();
}