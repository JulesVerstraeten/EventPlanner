using System.Text.Json;
using MongoDB.Bson;

namespace EventPlanner.Services.Extensions;

public class Converter
{
    public static BsonArray ToBson<T>(T obj)
    {
        var bsonArray = new BsonArray();
        var json = JsonSerializer.Serialize(obj);
        bsonArray.Add(BsonDocument.Parse(json));
        return bsonArray;
    }
    
    public static BsonArray ToBson<T>(List<T> obj)
    {
        var bsonArray = new BsonArray();
        foreach (var item in obj)
        {
            var json = JsonSerializer.Serialize(item);
            bsonArray.Add(BsonDocument.Parse(json));
        }
        return bsonArray;
    }
}