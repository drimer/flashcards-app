namespace FlashcardApp.Service;

public sealed class PokemonQuestion : IQuestion
{
    private readonly string _pokemonName;
    private readonly string _field;

    public PokemonQuestion(string pokemonName, string field)
    {
        _pokemonName = pokemonName;
        _field = field;
    }

    public string AsString()
    {
        return $"What is the {_field} of the Pokémon {_pokemonName}?";
    }
}