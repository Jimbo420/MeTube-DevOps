using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using MeTube_DevOps.UserService.DTO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MeTube_DevOps.UserService.IntegrationTests
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UserControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOk_AndUsersList()
        {
            // Act
            var response = await _client.GetAsync("/api/User/manageUsers");
            
            // Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
            Assert.NotNull(users);
            Assert.Contains(users, u => u.Username == "user1");
            Assert.Contains(users, u => u.Username == "user2");
        }

        [Fact]
        public async Task GetUser_WithValidId_ReturnsOk_AndUser()
        {
            // Act
            var response = await _client.GetAsync("/api/User/1");
            
            // Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal("user1", user.Username);
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/User/999");
            
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SignUp_WithValidData_ReturnsOk()
        {
            // Arrange
            var newUser = new CreateUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "TestPassword123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client.PostAsync("/api/User/signup", content);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User signed up successfully", responseString);
        }

        [Fact]
        public async Task SignUp_WithExistingUsername_ReturnsBadRequest()
        {
            // Arrange
            var newUser = new CreateUserDto
            {
                Username = "user1", // Already exists in test db
                Email = "new@example.com",
                Password = "TestPassword123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client.PostAsync("/api/User/signup", content);
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Username already exists", responseString);
        }

        [Fact]
        public async Task SignUp_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var newUser = new CreateUserDto
            {
                Username = "brandnewuser",
                Email = "example", // Already exists in test db
                Password = "TestPassword123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            
            // Act
            var response = await _client.PostAsync("/api/User/signup", content);
            
            // Assert - only check status code, don't depend on message format
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}