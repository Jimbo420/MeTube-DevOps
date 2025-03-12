using MeTube_DevOps.UserService.DTO;
using MeTube_DevOps.UserService.IntegrTests;
using Microsoft.Playwright;
using System.Text.Json;
using Xunit.Abstractions;
using MeTube_DevOps.UserService.Entities;
using FluentAssertions;

namespace MeTube_DevOps.UserService.IntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<PlaywrightFixture>
    {
        private readonly IAPIRequestContext _apiContext;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ITestOutputHelper _output;

        public UserControllerIntegrationTests(PlaywrightFixture fixture, ITestOutputHelper output)
        {
        _apiContext = fixture.ApiContext;
        _output = output;
        _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
        }

        [Fact]
        public async Task HttpGet_Should_Return_All_Users()
        {
            // Arrange
            // Add some user data to the database
            var expected1 = new User { Id = 1, Username = "user1", Email = "test1@mail.com", Password = "password1", Role = "User" };
            var expected2 = new User { Id = 2, Username = "user2", Email = "test2@mail.com", Password = "password2", Role = "User" };
            await _apiContext.PostAsync("/api/User/signup", new APIRequestContextOptions {DataObject = expected1});
            await _apiContext.PostAsync("/api/User/signup", new APIRequestContextOptions {DataObject = expected2});

            // Act
            var response = await _apiContext.GetAsync("/api/User/manageUsers");

            // Assert
            response.Status.Should().Be(200);

            var users = JsonSerializer.Deserialize<List<User>>(await );
            users.Should().NotBeNull();

        }
    }
}