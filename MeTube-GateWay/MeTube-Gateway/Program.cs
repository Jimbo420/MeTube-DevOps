using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//Test4

// Make sure the necessary environment variables are available.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("METUBE_GATEWAY_PORT"))) {
    throw new Exception("Please specify the port number for METUBE.Gateway with the environment variable METUBE_GATEWAY_PORT.");
}

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("METUBE_USERSERVICE_SCHEME"))) {
    throw new Exception("Please specify the scheme for METUBE.USERSERVICE with the environment variable METUBE_USERSERVICE_SCHEME.");
}

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("METUBE_USERSERVICE_HOST"))) {
    throw new Exception("Please specify the host for METUBE.USERSERVICE with the environment variable METUBE_USERSERVICE_HOST.");
}

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("METUBE_USERSERVICE_PORT"))) {
    throw new Exception("Please specify the port number for METUBE.USERSERVICE with the environment variable METUBE_USERSERVICE_PORT.");
}


string USERSERVICE_SCHEME = Environment.GetEnvironmentVariable("METUBE_USERSERVICE_SCHEME") ?? string.Empty;
string USERSERVICE_HOST = Environment.GetEnvironmentVariable("METUBE_USERSERVICE_HOST") ?? string.Empty;
int USERSERVICE_PORT = int.Parse(Environment.GetEnvironmentVariable("METUBE_USERSERVICE_PORT") ?? "8080");

builder.Configuration.AddEnvironmentVariables("METUBE_");

//string USERSERVICE_SCHEME = "http"; // Använd alltid http om du inte har https konfigurerat
//string USERSERVICE_HOST = "metube-user"; // Använd container-namnet istället för localhost
//int USERSERVICE_PORT = 8080; // Internporten i containern

builder.Services.AddHttpClient("UserServiceClient", client => {
    client.BaseAddress = new Uri($"{USERSERVICE_SCHEME}://{USERSERVICE_HOST}:{USERSERVICE_PORT}");
    // Timeout 
    client.Timeout = TimeSpan.FromSeconds(30);
});


// Configure logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });
    });

var app = builder.Build();
app.UseCors("AllowAll");
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 200;
        return;
    }
    await next();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi();
}

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
int GATEWAY_PORT = app.Configuration.GetValue<int>("GATEWAY_PORT");
//int GATEWAY_PORT = 8080;

Console.WriteLine($"Microservice online and listening on port {GATEWAY_PORT}.");
app.Run($"http://0.0.0.0:{GATEWAY_PORT}");