using FlashcardApp.Model;

namespace FlashcardApp.Service;


public class QuestionBuilder
{
    public IQuestion CreateQuestion(ITopic topic)
    {
        switch (topic)
        {
            case Pokemon pokemon:
                return BuildPokemonQuestion(pokemon);
            case HistoricalFigure historicalFigure:
                return BuildHistoricalFigureQuestion(historicalFigure);
            default:
                throw new ArgumentException("Unsupported topic type");
        }
    }

    private PokemonQuestion BuildPokemonQuestion(Pokemon pokemon)
    {
        var randomField = new Random().Next(0, 2) == 0 ? "type" : "hp";
        return new PokemonQuestion(pokemon, randomField);
    }

    private HistoricalFigureQuestion BuildHistoricalFigureQuestion(HistoricalFigure figure)
    {
        string[] fields = { "Conflicts", "Occupation", "CauseOfDeath" };
        var randomField = fields[new Random().Next(fields.Length)];
        return new HistoricalFigureQuestion(figure, randomField);
    }
}