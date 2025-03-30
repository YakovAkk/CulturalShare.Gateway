using AuthenticationProto;
using CulturalShare.Foundation.AspNetCore.Extensions.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CulturalShare.Gateway.Extensions;

public static class ControllerExtension
{
    public static async Task<Metadata> CreateSecureHeaderWithCorrelationId(this ControllerBase controllerBase, HttpContext httpContext)
    {
        var authHeaders = new Metadata().AddAuthHeader(GetAuthToken(httpContext));

        var headers = authHeaders.AddCorrelationIdHeader(httpContext);

        return headers;
    }

    private static string GetAuthToken(HttpContext httpContext)
    {
        if (httpContext == null || httpContext.Request == null)
        {
            throw new ArgumentNullException(nameof(httpContext), "HttpContext is null.");
        }

        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authHeader.Substring("Bearer ".Length).Trim();
    }
}
