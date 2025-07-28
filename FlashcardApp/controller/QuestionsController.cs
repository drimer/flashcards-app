using System.Globalization;
using FlashcardApp.Client;
using FlashcardApp.Model;
using FlashcardApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace FlashcardApp.Controller;

[Route("question")]
public class QuestionsController(IPokemonApiService pokemonApiService, IHistoricalFigureApiClient historicalFigureApiClient) : ControllerBase
{
    private readonly IPokemonApiService pokemonApiService = pokemonApiService;
    private readonly IHistoricalFigureApiClient historicalFigureApiClient = historicalFigureApiClient;

    [HttpGet]
    public async Task<Dto.NewQuestionResponseDto> TestGet()
    {
        return await GetNewQuestion("pokemon");
    }

    [HttpGet("{topic}")]
    public async Task<Dto.NewQuestionResponseDto> GetNewQuestion(string topic)
    {
        bool countSuccess = int.TryParse(HttpContext.Request.Query["count"], out int count);
        return countSuccess && count < 1
            ? new Dto.NewQuestionResponseDto { Error = "Count must be at least 1." }
            : topic switch
            {
                "pokemon" => await GetNewPokemonQuestions(count),
                "historicalFigure" => await GetNewHistoricalFigureQuestions(count),
                _ => new Dto.NewQuestionResponseDto { Error = "Invalid topic." },
            };
    }

    [HttpPost("answer")]
    public async Task<Dto.PostAnswerResponseDto> SubmitAnswer([FromBody] Dto.PostAnswerRequestDto request)
    {
        return request.Question.Type switch
        {
            "PokemonQuestion" => await HandlePokemonQuestion(request),
            "HistoricalFigureQuestion" => await HandleHistoricalFigureQuestion(request),
            _ => new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = "Invalid question type.",
            },
        };
    }

    private async Task<Dto.NewQuestionResponseDto> GetNewHistoricalFigureQuestions(int batchSize)
    {
        Dto.QuestionDto[] questions = new Dto.QuestionDto[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            HistoricalFigure historicalFigure = await historicalFigureApiClient.GetRandomHistoricalFigureAsync();
            IQuestion question = new QuestionBuilder().CreateQuestion(historicalFigure);

            questions[i] = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = new Dto.TopicDto
                {
                    Id = historicalFigure.Number,
                },
                Message = question.AsString(),
                Field = question.Field,
            };
        }

        return new Dto.NewQuestionResponseDto
        {
            Questions = questions,
        };
    }

    private async Task<Dto.NewQuestionResponseDto> GetNewPokemonQuestions(int batchSize)
    {
        Dto.QuestionDto[] questions = new Dto.QuestionDto[batchSize];

        for (int i = 0; i < batchSize; i++)
        {
            Pokemon? pokemon = await pokemonApiService.GetRandomPokemonAsync();
            IQuestion question = new QuestionBuilder().CreateQuestion(pokemon);

            questions[i] = new Dto.QuestionDto
            {
                Type = question.GetType().Name,
                Topic = PokemonTopicMapper.ToTopicDto(pokemon),
                Message = question.AsString(),
                Field = question.Field,
            };
        }

        return new Dto.NewQuestionResponseDto
        {
            Questions = questions,
        };
    }

    private async Task<Dto.PostAnswerResponseDto> HandlePokemonQuestion(Dto.PostAnswerRequestDto request)
    {
        Pokemon topic = PokemonTopicMapper.ToPokemon(request.Question.Topic);
        Pokemon pokemon = await pokemonApiService.GetPokemonByNumberAsync(topic.Number);
        PokemonQuestion question = new PokemonQuestion(pokemon, request.Question.Field);
        Answer answer = new Answer { Value = request.Answer.Trim().ToLower(CultureInfo.InvariantCulture) };

        PokemonAnswerEvaluator evaluator = new PokemonAnswerEvaluator();
        bool isCorrect = evaluator.IsCorrect(question, answer);

        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Correct!" : "Incorrect.",
        };
    }

    private async Task<Dto.PostAnswerResponseDto> HandleHistoricalFigureQuestion(Dto.PostAnswerRequestDto request)
    {
        HistoricalFigure topic = HistoricalFigureTopicMapper.ToHistoricalFigure(request.Question.Topic);
        HistoricalFigure? historicalFigure = await historicalFigureApiClient.GetHistoricalFigureByNumberAsync(topic.Number);
        HistoricalFigureQuestion question = new HistoricalFigureQuestion(historicalFigure, request.Question.Field);
        Answer answer = new Answer { Value = request.Answer.Trim().ToLower(CultureInfo.InvariantCulture) };

        HistoricalFigureAnswerEvaluator evaluator = new HistoricalFigureAnswerEvaluator();
        bool isCorrect = evaluator.IsCorrect(question, answer);

        return new Dto.PostAnswerResponseDto
        {
            IsCorrect = isCorrect,
            Message = isCorrect ? "Correct!" : "Incorrect.",
        };
    }
}
