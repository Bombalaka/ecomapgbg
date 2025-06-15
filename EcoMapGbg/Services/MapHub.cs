using Microsoft.AspNetCore.SignalR;

namespace EcoMapGbg.Services;

/// <summary>
/// SignalR hub for real-time map updates
/// Like WhatsApp for your map - instant updates when someone adds a location!
/// </summary>
public class MapHub : Hub
{
    private readonly ILogger<MapHub> _logger;

    public MapHub(ILogger<MapHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// User joins the map group to receive updates
    /// </summary>
    public async Task JoinMapGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "MapUsers");
        _logger.LogInformation("User {ConnectionId} joined map group", Context.ConnectionId);
    }

    /// <summary>
    /// User leaves the map group  
    /// </summary>
    public async Task LeaveMapGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "MapUsers");
        _logger.LogInformation("User {ConnectionId} left map group", Context.ConnectionId);
    }

    /// <summary>
    /// Called when user disconnects
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User {ConnectionId} disconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}