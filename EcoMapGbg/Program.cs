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
builder.Services.AddSignalR();

// Nominatim — User-Agent must be ASCII and identify the app (no non-ASCII: can break headers)
// https://operations.osmfoundation.org/policies/nominatim/
builder.Services.AddHttpClient("Nominatim", (_, client) =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.DefaultRequestHeaders.TryAddWithoutValidation(
        "User-Agent",
        "EcoMapGbg/1.0 (https://github.com/ecomapgbg; contact via repo)");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    client.Timeout = TimeSpan.FromSeconds(20);
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EcoMapGBG API", Version = "v1" });
});

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

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoMapGBG API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors();

// Serve SPA static files (Vue build output in wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

// API endpoints
app.MapControllers();
app.MapHub<MapHub>("/maphub");

// SPA fallback: serve index.html for client routes
app.MapFallbackToFile("index.html");

app.Run();
