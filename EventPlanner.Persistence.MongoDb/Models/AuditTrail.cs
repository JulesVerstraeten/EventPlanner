using MongoDB.Bson;

namespace EventPlanner.Domain.Models;

public class AuditTrail
{
    public ObjectId Id { get; set; }
    public required DateTime EntryDate { get; set; }
    public required string Subject { get; set; }
    public required string Action { get; set; }
    public required BsonValue OldValues { get; set; }
    public required BsonValue NewValues { get; set; }
}