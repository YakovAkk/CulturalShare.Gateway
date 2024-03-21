using Serilog.Core;

namespace CulturalShare.Gateway.Configuration.Base;

public interface IServiceInstaller
{
    void Install(WebApplicationBuilder builder, Logger logger);
}
