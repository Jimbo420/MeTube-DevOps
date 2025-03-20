using Microsoft.AspNetCore.Mvc;
using MeTube_GateWay.DTO;
using System.Security.Claims;
namespace MeTube.Gateway.Controllers;

[ApiController]
[Route("api")]
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

    // GET: get all users from the microservice UserService
    [HttpGet("user")]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("GetAllUsers() Called");
        var client = _httpClientFactory.CreateClient("UserServiceClient");
        var content = await client.GetFromJsonAsync<List<UserDto>>("api/user/manageUsers") ?? [];
        return Ok(content);
    }

    // GET: user by id from the microservice UserService
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        _logger.LogInformation("GetUser() Called");
        var client = _httpClientFactory.CreateClient("UserServiceClient");
        var content = await client.GetFromJsonAsync<UserDto>($"api/user/{id}");
        if (content == null)
        {
            return NotFound();
        }
        return Ok(content);
    }

    [HttpPost("user/login")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDto request)
    {
        _logger.LogInformation("UserLogin() Called - forwarding to regular login");
        // Anropa den vanliga login-metoden
        return await Login(request);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        _logger.LogInformation("Login() Called with username: {Username}", request?.Username);

        try
        {
            var client = _httpClientFactory.CreateClient("UserServiceClient");

            // Log URL we're calling
            var requestUrl = "api/user/login";
            _logger.LogInformation("Sending request to: {Url}", requestUrl);

            var response = await client.PostAsJsonAsync(requestUrl, request);

            _logger.LogInformation("Received response with status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Error response: {Error}", error);

                // Return the actual error from the user service
                return StatusCode((int)response.StatusCode, error);
            }

            var result = await response.Content.ReadFromJsonAsync<object>();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login process");
            return StatusCode(500, "An unexpected error occurred during login");
        }
    }



    // POST: signup to the microservice UserService
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto request)
    {
        _logger.LogInformation("SignUp() Called with username: {Username}", request?.Username);

        try
        {
            var client = _httpClientFactory.CreateClient("UserServiceClient");

            // Log the URL we're sending to
            var requestUrl = "api/user";
            _logger.LogInformation("Sending request to: {Url}", requestUrl);

            // Add more details to debug what's happening
            var response = await client.PostAsJsonAsync(requestUrl, request);

            _logger.LogInformation("Received response with status code: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Error response: {Error}", error);

                // Return the actual error from the user service
                return StatusCode((int)response.StatusCode, error);
            }

            return Ok(new { Message = "User signed up successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during signup process");
            return StatusCode(500, "An unexpected error occurred during signup");
        }
    }

    [HttpPost("user/signup")]
    public async Task<IActionResult> UserSignUp([FromBody] CreateUserDto request)
    {
        _logger.LogInformation("UserSignUp() Called - forwarding to regular signup");
        // Just reuse the existing signup method
        return await SignUp(request);
    }

    // DELETE: remove user by username from the microservice UserService
    [HttpDelete("user/{username}")]
    public async Task<IActionResult> RemoveUser(string username)
    {
        _logger.LogInformation("RemoveUser() Called");
        var client = _httpClientFactory.CreateClient("UserServiceClient");
        var response = await client.DeleteAsync($"api/user/byUsername/{username}");
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest();
        }
        return Ok();
    }

}
