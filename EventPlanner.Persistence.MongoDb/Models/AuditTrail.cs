using System.Text.Json.Nodes;
using EventPlanner.Domain.Enums;
using MongoDB.Bson;

namespace EventPlanner.Persistence.MongoDb.Models;

public class AuditTrail
{
    public ObjectId Id { get; set; }
    public required DateTime EntryDate { get; set; }
    public required string Subject { get; set; }
    public required string Action { get; set; }
    public required BsonArray? OldValues { get; set; }
    public required BsonArray? NewValues { get; set; }
}