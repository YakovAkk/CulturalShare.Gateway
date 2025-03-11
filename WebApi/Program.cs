using CulturalShare.Common.Helper.Extensions;
using CulturalShare.Gateway.Configuration.Base;
using CulturalShare.Gateway.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.InstallServices(logger, typeof(IServiceInstaller).Assembly);

var app = builder.Build();

app.UseExceptionsHandler();
app.UseCorrelationIdMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsEnvironment("Test"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecureHeaders();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/_health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHeaderPropagation();

app.MapControllers();

logger.Information($"Env: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")} Running App...");
app.Run();
logger.Information("App finished.");
