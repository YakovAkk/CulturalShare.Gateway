using AuthenticationProto;
using CulturalShare.Foundation.AspNetCore.Extensions.Helpers;
using CulturalShare.Gateway.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserGrpcService.UserGrpcServiceClient _userClient;
    private readonly ILogger<UserController> _logger;

    public UserController(
        UserGrpcService.UserGrpcServiceClient userClient, 
        ILogger<UserController> logger)
    {
        _userClient = userClient;
        _logger = logger;
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    { 
        _logger.LogDebug($"{nameof(CreateUserAsync)} request.");

        var headers = HttpHelper.CreateHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.CreateUserAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("AllowUser")]
    public async Task<IActionResult> AllowUserAsync([FromBody] AllowUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(AllowUserAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.AllowUserAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("RestrictUser")]
    public async Task<IActionResult> RestrictUserAsync([FromBody] RestrictUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(RestrictUserAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.RestrictUserAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("FollowUser")]
    public async Task<IActionResult> FollowUserAsync([FromBody] FollowUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(FollowUserAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.FollowUserAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("UnfollowUser")]
    public async Task<IActionResult> UnfollowUserAsync([FromBody] UnfollowUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(UnfollowUserAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.UnfollowUserAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("SearchUserByName")]
    public async Task<IActionResult> SearchUserByNameAsync([FromBody] SearchUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(SearchUserByNameAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.SearchUserByNameAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }

    [HttpPost("ToggleNotifications")]
    public async Task<IActionResult> ToggleNotificationsAsync([FromBody] ToggleNotificationsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"{nameof(ToggleNotificationsAsync)} request.");

        var headers = await this.CreateSecureHeaderWithCorrelationId(HttpContext);

        var result = await _userClient.ToggleNotificationsAsync(request, headers, cancellationToken: cancellationToken);
        return Ok(result);
    }
}
