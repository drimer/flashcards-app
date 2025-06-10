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
            default:
                throw new ArgumentException("Unsupported topic type");
        }
    }

    private PokemonQuestion BuildPokemonQuestion(Pokemon pokemon)
    {
        var randomField = new Random().Next(0, 2) == 0 ? "type" : "hp";
        return new PokemonQuestion(pokemon, randomField);
    }
}