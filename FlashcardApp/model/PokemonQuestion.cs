namespace FlashcardApp.Model;

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
                return $"What is {Pokemon.CapitalisedName}'s type?";
            case "hp":
                return $"What is {Pokemon.CapitalisedName}'s base HP?";
            default:
                throw new ArgumentException($"Unknown field: {Field}");
        }
    }

    public ITopic getTopic()
    {
        return Pokemon;
    }
}