using FlashcardApp.Client;
using FlashcardApp.Service;

namespace FlashcardApp.Controller;



public static class QuestionsController
{

    private static readonly PokemonApiClient _pokemonApiClient = new();

    public static object GetNewQuestion(HttpContext httpContext)
    {
        var pokemon = new Model.Pokemon
        {
            Number = 25,
            Name = "Pikachu",
            Types = new[] { "Electric" }
        };
        var question = new Service.PokemonQuestion(pokemon, "type");

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

    public static async Task<Dto.PostAnswerResponseDto> SubmitAnswer(HttpContext httpContext, Dto.PostAnswerRequestDto request)
    {
        if (request.Question.Type != "PokemonQuestion")
        {
            return new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = "Invalid question type."
            };
        }

        if (request.Question.Topic == null)
        {
            return new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = "Topic is required."
            };
        }

        Model.Pokemon pokemon;
        pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(request.Question.Topic.Number);

        try
        {
            pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(request.Question.Topic.Number);
        }
        catch (Exception ex)
        {
            return new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = $"Error retrieving Pokemon data: {ex.Message}"
            };
        }

        var isCorrect = (new AnswerEvaluator()).IsCorrect(
            new Service.PokemonQuestion(pokemon, request.Question.Field),
            new Service.PokemonAnswer
            {
                Value = request.Answer.Trim().ToLower()
            }
        );

        if (!isCorrect)
        {
            return new Dto.PostAnswerResponseDto
            {
                IsCorrect = false,
                Message = "Incorrect answer. Please try again."
            };
        }
        else
        {
            return new Dto.PostAnswerResponseDto
            {
                IsCorrect = true,
                Message = "Correct answer!"
            };
        }
    }
}