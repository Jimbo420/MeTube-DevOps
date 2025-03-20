using Scalar.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections;
using System.Globalization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Disable accessing configuration for GATEWAY_PORT which causes the exception
builder.Configuration["GATEWAY_PORT"] = null;

// Configure microservice connections
string USERSERVICE_SCHEME = "http";
string USERSERVICE_HOST = "metube-user";
int USERSERVICE_PORT = 5000; // Match UserService port

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add core services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Configure HttpClient for UserService
builder.Services.AddHttpClient("UserServiceClient", client => {
    client.BaseAddress = new Uri($"{USERSERVICE_SCHEME}://{USERSERVICE_HOST}:{USERSERVICE_PORT}/");
    // Add timeout to prevent long-hanging requests
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Configure CORS policy
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Build the application
var app = builder.Build();

// Configure middleware pipeline
app.UseCors("AllowAll");

// Handle OPTIONS requests for CORS preflight
app.Use(async (context, next) => {
    if (context.Request.Method == "OPTIONS") {
        // Use indexer to set headers (fixes ASP0019 warning)
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
        context.Response.StatusCode = 200;
        return;
    }
    await next();
});

// Configure development-specific middleware
if (app.Environment.IsDevelopment()) {
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

// Add diagnostic endpoints
app.MapGet("/", () => "The MeTube Gateway is up and running!");
app.MapGet("/health", () => new { 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    Service = "MeTube Gateway"
});

// SIMPLIFIED PORT HANDLING - avoids using Configuration which causes the error
int GATEWAY_PORT = 8080; // Default port

try {
    // ONLY use Environment.GetEnvironmentVariable - avoid app.Configuration
    var metubePart = Environment.GetEnvironmentVariable("METUBE_GATEWAY_PORT");
    Console.WriteLine($"METUBE_GATEWAY_PORT value: {metubePart ?? "not set"}");
    
    if (!string.IsNullOrEmpty(metubePart) && int.TryParse(metubePart, out int parsedPort)) {
        GATEWAY_PORT = parsedPort;
        Console.WriteLine($"Using METUBE_GATEWAY_PORT: {GATEWAY_PORT}");
    }
}
catch (Exception ex) {
    Console.WriteLine($"Error parsing port configuration: {ex.Message}");
    Console.WriteLine($"Using default port: {GATEWAY_PORT}");
}

Console.WriteLine($"Gateway service starting on port {GATEWAY_PORT}");
app.Run($"http://0.0.0.0:{GATEWAY_PORT}");