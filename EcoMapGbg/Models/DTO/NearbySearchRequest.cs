namespace EcoMapGbg.Models.DTO;

public class NearbySearchRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusKm { get; set; } = 5.0;
    public string? Type { get; set; }
    public bool? OnlyFree { get; set; }
    public int Limit { get; set; } = 50;
    
}

// This class represents a request for searching nearby locations.
// It includes properties for latitude, longitude, radius, type of location,