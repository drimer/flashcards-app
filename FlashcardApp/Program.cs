using FlashcardApp.Client;
using FlashcardApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlashcardApp;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<PokemonApiClient>();
        builder.Services.AddScoped<IPokemonApiService, PokemonApiService>();
        builder.Services.AddScoped<IHistoricalFigureApiClient, HistoricalFigureApiClient>();
        // builder.Services.AddScoped<Controller.QuestionsController>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
