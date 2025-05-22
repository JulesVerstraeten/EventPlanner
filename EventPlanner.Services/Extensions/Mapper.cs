using EventPlanner.Api.Contracts.AuditTrail;
using EventPlanner.Api.Contracts.Event;
using EventPlanner.Api.Contracts.Location;
using EventPlanner.Api.Contracts.Task;
using EventPlanner.Domain.Enums;
using EventPlanner.Domain.Models;
using EventPlanner.Persistence;
using EventPlanner.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Task = EventPlanner.Persistence.Models.Task;

namespace EventPlanner.Services.Extensions;

public class Mapper()
{
    
    // MySQL Entity to Response
    // Api Request to MySQL Entity
    public static LocationResponse ToContract(Location entity)
    {
        try
        {
            return new LocationResponse()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                GpsLat = entity.GpsLat,
                GpsLon = entity.GpsLon,
            };
        }
        catch (Exception)
        {
            throw new Exception("Could not convert to location contract");
        }
    }
    public static Location ToEntity(LocationRequest request)
    {
        return new Location()
        {
            Name = request.Name,
            Description = request.Description,
            GpsLat = request.GpsLat,
            GpsLon = request.GpsLon,
        };
    }
    public static EventResponse ToContract (Event entity)
    {
        try
        {
            return new EventResponse()
            {
                Id = entity.Id,
                Location = ToContract(entity.Location),
                Name = entity.Name,
                Start = entity.Start,
                End = entity.End,
            };

        }
        catch (Exception e)
        {
            throw new Exception("Couldn't make a event contract", e);
        }
    }

    public static EventTasksResponse ToContract(List<Task> tasksEntity)
    {
        EventResponse eventResponse = Mapper.ToContract(tasksEntity[0].Event);
        List<EventTask> eventTasks = new List<EventTask>();
        
        foreach (Task taskEntity in tasksEntity)
        {
            eventTasks.Add(
                new EventTask()
            {
                Id = taskEntity.Id,
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                Importance = taskEntity.Importance,
                Status = taskEntity.Status,
                DeadlineDateTime = taskEntity.DeadlineDateTime,
            });
        }

        return new EventTasksResponse()
        {
            Event = eventResponse,
            EventTasks = eventTasks,
        };
    }
    public static Event ToEntity(EventRequest request, EventContext context)
    {
        var location = context.Locations.Find(request.LocationId);
        if (location == null) throw new Exception("Location not found");
        
        return new Event()
        {
            Name = request.Name,
            Location = location,
            Start = request.Start,
            End = request.End,
        };
    }
    
    public static TaskResponse ToContract(Task entity)
    {
        return new TaskResponse()
        {
            Id = entity.Id,
            Event = ToContract(entity.Event),
            Name = entity.Name,
            Description = entity.Description,
            Importance = entity.Importance,
            Status = entity.Status,
            DeadlineDateTime = entity.DeadlineDateTime,
        };
    }
    public static Task ToEntity(TaskRequest request, EventContext context)
    {
        var eventEntity = context.Events.Include(o => o.Location).FirstOrDefault(o => o.Id == request.EventId);
        if (eventEntity == null) throw new Exception("Event not found");

        var importance = request.Importance switch
        {
            "Must" => Importance.Must,
            "Should" => Importance.Should,
            "Could" => Importance.Could,
            _ => throw new Exception("Incorrect importance")
        };

        var status = request.Status switch
        {
            "Todo" => Status.Todo,
            "Doing" => Status.Doing,
            "Done" => Status.Done,
            "Cancelled" => Status.Cancelled,
            _ => throw new Exception("Incorrect status")
        };

        return new Task()
        {
            Event = eventEntity,
            Name = request.Name,
            Description = request.Description,
            Importance = importance,
            Status = status,
            DeadlineDateTime = request.DeadlineDateTime,
        };
    }

    public static AuditTrailResponse ToContract(AuditTrail entity)
    {
        return new AuditTrailResponse()
        {
            Id = entity.Id,
            Action = entity.Action,
            Subject = entity.Subject,
            EntryDate = entity.EntryDate,
            OldValues = entity.OldValues == BsonNull.Value ? null : ConvertBsonValueToListOfDict(entity.OldValues),
            NewValues = entity.NewValues == BsonNull.Value ? null : ConvertBsonValueToListOfDict(entity.NewValues),

        };
    }
    
    public static List<Dictionary<string, object>>? ConvertBsonValueToListOfDict(BsonValue? bsonValue)
    {
        if (bsonValue == null || bsonValue == BsonNull.Value)
            return null;

        if (bsonValue is not BsonArray bsonArray) return null;
        var list = new List<Dictionary<string, object?>>();

        foreach (var item in bsonArray)
        {
            if (item is BsonDocument doc)
            {
                var dict = new Dictionary<string, object?>();
                foreach (var element in doc.Elements)
                {
                    dict[element.Name] = BsonValueToDotNetObject(element.Value);
                }
                list.Add(dict);
            }
        }

        return list;
    }

    private static object? BsonValueToDotNetObject(BsonValue value)
    {
        return value switch
        {
            BsonString s => s.AsString,
            BsonInt32 i => i.AsInt32,
            BsonInt64 l => l.AsInt64,
            BsonDouble d => d.AsDouble,
            BsonBoolean b => b.AsBoolean,
            BsonDateTime dt => dt.ToUniversalTime(),
            BsonNull _ => null!,
            BsonDocument doc => ConvertBsonDocumentToDict(doc),
            BsonArray arr => ConvertBsonValueToListOfDict(arr),
            _ => value.ToString()
        };
    }

    private static Dictionary<string, object> ConvertBsonDocumentToDict(BsonDocument doc)
    {
        var dict = new Dictionary<string, object>();
        foreach (var element in doc.Elements)
        {
            dict[element.Name] = BsonValueToDotNetObject(element.Value);
        }
        return dict;
    }
}