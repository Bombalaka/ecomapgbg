using System.ComponentModel.DataAnnotations;

namespace EcoMapGbg.Models
{
    public class CreateLocationRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = "";

    [Required]
    public string Type { get; set; } = "";

    public string Description { get; set; } = "";

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public bool IsFree { get; set; }

    public List<string> Tags { get; set; } = new();
}
}