using EcoMapGbg.Components;
using MongoDB.Driver;
using EcoMapGbg.Data;
using EcoMapGbg.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add basic logging
builder.Services.AddLogging();

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EcoMapGBG API", Version = "v1" });
});

builder.Services.AddSignalR();

// MongoDB setup
var connectionString = builder.Configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27020";
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddScoped<IMongoDatabase>(sp => 
    sp.GetRequiredService<IMongoClient>().GetDatabase("ecomapgbg"));

// Register your services
builder.Services.AddScoped<ILocationRepository, MongoLocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    // Enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoMapGBG API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAntiforgery();

app.MapStaticAssets();
//  Configure Blazor and API Endpoints
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add API endpoints
app.MapControllers();
//  Map SignalR Hub
app.MapHub<MapHub>("/maphub");

// Simple API root
app.MapGet("/api", () => new
{
    message = "🌱 EcoMapGBG API is running!",
    blazor = "/",
    swagger = "/swagger",
    api = "/api/locations"
});

app.Run();