using EcoMapGbg.Models;
using EcoMapGbg.Models.DTO;
using EcoMapGbg.Data;


namespace EcoMapGbg.Services;

/// <summary>
/// Simple business logic for location operations
/// </summary>
public class LocationService : ILocationService
{
    private readonly ILocationRepository _repository;
    private readonly ILogger<LocationService> _logger;

    public LocationService(ILocationRepository repository, ILogger<LocationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<List<Location>> GetAllLocationsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all locations");
            return await _repository.GetAllActiveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all locations");
            throw;
        }
    }

    public async Task<Location?> GetLocationAsync(string id)
    {
        try
        {
            _logger.LogInformation("Fetching location with ID: {LocationId}", id);
            
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Location ID cannot be empty");
            }

            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting location {LocationId}", id);
            throw;
        }
    }

    public async Task<Location> CreateLocationAsync(CreateLocationRequest request)
    {
        try
        {
            _logger.LogInformation("Creating new location: {LocationName}", request.Name);

            // Simple validation
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Location name is required");

            if (string.IsNullOrWhiteSpace(request.Address))
                throw new ArgumentException("Location address is required");

            // Convert DTO to Entity - ONLY existing properties
            var location = new Location
            {
                Name = request.Name.Trim(),
                Address = request.Address.Trim(),
                Type = request.Type,
                Description = request.Description?.Trim() ?? "",
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsFree = request.IsFree,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Save to database
            var createdLocation = await _repository.CreateAsync(location);
            
            _logger.LogInformation("Successfully created location: {LocationName}", location.Name);

            return createdLocation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating location {LocationName}", request.Name);
            throw;
        }
    }

    public async Task<List<LocationSummaryDto>> SearchNearbyAsync(NearbySearchRequest request)
    {
        try
        {
            _logger.LogInformation("Searching nearby locations at {Lat}, {Lng} within {Radius}km", 
                request.Latitude, request.Longitude, request.RadiusKm);

            // Simple validation
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.RadiusKm <= 0 || request.RadiusKm > 50)
                throw new ArgumentException("Search radius must be between 0 and 50 kilometers");

            // Get locations from repository
            var locations = await _repository.SearchNearbyAsync(
                request.Latitude, 
                request.Longitude, 
                request.RadiusKm);

            // Convert to DTOs - simple approach
            var summaries = locations
                .Take(50)
                .Select(location => new LocationSummaryDto
                {
                    Id = location.Id!,
                    Name = location.Name,
                    Address = location.Address,
                    Type = location.Type,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    IsFree = location.IsFree,
                    DistanceKm = location.DistanceTo(request.Latitude, request.Longitude)
                })
                .ToList();

            _logger.LogInformation("Found {Count} nearby locations", summaries.Count);
            return summaries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching nearby locations");
            throw;
        }
    }
}