using MongoDB.Driver;
using EcoMapGbg.Models;

namespace EcoMapGbg.Data;

public interface ILocationRepository
{
    Task<List<Location>> GetAllActiveAsync();
    Task<Location?> GetByIdAsync(string id);
    Task<Location> CreateAsync(Location location);
    Task<List<Location>> SearchNearbyAsync(double lat, double lng, double radiusKm);
}

public class MongoLocationRepository : ILocationRepository
{
    private readonly IMongoCollection<Location> _locations;

    public MongoLocationRepository(IMongoDatabase database)
    {
        _locations = database.GetCollection<Location>("locations");
    }

    public async Task<List<Location>> GetAllActiveAsync()
    {
        return await _locations.Find(l => l.IsActive).ToListAsync();
    }

    public async Task<Location?> GetByIdAsync(string id)
    {
        return await _locations.Find(l => l.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Location> CreateAsync(Location location)
    {
        await _locations.InsertOneAsync(location);
        return location;
    }

    public async Task<List<Location>> SearchNearbyAsync(double lat, double lng, double radiusKm)
    {
        var allLocations = await GetAllActiveAsync();
        return allLocations
            .Where(loc => loc.DistanceTo(lat, lng) <= radiusKm)
            .OrderBy(loc => loc.DistanceTo(lat, lng))
            .ToList();
    }
}