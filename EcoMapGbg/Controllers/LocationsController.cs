using Microsoft.AspNetCore.Mvc;
using EcoMapGbg.Models;
using EcoMapGbg.Models.DTO;
using EcoMapGbg.Services;


namespace EcoMapGbg.Controllers;

/// <summary>
/// API Controller for location operations
/// Handles HTTP requests and calls the business logic service
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(ILocationService locationService, ILogger<LocationsController> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all active locations for the map
    /// GET /api/locations
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<Location>), 200)]
    public async Task<ActionResult<List<Location>>> GetAllLocations()
    {
        try
        {
            _logger.LogInformation("API: Getting all locations");
            
            var locations = await _locationService.GetAllLocationsAsync();
            
            return Ok(locations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API error: Failed to get all locations");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Get a specific location by ID
    /// GET /api/locations/{id}
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Location), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Location>> GetLocationById(string id)
    {
        try
        {
            _logger.LogInformation("API: Getting location {LocationId}", id);
            
            var location = await _locationService.GetLocationAsync(id);
            
            if (location == null)
            {
                return NotFound(new { error = "Location not found" });
            }
            
            return Ok(location);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("API: Invalid request for location {LocationId}: {Error}", id, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API error: Failed to get location {LocationId}", id);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new location
    /// POST /api/locations
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Location), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Location>> CreateLocation([FromBody] CreateLocationRequest request)
    {
        try
        {
            _logger.LogInformation("API: Creating new location {LocationName}", request.Name);
            
            var createdLocation = await _locationService.CreateLocationAsync(request);
            
            return CreatedAtAction(
                nameof(GetLocationById), 
                new { id = createdLocation.Id }, 
                createdLocation);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("API: Invalid create request for {LocationName}: {Error}", request.Name, ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API error: Failed to create location {LocationName}", request.Name);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Search for nearby locations
    /// GET /api/locations/search?latitude=57.7&longitude=11.9&radiusKm=5
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<LocationSummaryDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<List<LocationSummaryDto>>> SearchNearby([FromQuery] NearbySearchRequest request)
    {
        try
        {
            _logger.LogInformation("API: Searching nearby locations at {Lat}, {Lng}", 
                request.Latitude, request.Longitude);
            
            var locations = await _locationService.SearchNearbyAsync(request);
            
            return Ok(locations);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("API: Invalid search request: {Error}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API error: Failed to search nearby locations");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Health check endpoint
    /// GET /api/locations/health
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(200)]
    public IActionResult Health()
    {
        return Ok(new 
        { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            service = "EcoMapGBG Locations API"
        });
    }
}