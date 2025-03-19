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

    // POST: signup to the microservice UserService
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto request)
    {
        _logger.LogInformation("SignUp() Called");
        var client = _httpClientFactory.CreateClient("UserServiceClient");
        var response = await client.PostAsJsonAsync("api/user/signup", request);
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest();
        }
        return Ok();
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
