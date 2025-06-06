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
    public static object GetWeatherForecast(HttpContext httpContext)
    {
        var summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        var forecast = Enumerable.Range(1, 5).Select(index =>
            new Controller.WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)],
            })
            .ToArray();

        return forecast;
    }

    public static object GetNewQuestion(HttpContext httpContext)
    {
        var response = new Dto.NewQuestionResponseDto
        {
            Type = "text",
            Field = "question",
            Id = 1,
            Message = "What is the capital of France?"
        };

        return response;
    }
}