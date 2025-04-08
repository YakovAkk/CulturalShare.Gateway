using CulturalShare.Foundation.AspNetCore.Extensions.Constants;
using CulturalShare.Foundation.Authorization.JwtServices;
using CulturalShare.Gateway.Configuration.Base;
using Google.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        builder.Services.AddScoped<IJwtBlacklistService, JwtBlacklistService>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHeaderPropagation(options => options.Headers.Add(LoggingConsts.CorrelationIdHeaderName));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        logger.Information($"{nameof(ApplicationServiceInstaller)} installed.");
    }
}
