using MongoDB.Driver;
using EcoMapGbg.Models;
using EcoMapGbg.Models.Enum;
using System.Collections.Generic;

namespace EcoMapGbg.Data;


/// <summary>
/// MongoDB implementation of our location repository
/// This is our actual "librarian" who knows how to work with MongoDB
/// </summary>
public class MongoLocationRepository : ILocationRepository
{
    private readonly IMongoCollection<Location> _locations;
    private readonly ILogger<MongoLocationRepository> _logger;

    /// <summary>
    /// Constructor - Set up our MongoDB connection
    /// Like hiring our librarian and showing them where the books are stored
    /// </summary>
    public MongoLocationRepository(IMongoDatabase database, ILogger<MongoLocationRepository> logger)
    {
        _locations = database.GetCollection<Location>("locations");
        _logger = logger;
        
        // Create database indexes for better performance (like organizing library shelves)
        CreateIndexes();
    }

    // 📖 READING OPERATIONS

    public async Task<List<Location>> GetAllActiveAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all active locations");
            
            // Find all locations where IsActive = true
            return await _locations
                .Find(location => location.IsActive)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all active locations");
            throw; // Re-throw so the caller knows something went wrong
        }
    }

    public async Task<Location?> GetByIdAsync(string id)
    {
        try
        {
            _logger.LogInformation("Fetching location with ID: {LocationId}", id);
            
            return await _locations
                .Find(location => location.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching location with ID: {LocationId}", id);
            throw;
        }
    }

    public async Task<List<Location>> SearchNearbyAsync(double lat, double lng, double radiusKm)
    {
        try
        {
            _logger.LogInformation("Searching locations near {Lat}, {Lng} within {Radius}km", lat, lng, radiusKm);

            // Get all active locations
            var allLocations = await _locations
                .Find(location => location.IsActive)
                .ToListAsync();

            // Filter by distance and sort by distance
            return allLocations
                .Where(loc => loc.DistanceTo(lat, lng) <= radiusKm)
                .OrderBy(loc => loc.DistanceTo(lat, lng))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching nearby locations");
            throw;
        }
    }

    // ✏️ WRITING OPERATIONS

    public async Task<Location> CreateAsync(Location location)
    {
        try
        {
            _logger.LogInformation("Creating new location: {LocationName}", location.Name);
            
            // Set creation timestamp
            location.CreatedAt = DateTime.UtcNow;
            
            // Save to database
            await _locations.InsertOneAsync(location);
            
            _logger.LogInformation("Successfully created location with ID: {LocationId}", location.Id);
            return location;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating location: {LocationName}", location.Name);
            throw;
        }
    }

    // 📊 Database optimization
    
    /// <summary>
    /// Create database indexes for better performance
    /// Like organizing library sections for faster book finding
    /// </summary>
    private void CreateIndexes()
    {
        try
        {
            // Index for location-based queries (faster geographical searches)
            var geoIndexKeys = Builders<Location>.IndexKeys.Geo2DSphere(l => new { l.Latitude, l.Longitude });
            _locations.Indexes.CreateOneAsync(new CreateIndexModel<Location>(geoIndexKeys));

            // Index for type queries (faster filtering by location type)
            var typeIndexKeys = Builders<Location>.IndexKeys.Ascending(l => l.Type);
            _locations.Indexes.CreateOneAsync(new CreateIndexModel<Location>(typeIndexKeys));

            // Index for active status (faster queries for active locations)
            var activeIndexKeys = Builders<Location>.IndexKeys.Ascending(l => l.IsActive);
            _locations.Indexes.CreateOneAsync(new CreateIndexModel<Location>(activeIndexKeys));
            
            _logger.LogInformation("Database indexes created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not create database indexes (this is usually fine)");
        }
    }
}