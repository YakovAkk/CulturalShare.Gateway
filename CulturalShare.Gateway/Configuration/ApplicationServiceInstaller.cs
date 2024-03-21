using CulturalShare.Common.Helper.Constants;
using CulturalShare.Gateway.Configuration.Base;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHeaderPropagation(options => options.Headers.Add(LoggingConsts.CorrelationIdHeaderName));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        logger.Information($"{nameof(ApplicationServiceInstaller)} installed.");
    }
}
