using FlashcardApp.Client;
using FlashcardApp.Service;

namespace FlashcardApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<PokemonApiClient>();
            builder.Services.AddScoped<IPokemonApiService, PokemonApiService>();
            builder.Services.AddScoped<IHistoricalFigureApiClient, HistoricalFigureApiClient>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

#if !LAMBDA
            app.Run();
#endif
        }
    }
}
