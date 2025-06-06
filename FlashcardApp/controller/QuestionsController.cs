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
        var questionField = "type";
        var question = new Service.PokemonQuestion("Pikachu", questionField);

        var response = new Dto.NewQuestionResponseDto
        {
            Type = question.GetType().Name,
            Field = questionField,
            Id = 25,
            Message = question.AsString()
        };

        return response;
    }
}