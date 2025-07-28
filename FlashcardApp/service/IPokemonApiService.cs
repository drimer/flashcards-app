using FlashcardApp.Model;

namespace FlashcardApp.Service
{
    public interface IPokemonApiService
    {
        Task<Pokemon?> GetRandomPokemonAsync();

        Task<Pokemon> GetPokemonByNumberAsync(int number);
    }
}
