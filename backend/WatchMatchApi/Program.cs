using Microsoft.AspNetCore.SignalR;
using System.Net;
using TMDbLib.Client;
using WatchMatchApi.ApiClients;
using WatchMatchApi.Hubs;
using WatchMatchApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxt", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.AddFilter<RoomHubFilter>();
});

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["TMDB:API_KEY"];

    string? proxyUrl =
        config["TMDB:PROXY"] ??
        Environment.GetEnvironmentVariable("TMDB_PROXY");

    IWebProxy? proxy = null;
    if (!string.IsNullOrWhiteSpace(proxyUrl))
    {
        proxy = new WebProxy(proxyUrl)
        {
            BypassProxyOnLocal = true
        };
    }

    return new TMDbClient(
        apiKey,
        proxy: proxy
    );
});

builder.Services.AddSingleton<TMDbApi>();
builder.Services.AddSingleton<MovieService>();
builder.Services.AddSingleton<RoomService>();
builder.Services.AddSingleton(new DelayedActionScheduler(TimeSpan.FromSeconds(30)));
builder.Services.AddSingleton<GroupTrackerService>();
builder.Services.AddSingleton<IUserIdProvider, GuestUserIdProvider>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowNuxt");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<RoomHub>("/room-hub");

app.Run();
