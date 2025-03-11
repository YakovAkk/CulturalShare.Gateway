using AuthenticationProto;
using CulturalShare.Common.Helper.EnvHelpers;
using CulturalShare.Gateway.Configuration.Base;
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

        builder.Services.AddGrpcClient<Authentication.AuthenticationClient>(options =>
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
