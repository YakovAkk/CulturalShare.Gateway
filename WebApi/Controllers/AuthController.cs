using AuthenticationProto;
using CulturalShare.Foundation.AspNetCore.Extensions.Helpers;
using CulturalShare.Gateway.Extensions;
using Google.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PostsReadProto;

namespace CulturalShare.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthenticationGrpcService.AuthenticationGrpcServiceClient _authClient;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AuthenticationGrpcService.AuthenticationGrpcServiceClient authClient, 
        ILogger<AuthController> logger)
    {
        _authClient = authClient;
        _logger = logger;
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> LoginAsync([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(LoginAsync)} request.");

        var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.SignInAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("SignOut")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] SignOutRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RefreshTokenAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.SignOutAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RefreshTokenAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _authClient.RefreshTokenAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }
}
