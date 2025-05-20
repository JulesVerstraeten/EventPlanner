// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence.Models;
using EventPlanner.Persistence.MongoDb;
using EventPlanner.Persistence.MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;

MongoDbContext context = new MongoDbContext();

Location location = new Location()
{
    Id = 1,
    Name = "New York",
    GpsLat = 19,
    GpsLon= 20
};

var json = JsonSerializer.Serialize(location);

// AuditTrail auditTrail = new AuditTrail()
// {
//     EntryDate = DateTime.Now,
//     Subject = AuditSubject.Location,
//     Action = AuditAction.Create,
//     OldValues = null,
//     NewValues = BsonDocument.Parse(json)
// };

//context.AuditTrail.InsertOne(auditTrail);

var result = await context.AuditTrail.Find(_ => true).ToListAsync();

Console.WriteLine(result.ToJson());
