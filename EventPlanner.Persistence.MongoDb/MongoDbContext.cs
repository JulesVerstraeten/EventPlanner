using EventPlanner.Domain.Models;
using MongoDB.Driver;

namespace EventPlanner.Persistence.MongoDb;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("EventPlanner");
    }
    
    public IMongoCollection<AuditTrail> AuditTrail => _database.GetCollection<AuditTrail>("AuditTrail");
}