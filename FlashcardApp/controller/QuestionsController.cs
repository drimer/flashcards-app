using FlashcardApp.Service;

namespace FlashcardApp.Controller;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
};



public static class QuestionsController
{
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

    public static Dto.PostAnswerResponseDto SubmitAnswer(HttpContext httpContext, Dto.PostAnswerRequestDto request)
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
                Message = "Invalid topic."
            };
        }

        var isCorrect = (new AnswerEvaluator()).IsCorrect(
            new Service.PokemonQuestion(
                new Model.Pokemon
                {
                    Number = request.Question.Topic.Number,
                    Name = "pikachu",  // TODO: Retrieve name from API Call using Number
                    Types = new[] { "electric" }  // TODO: Retrieve types from API Call using Number
                },
                request.Question.Field
            ),
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