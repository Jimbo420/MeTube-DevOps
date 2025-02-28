using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MeTube_DevOps.UserManagement.Data;
using MeTube_DevOps.UserManagement.Repositories;
using MeTube_DevOps.UserManagement.UserProfile;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables
int PORT = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "80");

// Lägg till tjänster
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Add unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add profiles
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);


// Lägg till databaskoppling
// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString));

// var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// if (string.IsNullOrEmpty(connectionString))
// {
//     throw new InvalidOperationException("DB_CONNECTION_STRING is not set.");
// }
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//Console.WriteLine($"PORT: {PORT}");
//app.Run($"http://localhost:{PORT}");

app.Run();



