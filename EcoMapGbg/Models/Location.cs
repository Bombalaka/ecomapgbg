using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoMapGbg.Models;

public class Location
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = "";

    [Required]
    public string Type { get; set; } = "";

    public string Description { get; set; } = "";

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsFree { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Simple distance calculation
    public double DistanceTo(double lat, double lng)
    {
        const double R = 6371; // Earth's radius in km
        var dLat = ToRadians(lat - Latitude);
        var dLng = ToRadians(lng - Longitude);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(lat)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;

    public enum LocationType
{
    SecondHand,
    FreeShop,
    RecyclingCenter,
    BikeWorkshop,
    RepairCafe,
    ClothingSwap
}

}