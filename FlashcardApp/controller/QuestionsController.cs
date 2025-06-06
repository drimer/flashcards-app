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

        var response = new Dto.NewQuestionResponseDto
        {
            Type = question.GetType().Name,
            Field = question.Field,
            Message = question.AsString()
        };

        return response;
    }
}