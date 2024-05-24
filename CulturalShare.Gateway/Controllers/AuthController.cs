using AuthenticationProto;
using CulturalShare.Common.Helper;
using Microsoft.AspNetCore.Mvc;

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
        _logger.LogDebug($"{nameof(LoginAsync)} request.");

        var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.LoginAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("Registration")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RegistrationAsync)} request.");

        var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.RegistrationAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RefreshTokenAsync)} request.");

        var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.RefreshTokenAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }
}
