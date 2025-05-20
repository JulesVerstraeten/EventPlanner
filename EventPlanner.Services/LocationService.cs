using EventPlanner.Api.Contracts.Location;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence;
using EventPlanner.Persistence.Models;
using EventPlanner.Services.Extensions;
using EventPlanner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Services;

public class LocationService(EventContext context, IAuditTrailService auditTrailService) : ILocationService
{
    public async Task<List<LocationResponse>> GetAllLocationsAsync()
    {
        // Get locations from DB
        var entity = await context.Locations.Select(location => Mapper.ToContract(location)).ToListAsync();

        // Audit logger
        await auditTrailService.LogAsyncList(AuditAction.Read, AuditSubject.Location, entity, null);
        
        // Return locations
        return entity;
    }
    
    public async Task<LocationResponse?> GetLocationByIdAsync(int id)
    {
        // Get location from DB
        var entity = await context.Locations.FindAsync(id);
        
        // Audit logger
        await auditTrailService.LogAsyncSingle(AuditAction.Read, AuditSubject.Location, entity, null);
        
        // Return location
        return entity is null ? null : Mapper.ToContract(entity);
    }

    public async Task<LocationResponse> CreateLocationAsync(LocationRequest location)
    {
        // Map the input and add > save it to DB
        var entity = Mapper.ToEntity(location);
        await context.Locations.AddAsync(entity);
        await context.SaveChangesAsync();

        // Audit logger, if failed than the input will be deleted
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Create, AuditSubject.Location, null, entity);
        }
        catch (Exception ex)
        {
            // Rollback
            context.Locations.Remove(entity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        // Return data
        return Mapper.ToContract(entity);
    }

    public async Task<bool> UpdateLocationAsync(int id, LocationRequest location)
    {
        // Get the old data, make a new variable for the new data
        var oldEntity = await context.Locations.FindAsync(id);
        if (oldEntity is null) throw new Exception("Location not found");
        var newEntity = oldEntity;
        
        // Insert the new data in the new variable
        newEntity.Name = location.Name;
        newEntity.Description = location.Description;
        newEntity.GpsLat = location.GpsLat;
        newEntity.GpsLon = location.GpsLon;
        
        // Save changes
        context.Locations.Update(newEntity);
        await context.SaveChangesAsync();
        
        // Audit logger. If it failed it will rolback to the old data
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Update, AuditSubject.Location, oldEntity, newEntity);
        }
        catch (Exception ex)
        {
            // Rollback
            context.Locations.Update(oldEntity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }

        return true;
    }

    public async Task<bool> DeleteLocationAsync(int id)
    {
        // Get data from Id, if it doesn't exist its throw error
        var entity = await context.Locations.FindAsync(id);
        if (entity is null) throw new Exception("Location not found");
        
        // Remove data
        context.Locations.Remove(entity);
        await context.SaveChangesAsync();

        // Audit logger. If it failed it will rolback
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Delete, AuditSubject.Location, entity, null);
        }
        catch (Exception ex)
        {
            // Rollback
            await context.Locations.AddAsync(entity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        return true;
    }
}