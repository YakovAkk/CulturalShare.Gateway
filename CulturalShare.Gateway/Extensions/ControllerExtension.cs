using AuthenticationProto;
using CulturalShare.Common.Helper;
using CulturalShare.Common.Helper.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace CulturalShare.Gateway.Extensions;

public static class ControllerExtension
{
    public static async Task<Metadata> CreateSecureHeader(this ControllerBase controllerBase, HttpContext httpContext, Authentication.AuthenticationClient client)
    {
        var userId = HttpHelper.GetCustomerId(httpContext);
        var userEmail = HttpHelper.GetCustomerEmail(httpContext);

        var accessTokenRequest = new GetOneTimeTokenRequest()
        {
            UserId = userId,
            Email = userEmail
        };
        var accessToken = await client.GetOneTimeTokenAsync(accessTokenRequest);

        var headers = HttpHelper.CreateHeaderWithCorrelationId()
            .AddAuthHeader(accessToken.AccessToken);

        return headers;
    }
}
