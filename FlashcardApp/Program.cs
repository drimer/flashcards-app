using FlashcardApp.Client;
using FlashcardApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlashcardApp;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Register Dependencies for DI
        builder.Services.AddScoped<PokemonApiClient>();
        builder.Services.AddScoped<IPokemonApiService, PokemonApiService>();
        builder.Services.AddScoped<IHistoricalFigureApiClient, HistoricalFigureApiClient>();
        builder.Services.AddScoped<Controller.QuestionsController>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapGet("/question/{topic}", async (HttpContext context, string topic, [FromServices] Controller.QuestionsController questionsController) =>
        {
            var result = await questionsController.GetNewQuestion(context, topic);
            return Results.Ok(result);
        })
            .WithName("GetNewQuestion")
            .WithOpenApi();

        app.MapPost("/question/answer", async (HttpContext context, [FromServices] Controller.QuestionsController questionsController) =>
        {
            var request = await context.Request.ReadFromJsonAsync<Dto.PostAnswerRequestDto>();
            if (request == null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid request body.");
                return;
            }
            var result = await questionsController.SubmitAnswer(context, request);
            await context.Response.WriteAsJsonAsync(result);
        })
            .WithName("SubmitAnswer")
            .WithOpenApi()
            .Accepts<Dto.PostAnswerRequestDto>("application/json")
            .Produces<Dto.PostAnswerResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Accepts<Dto.PostAnswerRequestDto>("application/json")
            .Produces<Dto.PostAnswerResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.Run();
    }
}
