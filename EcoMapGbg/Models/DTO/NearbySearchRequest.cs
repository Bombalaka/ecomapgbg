namespace EcoMapGbg.Models
{
    public class NearbySearchRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusKm { get; set; } = 5.0;
    public string? Type { get; set; }
    public bool? OnlyFree { get; set; }
}

}