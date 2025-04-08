using CulturalShare.Foundation.EntironmentHelper.EnvHelpers;
using CulturalShare.Gateway.Configuration.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using StackExchange.Redis;

namespace Common.Configuration;

public class DatabaseServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        var sortOutCredentialsHelper = new SortOutCredentialsHelper(builder.Configuration);

        builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            return ConnectionMultiplexer.Connect(sortOutCredentialsHelper.GetRedisConnectionString());
        });

        logger.Information($"{nameof(DatabaseServiceInstaller)} installed.");
    }
}
