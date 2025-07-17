using FlashcardApp.Model;

namespace FlashcardApp.Client;

public interface IHistoricalFigureApiClient
{
    Task<Model.HistoricalFigure> GetRandomHistoricalFigureAsync();
    Task<Model.HistoricalFigure?> GetHistoricalFigureByNumberAsync(int number);
}
