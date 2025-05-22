using MongoDB.Bson;

namespace EventPlanner.Api.Contracts.AuditTrail;

public class AuditTrailResponse
{
    public ObjectId Id { get; set; }
    public required DateTime EntryDate { get; set; }
    public required string Subject { get; set; }
    public required string Action { get; set; }
    public required List<Dictionary<string, object>>? OldValues { get; set; }
    public required List<Dictionary<string, object>>? NewValues { get; set; }
}