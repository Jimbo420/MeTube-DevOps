using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MeTube_DevOps.UserManagement.Data;

var builder = WebApplication.CreateBuilder(args);

// Lägg till tjänster
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



