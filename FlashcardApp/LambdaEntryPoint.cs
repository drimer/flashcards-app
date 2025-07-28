using Amazon.Lambda.AspNetCoreServer;

namespace FlashcardApp
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            _ = builder.UseLambdaServer();
            _ = builder.UseStartup<Startup>();
        }
    }
}
