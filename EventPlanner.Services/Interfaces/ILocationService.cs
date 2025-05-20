using EventPlanner.Api.Contracts.Location;

namespace EventPlanner.Services.Interfaces;

public interface ILocationService
{
    Task<List<LocationResponse>> GetAllLocationsAsync();
    Task<LocationResponse?> GetLocationByIdAsync(int id);
    Task<LocationResponse> CreateLocationAsync(LocationRequest location);
    Task<bool> UpdateLocationAsync(int id, LocationRequest location);
    Task<bool> DeleteLocationAsync(int id);
}