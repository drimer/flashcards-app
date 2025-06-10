using FlashcardApp.Model;

namespace FlashcardApp.Service;

public sealed class PokemonQuestion : IQuestion
{
    public string Type { get => "PokemonQuestion"; }
    public Pokemon Pokemon { get; init; }
    public string Field { get; init; }

    public PokemonQuestion(Pokemon pokemon, string field)
    {
        Pokemon = pokemon;
        Field = field;
    }

    public string AsString()
    {
        switch (Field)
        {
            case "type":
                return $"What is {Pokemon.Name}'s type?";
            case "hp":
                return $"What is {Pokemon.Name}'s base HP?";
            default:
                throw new ArgumentException($"Unknown field: {Field}");
        }
        return $"What is {Pokemon.Name}'s {Field}?";
    }

    public ITopic getTopic()
    {
        return Pokemon;
    }
}