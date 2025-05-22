using EventPlanner.Api.Contracts.AuditTrail;
using EventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers;

[ApiController]
[Route("/api/audittrail")]
public class AuditController(IAuditTrailService context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<AuditTrailResponse>> GetAuditLogsAsync([FromQuery]string? action, [FromQuery]string? subject)
    {
        var logs = await context.GetAuditLogsAsync(action, subject);
        return logs;
    }
}