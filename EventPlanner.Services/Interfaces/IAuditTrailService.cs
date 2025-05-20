using EventPlanner.Domain.Enums;
using EventPlanner.Persistence.MongoDb.Models;

namespace EventPlanner.Services.Interfaces;

public interface IAuditTrailService
{
    Task LogAsyncSingle<T>(AuditAction action, AuditSubject subject, T? oldValues, T? newValues);
    Task LogAsyncList<T>(AuditAction action, AuditSubject subject, List<T>? oldValues, List<T>? newValues);
}