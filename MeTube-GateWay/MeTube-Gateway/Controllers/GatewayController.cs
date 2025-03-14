namespace MeTube.Gateway.Controllers;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

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
        _logger.LogInformation("GatewayController() called.");
    }
}