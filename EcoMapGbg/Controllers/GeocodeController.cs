using System.Text.Json;
using EcoMapGbg.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EcoMapGbg.Controllers;

/// <summary>
/// Proxies address search to OpenStreetMap Nominatim (respects their usage policy: identify app via User-Agent).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GeocodeController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GeocodeController> _logger;
    private const string NominatimSearch = "https://nominatim.openstreetmap.org/search";

    public GeocodeController(IHttpClientFactory httpClientFactory, ILogger<GeocodeController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Geocode a free-text address (bias Sweden for Göteborg-focused app).
    /// GET /api/geocode/search?q=Vasagatan+10+Göteborg
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(GeocodeResultDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GeocodeResultDto>> Search([FromQuery] string q, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new { error = "Parameter 'q' (address) is required" });
        }

        try
        {
            var client = _httpClientFactory.CreateClient("Nominatim");
            var encoded = Uri.EscapeDataString(q.Trim());
            // Absolute URL avoids BaseAddress + relative URI quirks on some systems
            var url =
                $"{NominatimSearch}?q={encoded}&format=json&limit=5&countrycodes=se&addressdetails=1";

            using var response = await client.GetAsync(url, cancellationToken);

            if ((int)response.StatusCode == 429)
            {
                return StatusCode(429, new { error = "Too many searches. Wait a few seconds (Nominatim limit)." });
            }

            if (!response.IsSuccessStatusCode)
            {
                var errBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning("Nominatim HTTP {Status}: {Body}", response.StatusCode,
                    errBody.Length > 200 ? errBody[..200] : errBody);
                return StatusCode(502, new { error = "Geocoding service returned an error. Try again later." });
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound(new { error = "No results for that address. Add city, e.g. Goteborg." });
            }

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
            {
                // Retry without country filter (still ASCII query OK)
                var url2 =
                    $"{NominatimSearch}?q={encoded}&format=json&limit=5&addressdetails=1";
                using var response2 = await client.GetAsync(url2, cancellationToken);
                if (!response2.IsSuccessStatusCode)
                {
                    return NotFound(new { error = "No results for that address. Try adding city, e.g. Goteborg." });
                }

                var json2 = await response2.Content.ReadAsStringAsync(cancellationToken);
                using var doc2 = JsonDocument.Parse(json2);
                var root2 = doc2.RootElement;
                if (root2.ValueKind != JsonValueKind.Array || root2.GetArrayLength() == 0)
                {
                    return NotFound(new { error = "No results for that address. Try adding city, e.g. Goteborg." });
                }

                return Ok(MakeResult(root2[0], q));
            }

            return Ok(MakeResult(root[0], q));
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
        {
            return StatusCode(499, new { error = "Request cancelled" });
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("Nominatim request timed out");
            return StatusCode(504, new { error = "Geocoding timed out. Check your internet connection." });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Nominatim HTTP connection failed");
            return StatusCode(502, new { error = "Could not reach geocoding service. Check firewall / internet." });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Nominatim JSON parse failed");
            return StatusCode(502, new { error = "Invalid response from geocoding service." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Geocode failed for query");
            return StatusCode(500, new { error = "Geocoding failed. Check API logs." });
        }
    }

    private static GeocodeResultDto MakeResult(JsonElement first, string q)
    {
        var lat = double.Parse(first.GetProperty("lat").GetString()!, System.Globalization.CultureInfo.InvariantCulture);
        var lon = double.Parse(first.GetProperty("lon").GetString()!, System.Globalization.CultureInfo.InvariantCulture);
        var display = first.TryGetProperty("display_name", out var dn)
            ? dn.GetString() ?? q
            : q;

        return new GeocodeResultDto
        {
            Latitude = lat,
            Longitude = lon,
            DisplayName = display,
        };
    }
}
