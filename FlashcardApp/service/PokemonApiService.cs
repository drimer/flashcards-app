using FlashcardApp.Client;

namespace FlashcardApp.Service;

public class PokemonApiService : IPokemonApiService
{
    private static readonly PokemonApiClient _pokemonApiClient = new();

    public async Task<Model.Pokemon?> GetRandomPokemonAsync()
    {
        int randomNumber = new Random().Next(1, 1026);
        Model.Pokemon pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(randomNumber) ?? throw new ArgumentException("Pokemon not found.");
        return pokemon;
    }

    public async Task<Model.Pokemon> GetPokemonByNumberAsync(int number)
    {
        Model.Pokemon pokemon = await _pokemonApiClient.GetPokemonByNumberAsync(number) ?? throw new ArgumentException($"Pokemon with number {number} not found.");
        return pokemon;
    }
}
