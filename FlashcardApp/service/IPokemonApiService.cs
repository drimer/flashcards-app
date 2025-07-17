namespace FlashcardApp.Service;

public interface IPokemonApiService
{
    Task<Model.Pokemon?> GetRandomPokemonAsync();
    Task<Model.Pokemon> GetPokemonByNumberAsync(int number);
}
