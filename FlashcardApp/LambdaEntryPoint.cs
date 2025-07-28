using Amazon.Lambda.AspNetCoreServer;

namespace FlashcardApp;

public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseLambdaServer();
        builder.UseStartup<Startup>();
    }
}
