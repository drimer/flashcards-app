using FlashcardApp.Model;

namespace FlashcardApp.Service;

public sealed class PokemonQuestion : IQuestion
{
    private readonly Pokemon _pokemon;
    public Pokemon Pokemon { get { return _pokemon; } }

    private readonly string _field;
    public string Field { get { return _field; } }


    public PokemonQuestion(Pokemon pokemon, string field)
    {
        _pokemon = pokemon;
        _field = field;
    }

    public string AsString()
    {
        return $"What is {Pokemon.Name}'s {Field}?";
    }
}