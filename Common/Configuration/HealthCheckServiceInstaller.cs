﻿using CulturalShare.Gateway.Configuration.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class HealthCheckServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        builder.Services.AddHealthChecks();

        logger.Information($"{nameof(HealthCheckServiceInstaller)} installed.");
    }
}
