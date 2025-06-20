using FlashcardApp.Client;
using FlashcardApp.Model;
using FlashcardApp.Service;
using Microsoft.AspNetCore.Http.Features;

namespace FlashcardApp.Controller;



public static class QuestionsController
{

    private static readonly PokemonApiClient _pokemonApiClient = new();
    private static readonly PokemonApiService _pokemonApiService = new();
    private static readonly HistoricalFigureApiClient _historicalFigureApiClient = new();

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
        var historicalFigure = _historicalFigureApiClient.GetRandomHistoricalFigureAsync().Result;
        var question = (new QuestionBuilder()).CreateQuestion(historicalFigure);
        return new Dto.NewQuestionResponseDto
        {
            Question = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = new Dto.TopicDto
                {
                    Id = historicalFigure.Number
                },
                Message = question.AsString(),
                Field = question.Field
            }
        };
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
                Topic = PokemonTopicMapper.ToTopicDto(pokemon),
                Message = question.AsString(),
                Field = question.Field
            }
        };
    }

    private static async Task<Dto.PostAnswerResponseDto> HandlePokemonQuestion(Dto.PostAnswerRequestDto request)
    {
        var topic = PokemonTopicMapper.ToPokemon(request.Question.Topic);
        var pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(topic.Number);
        var question = new PokemonQuestion(pokemon, request.Question.Field);
        var answer = new Answer { Value = request.Answer.Trim().ToLower() };

        var evaluator = new PokemonAnswerEvaluator();
        bool isCorrect = evaluator.IsCorrect(question, answer);

        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Correct!" : "Incorrect."
        };
    }

    private static async Task<Dto.PostAnswerResponseDto> HandleHistoricalFigureQuestion(Dto.PostAnswerRequestDto request)
    {
        var topic = HistoricalFigureTopicMapper.ToHistoricalFigure(request.Question.Topic);
        var historicalFigure = await _historicalFigureApiClient.GetHistoricalFigureByNumberAsync(topic.Number);
        var question = new HistoricalFigureQuestion(historicalFigure, request.Question.Field);
        var answer = new Answer { Value = request.Answer.Trim().ToLower() };

        var evaluator = new HistoricalFigureAnswerEvaluator();
        bool isCorrect = evaluator.IsCorrect(question, answer);

        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Correct!" : "Incorrect."
        };
    }
}