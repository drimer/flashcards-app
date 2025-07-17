using FlashcardApp.Client;

namespace FlashcardApp.Service;


public class PokemonApiService : IPokemonApiService
{
    private static readonly PokemonApiClient _pokemonApiClient = new();

    public async Task<Model.Pokemon?> GetRandomPokemonAsync()
    {
        var randomNumber = new Random().Next(1, 1026);
        var pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(randomNumber);
        if (pokemon == null)
        {
            throw new ArgumentException("Pokemon not found.");
        }
        return pokemon;
    }

    public async Task<Model.Pokemon> GetPokemonByNumberAsync(int number)
    {
        var pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(number);
        if (pokemon == null)
        {
            throw new ArgumentException($"Pokemon with number {number} not found.");
        }
        return pokemon;
    }
}