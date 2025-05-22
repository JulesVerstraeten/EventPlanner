using System.Text.Json;
using EventPlanner.Api.Contracts.AuditTrail;
using EventPlanner.Domain.Enums;
using EventPlanner.Domain.Models;
using EventPlanner.Persistence.MongoDb;
using EventPlanner.Services.Extensions;
using EventPlanner.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

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
                OldValues = oldValues != null ? Converter.ToBson(oldValues) : BsonNull.Value,
                NewValues = newValues != null ? Converter.ToBson(newValues) : BsonNull.Value,
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
                OldValues = oldValues != null ? Converter.ToBson(oldValues) : BsonNull.Value,
                NewValues = newValues != null ? Converter.ToBson(newValues) : BsonNull.Value
            };
            
            await context.AuditTrail.InsertOneAsync(log);
        }
        catch (Exception)
        {
            throw new Exception("Failed to log in MongoDB");
        }
    }
    public async Task<IEnumerable<AuditTrailResponse>> GetAuditLogsAsync(string? action, string? subject)
    {
        var filterBuilder = Builders<AuditTrail>.Filter;
        var filters = new List<FilterDefinition<AuditTrail>>();

        if (!string.IsNullOrEmpty(subject))
        {
            filters.Add(filterBuilder.Eq(e => e.Subject, subject));
        }
        
        if (!string.IsNullOrEmpty(action))
        {
            filters.Add(filterBuilder.Eq(e => e.Action, action));
        }
        
        var filter = filters.Any() ? filterBuilder.And(filters) : FilterDefinition<AuditTrail>.Empty;
        var logsEntity = await context.AuditTrail.Find(filter).ToListAsync();
        var logs = logsEntity.Select(o => Mapper.ToContract(o));
        
        return logs;
    }
}