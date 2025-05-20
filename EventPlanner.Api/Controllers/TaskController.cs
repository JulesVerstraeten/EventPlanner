using EventPlanner.Api.Contracts.Task;
using EventPlanner.Domain.Enums;
using EventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;

namespace EventPlanner.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController(ITaskService service) : ControllerBase
{
    // Geef alle tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAllTasks()
    {
        try
        {
            var result = await service.GetAllTasks();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Geef task van aangegeven ID
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(int id)
    {
        try
        {
            var result = await service.GetTaskById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Maak task
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask(TaskRequest taskRequest)
    {
        try
        {
            var created  = await service.CreateTask(taskRequest);
            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update task
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponse>> UpdateTask(int id, [FromBody] TaskRequest taskRequest)
    {
        try
        {
            var updated = await service.UpdateTask(id, taskRequest);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Verwijder task
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<TaskResponse>> DeleteTask(int id)
    {
        try
        {
            var deleted = await service.DeleteTask(id);
            return Ok(deleted);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update status
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<TaskResponse>> PatchStatus(int id, [FromBody] Status newStatus)
    {
        try
        {
            var success = await service.PatchStatus(id, newStatus);
            if (!success) return NotFound($"Task with id {id} not found");
            return Ok(success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}