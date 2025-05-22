using EventPlanner.Api.Contracts.Event;
using EventPlanner.Api.Contracts.Task;
using EventPlanner.Domain.Enums;

namespace EventPlanner.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskResponse>> GetAllTasks();
    Task<TaskResponse?> GetTaskById(int id);
    Task<TaskResponse> CreateTask(TaskRequest task);
    Task<bool> UpdateTask(int id, TaskRequest taskRequest);
    Task<bool> DeleteTask(int id);
    Task<bool> PatchStatus(int id, Status newStatus);
}