using CulturalShare.Foundation.AspNetCore.Extensions.Extensions;
using CulturalShare.Gateway.Configuration.Base;
using Microsoft.AspNetCore.Builder;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class LoggingServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        builder.UseCustomSerilog(builder.Configuration);

        logger.Information($"{nameof(LoggingServiceInstaller)} installed.");
    }
}
