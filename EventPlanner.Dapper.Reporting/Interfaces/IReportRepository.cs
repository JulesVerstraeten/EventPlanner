using EventPlanner.Dapper.Reporting.Models;

namespace EventPlanner.Dapper.Reporting.Interfaces;

public interface IReportRepository
{
    Task<List<Report>> GetReportOverviewAsync();
}