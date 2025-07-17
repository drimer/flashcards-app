using FlashcardApp.Model;

namespace FlashcardApp.Client;

public class HistoricalFigureApiClient
{
    private static readonly HistoricalFigure[] AllFigures = new[]
    {
        new HistoricalFigure{
            Number = 1,
            Name = "Albert Einstein",
            Conflicts = new[] { "World War II" },
            Occupation = new[] { "Theoretical Physicist", "Philosopher of Science" },
            CauseOfDeath = "Natural Causes"
        },
    };

    public async Task<HistoricalFigure?> GetHistoricalFigureByNumberAsync(int number)
    {
        return AllFigures.FirstOrDefault(f => f.Number == number);
    }

    public async Task<HistoricalFigure> GetRandomHistoricalFigureAsync()
    {
        var random = new Random();
        return AllFigures[random.Next(AllFigures.Length)];
    }
}