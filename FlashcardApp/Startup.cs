namespace FlashcardApp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddScoped<Client.PokemonApiClient>();
        services.AddScoped<Service.IPokemonApiService, Service.PokemonApiService>();
        services.AddScoped<Client.IHistoricalFigureApiClient, Client.HistoricalFigureApiClient>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
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
    }
}
