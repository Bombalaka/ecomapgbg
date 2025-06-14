using System.ComponentModel.DataAnnotations;

namespace EcoMapGbg.Models
{
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
}