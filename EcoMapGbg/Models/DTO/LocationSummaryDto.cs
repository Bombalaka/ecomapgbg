using System.ComponentModel.DataAnnotations;

namespace EcoMapGbg.Models.DTO;

    public class LocationSummaryDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string Type { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFree { get; set; }
    public double? DistanceKm { get; set; }
}
// This class represents a summary of a location, including its ID, name, address, type,
// latitude, longitude, whether it is free, and optionally the distance from a search point.