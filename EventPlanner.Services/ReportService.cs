using EventPlanner.Dapper.Reporting;
using EventPlanner.Dapper.Reporting.Interfaces;
using EventPlanner.Dapper.Reporting.Models;
using EventPlanner.Services.Interfaces;

namespace EventPlanner.Services;

public class ReportService() : IReportService
{
    private readonly IReportRepository _repo = new ReportRepository();
    
    public async Task<List<Report>> GetReportOverviewAsync()
    {
        return await _repo.GetReportOverviewAsync();
    }
}