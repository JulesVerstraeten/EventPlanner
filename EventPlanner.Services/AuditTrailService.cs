using System.Text.Json;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence.MongoDb;
using EventPlanner.Persistence.MongoDb.Models;
using EventPlanner.Services.Extensions;
using EventPlanner.Services.Interfaces;
using MongoDB.Bson;

namespace EventPlanner.Services;

public class AuditTrailService(MongoDbContext context) : IAuditTrailService
{
    public async Task LogAsyncSingle<T>(AuditAction action, AuditSubject subject, T? oldValues, T? newValues)
    {
        try
        {
            var log = new AuditTrail()
            {
                Subject = subject.ToString(),
                Action = action.ToString(),
                EntryDate = DateTime.UtcNow,
                OldValues = oldValues != null ? Converter.ToBson(oldValues) : null,
                NewValues = newValues != null ? Converter.ToBson(newValues) : null
            };
            
            await context.AuditTrail.InsertOneAsync(log);
        }
        catch (Exception)
        {
            throw new Exception("Failed to log in MongoDB");
        }
    }
    
    public async Task LogAsyncList<T>(AuditAction action, AuditSubject subject, List<T>? oldValues, List<T>? newValues)
    {
        try
        {
            var log = new AuditTrail()
            {
                Subject = subject.ToString(),
                Action = action.ToString(),
                EntryDate = DateTime.UtcNow,
                OldValues = oldValues != null ? Converter.ToBson(oldValues) : null,
                NewValues = newValues != null ? Converter.ToBson(newValues) : null
            };
            
            await context.AuditTrail.InsertOneAsync(log);
        }
        catch (Exception)
        {
            throw new Exception("Failed to log in MongoDB");
        }
    }
}