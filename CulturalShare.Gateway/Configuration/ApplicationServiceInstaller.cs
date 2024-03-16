using CulturalShare.Gateway.Configuration.Base;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        logger.Information($"{nameof(ApplicationServiceInstaller)} installed.");
    }
}
