using System.Text.Json;
using EventPlanner.Api.Contracts.Location;
using EventPlanner.Domain.Enums;
using EventPlanner.Persistence.MongoDb.Models;
using EventPlanner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventPlanner.Api.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController(ILocationService service) : ControllerBase
{
    // Geef alle locaties mee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationResponse>>> GetAllLocations()
    {
        try
        {
            var result = await service.GetAllLocationsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Geeft locatie van aangegeven ID mee
    [HttpGet("{id}")]
    public async Task<ActionResult<LocationResponse>> GetLocation([FromRoute] int id)
    {
        try
        {
            var location = await service.GetLocationByIdAsync(id);
            if (location == null) return NotFound();
            return Ok(location);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Maakt locatie aan
    [HttpPost]
    public async Task<ActionResult<LocationResponse>> CreateLocation(LocationRequest location)
    {
        try
        {
            
            var created = await service.CreateLocationAsync(location);
            return CreatedAtAction(nameof(GetLocation), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update locatie
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLocation([FromRoute] int id, [FromBody] LocationRequest location)
    {
        try
        {
            await service.UpdateLocationAsync(id, location);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Delete locatie
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLocation([FromRoute] int id)
    {
        try
        {
            await service.DeleteLocationAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}