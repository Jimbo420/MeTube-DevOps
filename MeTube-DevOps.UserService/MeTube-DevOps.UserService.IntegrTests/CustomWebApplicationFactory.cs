using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MeTube_DevOps.UserService;
using MeTube_DevOps.UserService.Data;
using MeTube_DevOps.UserService.Entities;
using Microsoft.Extensions.Hosting;

namespace MeTube_DevOps.UserService.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Use test environment
            builder.UseEnvironment("Testing");
            
            // Replace the regular DB context with an in-memory one
            builder.ConfigureServices(services =>
            {
                // First, find and remove ALL database-related registrations
                var dbContextDescriptors = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                               d.ServiceType == typeof(DbContextOptions) ||
                               d.ServiceType == typeof(ApplicationDbContext) ||
                               (d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true))
                    .ToList();

                foreach (var descriptor in dbContextDescriptors)
                {
                    services.Remove(descriptor);
                }
                
                // Remove ANY SQL Server specific services
                var sqlServerDescriptors = services
                    .Where(d => d.ServiceType.FullName?.Contains("SqlServer") == true)
                    .ToList();
                    
                foreach (var descriptor in sqlServerDescriptors)
                {
                    services.Remove(descriptor);
                }

                // Add new in-memory database
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add ApplicationDbContext using in-memory provider
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDB");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build provider to initialize database
                var sp = services.BuildServiceProvider();

                // Create scope to get services
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                    try
                    {
                        // Ensure the database is created and seed with test data
                        InitializeDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database for tests.");
                    }
                }
            });
        }

        private void InitializeDatabase(ApplicationDbContext dbContext)
        {
            // Clear existing data
            dbContext.Users.RemoveRange(dbContext.Users);
            dbContext.SaveChanges();
            
            // Add test data
            var user1 = new User { Id = 1, Username = "user1", Email = "example", Role = "User", Password = "password1" };
            var user2 = new User { Id = 2, Username = "user2", Email = "example", Role = "User", Password = "password2" };

            dbContext.Users.Add(user1);
            dbContext.Users.Add(user2);
            dbContext.SaveChanges();
        }
    }
}