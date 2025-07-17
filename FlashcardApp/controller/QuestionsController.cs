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

    public static async Task<Dto.NewQuestionResponseDto> GetNewQuestion(HttpContext httpContext, string topic)
    {
        var countSuccess = int.TryParse(httpContext.Request.Query["count"], out int count);
        if (countSuccess && count < 1)
        {
            return new Dto.NewQuestionResponseDto { Error = "Count must be at least 1." };
        }

        return topic switch
        {
            "pokemon" => await GetNewPokemonQuestions(count),
            "historicalFigure" => GetNewHistoricalFigureQuestions(count),
            _ => new Dto.NewQuestionResponseDto { Error = "Invalid topic." }
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

    private static Dto.NewQuestionResponseDto GetNewHistoricalFigureQuestions(int batchSize)
    {
        var questions = new Dto.QuestionDto[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            var historicalFigure = _historicalFigureApiClient.GetRandomHistoricalFigureAsync();
            var question = (new QuestionBuilder()).CreateQuestion(historicalFigure);

            questions[i] = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = new Dto.TopicDto
                {
                    Id = historicalFigure.Number
                },
                Message = question.AsString(),
                Field = question.Field
            };
        }

        return new Dto.NewQuestionResponseDto
        {
            Questions = questions
        };
    }

    private static async Task<Dto.NewQuestionResponseDto> GetNewPokemonQuestions(int batchSize)
    {
        var questions = new Dto.QuestionDto[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            var pokemon = await _pokemonApiService.GetRandomPokemonAsync();
            var question = (new QuestionBuilder()).CreateQuestion(pokemon);

            questions[i] = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = PokemonTopicMapper.ToTopicDto(pokemon),
                Message = question.AsString(),
                Field = question.Field
            };
        }

        return new Dto.NewQuestionResponseDto
        {
            Questions = questions
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
        var historicalFigure = _historicalFigureApiClient.GetHistoricalFigureByNumberAsync(topic.Number);
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