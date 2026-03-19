using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
using EcoMapGbg.Data;
using EcoMapGbg.Services;
using DotNetEnv;

/*
 * .env / Env.Load (why & when)
 * -----------------------------
 * A file named `.env` is NOT magic to .NET by itself. The package DotNetEnv reads that file
 * and copies each line into **process environment variables** (same as export VAR=value in Linux).
 *
 * Then `builder.Configuration.GetConnectionString("MongoDB")` reads the variable
 * `ConnectionStrings__MongoDB` (two underscores = nested section "ConnectionStrings").
 *
 * `Env.Load()` with no path only looks in the **current working folder** — when you Run from
 * Visual Studio/Cursor that folder is often `EcoMapGbg/bin/Debug/net9.0`, so your repo-root `.env`
 * was never found. We walk up directories until we find `.env`.
 */
LoadDotEnvFromAncestors();

var builder = WebApplication.CreateBuilder(args);

/* Railway / Fly / etc. set PORT — ASP.NET must listen on it (not only 80). */
var railwayPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(railwayPort))
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");

static void LoadDotEnvFromAncestors()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null)
    {
        var path = Path.Combine(dir.FullName, ".env");
        if (File.Exists(path))
        {
            Env.Load(path);
            return;
        }
        dir = dir.Parent;
    }
}

/* So UseHttps + links work behind Railway’s TLS terminator (X-Forwarded-Proto). */
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

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

app.UseForwardedHeaders();

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
