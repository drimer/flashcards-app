using System.Text.Json;

namespace FlashcardApp.Client;

public class PokemonApiClient : IDisposable
{
    private readonly HttpClient _httpClient = new();
    private readonly string baseUrl;

    public PokemonApiClient()
    {
        string? apiUrl = Environment.GetEnvironmentVariable("POKEMON_API_URL");
        baseUrl = !Uri.IsWellFormedUriString(apiUrl, UriKind.Absolute) ? string.Empty : string.Join(string.Empty, apiUrl.TrimEnd('/'));
    }

    public async Task<Model.Pokemon> GetPokemonByNumberAsync(int number)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"{baseUrl}/{number}");
        _ = response.EnsureSuccessStatusCode();

        Stream responseAsString = response.Content.ReadAsStream();
        using JsonDocument doc = JsonDocument.Parse(responseAsString);
        JsonElement root = doc.RootElement;

        string? name = root.GetProperty("name").GetString();

        // Inspect the element `types[0].type.name`
        string? typeName = root
            .GetProperty("types")[0]
            .GetProperty("type")
            .GetProperty("name")
            .GetString();

        int hp = root.TryGetProperty("stats", out JsonElement stats) && stats.GetArrayLength() > 0
            ? stats[0].GetProperty("base_stat").GetInt32()
            : 0;

        JsonElement typesArray = root.GetProperty("types");
        string? type1 = typesArray[0].GetProperty("type").GetProperty("name").GetString();
        string? type2 = typesArray.GetArrayLength() > 1
            ? typesArray[1].GetProperty("type").GetProperty("name").GetString()
            : null;

        return new Model.Pokemon
        {
            Number = number,
            Name = name,
            Types = [type1, type2],
            Hp = hp,
        };
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
