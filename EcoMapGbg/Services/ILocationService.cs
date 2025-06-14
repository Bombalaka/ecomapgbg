using System.Threading.Tasks;
using EcoMapGbg.Models.Enum;
using System.Collections.Generic;
using EcoMapGbg.Models;
using EcoMapGbg.Models.DTO;


namespace EcoMapGbg.Services;



/// <summary>
/// Business logic contract for location operations
/// The smart manager who knows all the business rules!
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Get all locations for displaying on the map
    /// </summary>
    Task<List<Location>> GetAllLocationsAsync();
    
    /// <summary>
    /// Get one specific location with full details
    /// </summary>
    Task<Location?> GetLocationAsync(string id);
    
    /// <summary>
    /// Create a new location with business validation
    /// </summary>
    Task<Location> CreateLocationAsync(CreateLocationRequest request);
    
    /// <summary>
    /// Search nearby locations and return summaries
    /// </summary>
    Task<List<LocationSummaryDto>> SearchNearbyAsync(NearbySearchRequest request);
}