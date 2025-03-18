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
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("GetAllUsers() Called");
        var client = _httpClientFactory.CreateClient("UserServiceClient");
        var content = await client.GetFromJsonAsync<List<UserDto>>("api/user/manageUsers");
        return Ok(content);
    }

}
