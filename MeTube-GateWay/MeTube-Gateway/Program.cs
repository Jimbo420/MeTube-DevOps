using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

string USERSERVICE_SCHEME = Environment.GetEnvironmentVariable("METUBE_USERSERVICE_SCHEME") ?? string.Empty;
string USERSERVICE_HOST = Environment.GetEnvironmentVariable("METUBE_USERSERVICE_HOST") ?? string.Empty;
int USERSERVICE_PORT = int.Parse(Environment.GetEnvironmentVariable("METUBE_USERSERVICE_PORT") ?? "80");

builder.Configuration.AddEnvironmentVariables("METUBE_");

// Configure logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add HTTP clients for the various microservices to the container.
builder.Services.AddHttpClient("UserServiceClient", client => { client.BaseAddress = new Uri($"{USERSERVICE_SCHEME}://{USERSERVICE_HOST}:{USERSERVICE_PORT}"); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add a root endpoint to show that the gateway is running
app.MapGet("/", () => "The MeTube Gateway is up and running!");

// Add a more detailed health check endpoint
app.MapGet("/health", () => new { 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    Service = "MeTube Gateway"
});

// Fix: Use the correct configuration key for the port
int GATEWAY_PORT = app.Configuration.GetValue<int>("GATEWAY_PORT", 8080);
Console.WriteLine($"Microservice online and listening on port {GATEWAY_PORT}.");
app.Run($"http://0.0.0.0:{GATEWAY_PORT}");