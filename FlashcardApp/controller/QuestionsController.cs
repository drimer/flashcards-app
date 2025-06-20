using FlashcardApp.Client;
using FlashcardApp.Model;
using FlashcardApp.Service;
using Microsoft.AspNetCore.Http.Features;

namespace FlashcardApp.Controller;



public static class QuestionsController
{

    private static readonly PokemonApiClient _pokemonApiClient = new();
    private static readonly PokemonApiService _pokemonApiService = new();

    public static object GetNewQuestion(HttpContext httpContext, string topic)
    {
        return topic switch
        {
            "pokemon" => GetNewPokemonQuestion(httpContext),
            "historicalFigure" => GetNewHistoricalFigureQuestion(httpContext),
            _ => new { Error = "Invalid topic." }
        };
    }

    public static async Task<Dto.PostAnswerResponseDto> SubmitAnswer(HttpContext httpContext, Dto.PostAnswerRequestDto request)
    {
        return request.Question.Type switch
        {
            "PokemonQuestion" => await HandlePokemonQuestion(request),
            "HistoricalFigureQuestion" => await HandleHistoricalFigureQuestion(request),
            _ => new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = "Invalid question type."
            }
        };
    }

    private static object GetNewHistoricalFigureQuestion(HttpContext httpContext)
    {
        // Assuming HistoricalFigureQuestion is implemented similarly to PokemonQuestion
        // This part would need to be implemented based on the actual HistoricalFigureQuestion logic
        return new { Error = "Historical figure questions are not yet implemented." };
    }

    private static Dto.NewQuestionResponseDto GetNewPokemonQuestion(HttpContext httpContext)
    {
        var pokemon = _pokemonApiService.GetRandomPokemonAsync().Result;
        var question = (new QuestionBuilder()).CreateQuestion(pokemon);

        return new Dto.NewQuestionResponseDto
        {
            Question = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = new Dto.PokemonTopicDto
                {
                    Number = pokemon.Number
                },
                Message = question.AsString(),
                Field = question.Field
            }
        };
    }

    private static async Task<Dto.PostAnswerResponseDto> HandlePokemonQuestion(Dto.PostAnswerRequestDto request)
    {
        var pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(request.Question.Topic.Number);
        var question = new PokemonQuestion(pokemon, request.Question.Field);
        var answer = new PokemonAnswer { Value = request.Answer.Trim().ToLower() };

        var evaluator = new AnswerEvaluator();
        bool isCorrect = evaluator.IsCorrect(question, answer);

        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Correct!" : "Incorrect."
        };
    }

    private static async Task<Dto.PostAnswerResponseDto> HandleHistoricalFigureQuestion(Dto.PostAnswerRequestDto request)
    {
        // Assuming HistoricalFigureQuestion is implemented similarly to PokemonQuestion
        // This part would need to be implemented based on the actual HistoricalFigureQuestion logic
        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = false,
            Message = "Historical figure questions are not yet implemented."
        };
    }
}