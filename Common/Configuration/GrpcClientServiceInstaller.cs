using AuthenticationProto;
using CulturalShare.Foundation.EntironmentHelper.EnvHelpers;
using CulturalShare.Gateway.Configuration.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PostsReadProto;
using PostsWriteProto;
using Serilog.Core;

namespace CulturalShare.Gateway.Configuration;

public class GrpcClientServiceInstaller : IServiceInstaller
{
    public void Install(WebApplicationBuilder builder, Logger logger)
    {
        var sortOutCredentialsHelper = new SortOutCredentialsHelper(builder.Configuration);

        var grpcClientsUrlConfiguration = sortOutCredentialsHelper.GetGrpcClientsUrlConfiguration();

        builder.Services.AddGrpcClient<AuthenticationGrpcService.AuthenticationGrpcServiceClient>(options =>
        {
            options.Address = new Uri(grpcClientsUrlConfiguration.AuthClientUrl);
        });

        builder.Services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(options =>
        {
            options.Address = new Uri(grpcClientsUrlConfiguration.AuthClientUrl);
        });

        builder.Services.AddGrpcClient<PostsRead.PostsReadClient>(options =>
        {
            options.Address = new Uri(grpcClientsUrlConfiguration.PostReadClientUrl);
        });

        builder.Services.AddGrpcClient<PostsWrite.PostsWriteClient>(options =>
        {
            options.Address = new Uri(grpcClientsUrlConfiguration.PostWriteClientUrl);
        });

        logger.Information($"{nameof(GrpcClientServiceInstaller)} installed. {JsonConvert.SerializeObject(grpcClientsUrlConfiguration)}");
    }
}
