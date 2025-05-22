using EventPlanner.Dapper.Reporting.Models;
using EventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers;

[ApiController]
[Route("api/report/overview")]
public class ReportController(IReportService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Report>>> GetReportOverview()
    {
        var result = await service.GetReportOverviewAsync();
        return Ok(result);
    }
}