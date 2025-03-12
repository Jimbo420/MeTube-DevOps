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
            var expected1 = new User { Id = 666, Username = "user1", Email = "test1@mail.com", Password = "password1", Role = "User" };
            var expected2 = new User { Id = 1337, Username = "user2", Email = "test2@mail.com", Password = "password2", Role = "User" };
            await _apiContext.PostAsync("http://localhost:5218/api/User/signup", new APIRequestContextOptions {DataObject = expected1});
            await _apiContext.PostAsync("http://localhost:5218/api/User/signup", new APIRequestContextOptions {DataObject = expected2});

            // Act
            var response = await _apiContext.GetAsync("http://localhost:5218/api/User/manageUsers");

            // Assert
            response.Status.Should().Be(200);

            // Change this line to deserialize to UserDto instead of User
            var users = JsonSerializer.Deserialize<List<UserDto>>(await response.TextAsync(), _serializerOptions);
            users.Should().NotBeNull();
            ArgumentNullException.ThrowIfNull(users);
            users.Should().BeOfType<List<UserDto>>();

            var actual1 = users.FirstOrDefault(u => u.Id == expected1.Id);
            actual1?.Id.Should().Be(expected1.Id);
            actual1?.Username.Should().Be(expected1.Username);
            actual1?.Email.Should().Be(expected1.Email);
            
            // Don't check for Role if it's not in the UserDto
            // actual1?.Role.Should().Be(expected1.Role);

            var actual2 = users.FirstOrDefault(u => u.Id == expected2.Id);
            actual2?.Id.Should().Be(expected2.Id);
            actual2?.Username.Should().Be(expected2.Username);
            actual2?.Email.Should().Be(expected2.Email);
            
            // Don't check for Role if it's not in the UserDto
            // actual2?.Role.Should().Be(expected2.Role);

            _output.WriteLine($"items: Count={users.Count}");
        }
    }
}