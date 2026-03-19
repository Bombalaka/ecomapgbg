namespace EcoMapGbg.Models.DTO;

/// <summary>First geocoding hit for a search query (OpenStreetMap Nominatim).</summary>
public class GeocodeResultDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string DisplayName { get; set; } = "";
}
