using EventPlanner.Api.Contracts.Event;

namespace EventPlanner.Services.Interfaces;

public interface IEventService
{
    Task<EventTasksResponse> GetAllTasksFromEventId(int eventId);
    Task<List<EventResponse>> GetAllEventsAsync();
    Task<EventResponse?> GetEventByIdAsync(int id);
    Task<EventResponse> CreateEventAsync(EventRequest eventRequest);
    Task<bool> UpdateEventAsync(int id, EventRequest eventRequest);
    Task<bool> DeleteEventAsync(int id);
}