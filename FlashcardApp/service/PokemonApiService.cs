using FlashcardApp.Client;

namespace FlashcardApp.Service;


class PokemonApiService
{
    private static readonly PokemonApiClient _pokemonApiClient = new();

    public async Task<Model.Pokemon?> GetRandomPokemonAsync(string name)
    {
        var randomNumber = new Random().Next(1, 1026);
        var pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(randomNumber);
        if (pokemon == null)
        {
            throw new ArgumentException("Pokemon not found.");
        }
        return pokemon;
    }
}