using FlashcardApp.Model;

namespace FlashcardApp.Client;

public class HistoricalFigureApiClient : IHistoricalFigureApiClient
{
    private static readonly HistoricalFigure[] AllFigures =
    [
        new HistoricalFigure
            {
                Number = 1,
                Name = "Albert Einstein",
                Conflicts = ["World War II"],
                Occupation = ["Theoretical Physicist", "Philosopher of Science"],
                CauseOfDeath = "Natural Causes",
            },
        ];

    public async Task<HistoricalFigure?> GetHistoricalFigureByNumberAsync(int number)
    {
        return AllFigures.FirstOrDefault(f => f.Number == number);
    }

    public async Task<HistoricalFigure> GetRandomHistoricalFigureAsync()
    {
        Random random = new Random();
        return AllFigures[random.Next(AllFigures.Length)];
    }
}
