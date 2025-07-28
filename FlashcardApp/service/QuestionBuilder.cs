using FlashcardApp.Model;

namespace FlashcardApp.Service
{
    public class QuestionBuilder
    {
        public IQuestion CreateQuestion(ITopic topic)
        {
            return topic switch
            {
                Pokemon pokemon => BuildPokemonQuestion(pokemon),
                HistoricalFigure historicalFigure => BuildHistoricalFigureQuestion(historicalFigure),
                _ => throw new ArgumentException("Unsupported topic type"),
            };
        }

        private static PokemonQuestion BuildPokemonQuestion(Pokemon pokemon)
        {
            string randomField = new Random().Next(0, 2) == 0 ? "type" : "hp";
            return new PokemonQuestion(pokemon, randomField);
        }

        private static HistoricalFigureQuestion BuildHistoricalFigureQuestion(HistoricalFigure figure)
        {
            string[] fields = ["Conflicts", "Occupation", "CauseOfDeath"];
            string randomField = fields[new Random().Next(fields.Length)];
            return new HistoricalFigureQuestion(figure, randomField);
        }
    }
}
