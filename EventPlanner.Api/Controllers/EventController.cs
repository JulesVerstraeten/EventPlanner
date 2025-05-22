using EventPlanner.Api.Contracts.Event;
using EventPlanner.Api.Contracts.Task;
using EventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController(IEventService eventService) : ControllerBase
{
    // Geeft Alle Events Mee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents()
    {
        try
        {
            var events = await eventService.GetAllEventsAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Geeft Event van aangegeven ID mee
    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEvent([FromRoute] int id)
    {
        try
        {
            var eventResponse = await eventService.GetEventByIdAsync(id);
            if (eventResponse == null) return NotFound();
            return Ok(eventResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Maakt Event Aan
    [HttpPost]
    public async Task<ActionResult<EventResponse>> CreateEvent(EventRequest eventRequest)
    {
        try
        {
            var eventResponse = await eventService.CreateEventAsync(eventRequest);
            return CreatedAtAction(nameof(GetEvent), new { id = eventResponse.Id }, eventResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update Event
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEvent([FromRoute] int id, [FromBody] EventRequest eventRequest)
    {
        try
        {
            var eventResponse = await eventService.UpdateEventAsync(id, eventRequest);
            return Ok(eventResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // Delete Event
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent([FromRoute] int id)
    {
        try
        {
            await eventService.DeleteEventAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    // Geef alle task van event mee
    [HttpGet("{eventId}/tasks")]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksFromEvent(int eventId)
    {
        try
        {
            var result = await eventService.GetAllTasksFromEventId(eventId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}