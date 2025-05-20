using EventPlanner.Api.Contracts.Event;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence;
using EventPlanner.Services.Extensions;
using EventPlanner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Services;

public class EventService(EventContext context, IAuditTrailService auditTrailService) : IEventService
{
    public async Task<List<EventResponse>> GetAllEventsAsync()
    {
        // Fetch all events
        var events =  await context.Events
            .Include(o => o.Location)
            .Select(e => Mapper.ToContract(e)).ToListAsync();
        
        // Audit log
        await auditTrailService.LogAsyncList(AuditAction.Read, AuditSubject.Event, events, null);
        
        // Return events
        return events;
    }
    
    public async Task<EventResponse?> GetEventByIdAsync(int id)
    {
        // Fetch event by id
        var eventEntity = await context.Events
            .Include(o => o.Location)
            .SingleOrDefaultAsync(o => o.Id == id);
        
        // Throw error if event is null
        if (eventEntity == null) throw new Exception("Event not found");
        
        // Audit log
        await auditTrailService.LogAsyncSingle(AuditAction.Read, AuditSubject.Event, eventEntity, null);
        
        // Return event
        return Mapper.ToContract(eventEntity);
    }

    public async Task<EventResponse> CreateEventAsync(EventRequest eventRequest)
    {
        // Make entity of request
        var eventEntity = Mapper.ToEntity(eventRequest , context);
        
        // Save entity in DB
        await context.Events.AddAsync(eventEntity);
        await context.SaveChangesAsync();

        // Audit log with rollback
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Create, AuditSubject.Event, null, eventEntity);
        }
        catch (Exception)
        {
            // Rollback
            context.Events.Remove(eventEntity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        // Return created event with ID
        return Mapper.ToContract(eventEntity);
    }

    public async Task<bool> UpdateEventAsync(int id, EventRequest eventRequest)
    {
        // Check if event exist
        var oldEventEntity = await context.Events
            .Include(o => o.Location)
            .SingleOrDefaultAsync(o => o.Id == id);
        if (oldEventEntity == null) throw new Exception("Event not found");
        
        // Make new entity
        var newEventEntity = oldEventEntity;
        
        // Check if location exist
        var eventRequestLocation = await context.Locations.FindAsync(eventRequest.LocationId);
        if (eventRequestLocation == null) throw new Exception("Location not found");
        
        // Updating values
        newEventEntity.Name = eventRequest.Name;
        newEventEntity.Start = eventRequest.Start;
        newEventEntity.End = eventRequest.End;
        newEventEntity.Location = eventRequestLocation;
        
        // Update the values
        context.Events.Update(newEventEntity);
        await context.SaveChangesAsync();
        
        // Audit log with rollback
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Update, AuditSubject.Event, oldEventEntity, newEventEntity);
        }
        catch (Exception)
        {
            // Rollback
            context.Events.Update(oldEventEntity);
            await context.SaveChangesAsync();            
            throw new Exception("Audit logging failed");
        }
        
        return true;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        // Check if event exist
        var eventEntity = await context.Events.FindAsync(id);
        if (eventEntity == null) throw new Exception("Event not found");
        
        // Delete event
        context.Events.Remove(eventEntity);
        await context.SaveChangesAsync();
        
        // Audit log
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Delete, AuditSubject.Event, eventEntity, null);
        }
        catch (Exception)
        {
            // Rollback
            context.Events.Add(eventEntity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        return true;
    }
}