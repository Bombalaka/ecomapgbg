
using EcoMapGbg.Models;
using System.Collections.Generic;

namespace EcoMapGbg.Data;

public interface ILocationRepository
{
    Task<List<Location>> GetAllActiveAsync();
    Task<Location?> GetByIdAsync(string id);
    Task<Location> CreateAsync(Location location);
    Task<List<Location>> SearchNearbyAsync(double lat, double lng, double radiusKm);
}