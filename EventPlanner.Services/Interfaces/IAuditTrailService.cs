using EventPlanner.Api.Contracts.AuditTrail;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Services.Interfaces;

public interface IAuditTrailService
{
    Task LogAsyncSingle<T>(AuditAction action, AuditSubject subject, T? oldValues, T? newValues);
    Task LogAsyncList<T>(AuditAction action, AuditSubject subject, List<T>? oldValues, List<T>? newValues);
    Task<IEnumerable<AuditTrailResponse>> GetAuditLogsAsync(string? action, string? subject);
}