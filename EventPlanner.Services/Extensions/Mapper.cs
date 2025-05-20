using EventPlanner.Api.Contracts.Event;
using EventPlanner.Api.Contracts.Location;
using EventPlanner.Api.Contracts.Task;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence;
using EventPlanner.Persistence.Models;
using Task = EventPlanner.Persistence.Models.Task;

namespace EventPlanner.Services.Extensions;

public class Mapper()
{
    
    // MySQL Entity to Response
    // Api Request to MySQL Entity
    public static LocationResponse ToContract(Location entity)
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
            throw new Exception("Couldn't make a contract", e);
        }
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
        var eventEntity = context.Events.Find(request.EventId);
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
}