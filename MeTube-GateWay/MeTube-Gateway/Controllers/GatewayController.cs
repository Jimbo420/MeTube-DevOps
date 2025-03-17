using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MeTube.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MeTube.Gateway.Controllers;

[ApiController]
[Route("gateway")]
public class GatewayController : ControllerBase
{
    private readonly ILogger<GatewayController> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public GatewayController(ILogger<GatewayController> logger, IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger.LogInformation("GatewayController() Called");
    }

    private HttpClient CreateUserServiceClient()
    {
        var client = _httpClientFactory.CreateClient();
        var userServiceUrl = _config["Services:UserServiceUrl"];
        
        if (string.IsNullOrEmpty(userServiceUrl))
        {
            _logger.LogError("UserServiceUrl is not configured.");
            throw new Exception("UserServiceUrl is not configured.");
        }

        client.BaseAddress = new Uri(userServiceUrl);
        return client;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.PostAsJsonAsync("/api/user/login", loginRequest);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Login failed.");

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in Login: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var client = CreateUserServiceClient();
            var users = await client.GetFromJsonAsync<IEnumerable<UserDto>>("/api/user");
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetAllUsers: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("user/id-by-email/{email}")]
    public async Task<IActionResult> GetUserIdByEmail(string email)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.GetAsync($"/api/user/id-by-email/{email}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var userId = await response.Content.ReadFromJsonAsync<int?>();
            return Ok(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetUserIdByEmail: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.PostAsync("/api/user/logout", null);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Logout failed.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in Logout: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("get-token")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.PostAsJsonAsync("/api/user/login", loginRequest);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Login failed.");

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return Ok(loginResponse?.Token ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetToken: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("user/authenticated")]
    public async Task<IActionResult> IsUserAuthenticated([FromHeader] string token)
    {
        if (string.IsNullOrEmpty(token))
            return Ok(new Dictionary<string, string> { { "IsAuthenticated", "false" }, { "Role", "Customer" } });

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User";

        return Ok(new Dictionary<string, string> { { "IsAuthenticated", "true" }, { "Role", role } });
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.DeleteAsync($"/api/user/{id}");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to delete user.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in DeleteUser: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("user/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.PutAsJsonAsync($"/api/user/{id}", updateUserDto);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to update user.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in UpdateUser: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost("user/exists")]
    public async Task<IActionResult> DoesUserExist([FromBody] Dictionary<string, string> userData)
    {
        try
        {
            var client = CreateUserServiceClient();
            var response = await client.PostAsJsonAsync("/api/user/exists", userData);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "User existence check failed.");

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in DoesUserExist: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var client = CreateUserServiceClient();
            var user = await client.GetFromJsonAsync<UserDto>($"/api/user/{id}");
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetUserById: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("users/details")]
    public async Task<IActionResult> GetAllUsersDetails()
    {
        try
        {
            var client = CreateUserServiceClient();
            var userDetails = await client.GetFromJsonAsync<IEnumerable<UserDetailsDto>>("/api/user/details");
            return Ok(userDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetAllUsersDetails: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("user/logged-in-username")]
    public async Task<IActionResult> GetLoggedInUserName()
    {
        try
        {
            var client = CreateUserServiceClient();
            var username = await client.GetStringAsync("/api/user/logged-in-username");
            return Ok(username);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetLoggedInUserName: {ex.Message}");
            return StatusCode(500, "Internal server error.");
        }
    }
}
