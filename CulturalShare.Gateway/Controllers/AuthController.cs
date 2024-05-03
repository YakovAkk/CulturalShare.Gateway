using AuthenticationProto;
using CulturalShare.Common.Helper;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CulturalShare.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Authentication.AuthenticationClient _authClient;
    private readonly ILogger<PostsController> _logger;

    public AuthController(Authentication.AuthenticationClient authClient, 
        ILogger<PostsController> logger)
    {
        _authClient = authClient;
        _logger = logger;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(LoginAsync)} request. Boby = {JsonConvert.SerializeObject(request)}.");

        try
        {
            var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

            var result = await _authClient.LoginAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException ex)
        {
            throw;
        }
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RegistrationAsync)} request. Boby = {JsonConvert.SerializeObject(request)}");

        try
        {
            var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

            var result = await _authClient.RegistrationAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RefreshTokenAsync)} request. Boby = {JsonConvert.SerializeObject(request)}");

        try
        {
            var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

            var result = await _authClient.RefreshTokenAsync(request, headers, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }
}
