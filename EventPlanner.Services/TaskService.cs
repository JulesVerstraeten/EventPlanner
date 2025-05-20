using EventPlanner.Api.Contracts.Task;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence;
using EventPlanner.Services.Extensions;
using EventPlanner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Services;

public class TaskService(EventContext context, IAuditTrailService auditTrailService) : ITaskService
{
    public async Task<List<TaskResponse>> GetAllTasksFromEventId(int eventId)
    {
        // Get all tasks from event
        var tasks = await context.Tasks
            .Include(x => x.Event)
            .Include(x => x.Event.Location)
            .Where(x => x.Event.Id == eventId)
            .Select(o => Mapper.ToContract(o))
            .ToListAsync(); 
        if (tasks == null) throw new Exception("Event not found");
        
        // Audit logger with exception
        try
        {
            await auditTrailService.LogAsyncList(AuditAction.Read, AuditSubject.Task, tasks, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Audit logging failed");
        }
        
        // Return all tasks from event
        return tasks;
    }
    
    public async Task<List<TaskResponse>> GetAllTasks()
    {
        // Get all tasks from DB
        var tasks = await context.Tasks
            .Include(x => x.Event)
            .Include(x => x.Event.Location)
            .Select(o => Mapper.ToContract(o))
            .ToListAsync();
        
        // Audit logger
        try
        {
            await auditTrailService.LogAsyncList(AuditAction.Read, AuditSubject.Task, tasks, null);
        }
        catch (Exception ex)
        {
            throw new Exception($"Audit logging failed: {ex}", ex);
        }
        
        // Return tasks
        return tasks;
    }

    public async Task<TaskResponse?> GetTaskById(int id)
    {
        // Get task from DB
        var taskEntity = await context.Tasks
            .Include(t => t.Event)
            .Include(e => e.Event.Location)
            .SingleOrDefaultAsync(task => task.Id == id);
        if (taskEntity == null) throw new Exception("Task not found");
        
        // Audit logger
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Read, AuditSubject.Task, taskEntity, null);
        }
        catch (Exception)
        {
            throw new Exception("Audit logging failed");
        }
        
        // Return task
        return Mapper.ToContract(taskEntity);
    }

    public async Task<TaskResponse> CreateTask(TaskRequest task)
    {
        // Mapping request
        var entity = Mapper.ToEntity(task, context);
        
        // Save task in DB
        await context.Tasks.AddAsync(entity);
        await context.SaveChangesAsync();

        // Audit logger
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Create, AuditSubject.Task, null, entity);
        }
        catch (Exception)
        {
            context.Tasks.Remove(entity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        // Return task with ID
        return Mapper.ToContract(entity);
    }

    public async Task<bool> UpdateTask(int id, TaskRequest taskRequest)
    {
        // Dummy taskEntity mapped from task
        var dummyTaskEntity = Mapper.ToEntity(taskRequest, context);
        
        // Get old entity from DB with ID
        var oldTaskEntity = await context.Tasks.FindAsync(id);
        if (oldTaskEntity == null) throw new Exception("Task not found");
        
        // Set new entity
        var newTaskEntity = oldTaskEntity;
        
        // Check if event from request exist
        var taskRequestEvent = await context.Events.FindAsync(taskRequest.EventId);
        if (taskRequestEvent == null) throw new Exception("Event not found");
        
        // Update values
        newTaskEntity.Name = taskRequest.Name;
        newTaskEntity.Description = taskRequest.Description;
        newTaskEntity.Importance = dummyTaskEntity.Importance;
        newTaskEntity.Status = dummyTaskEntity.Status;
        newTaskEntity.Event = taskRequestEvent;
        
        // Updaten in DB
        context.Tasks.Update(newTaskEntity);
        await context.SaveChangesAsync();
        
        // Audit logger
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Update, AuditSubject.Task, oldTaskEntity, newTaskEntity);
        }
        catch (Exception)
        {
            context.Tasks.Update(oldTaskEntity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        return true;
    }

    public async Task<bool> DeleteTask(int id)
    {
        // Check if task exist
        var entity = await context.Tasks.FindAsync(id);
        if (entity == null) throw new Exception("Task not found");
        
        // Delete task
        context.Tasks.Remove(entity);
        await context.SaveChangesAsync();
        
        // Audit logger
        try
        {
            await auditTrailService.LogAsyncSingle(AuditAction.Delete, AuditSubject.Task, entity, null);
        }
        catch (Exception)
        {
            // Rollback
            await context.Tasks.AddAsync(entity);
            await context.SaveChangesAsync();
            throw new Exception("Audit logging failed");
        }
        
        return true;
    }

    public async Task<bool> PatchStatus(int id, Status newStatus)
    {
        var entity = await context.Tasks.FindAsync(id);
        if (entity == null) throw new Exception("Task not found");
        
        entity.Status = newStatus;
        
        await context.SaveChangesAsync();
        
        return true;
    }
}