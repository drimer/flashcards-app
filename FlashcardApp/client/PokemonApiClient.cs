using System.Text.Json;

namespace FlashcardApp.Client;


public class PokemonApiClient
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string BaseUrl;

    public PokemonApiClient()
    {
        var apiUrl = System.Environment.GetEnvironmentVariable("POKEMON_API_URL");
        if (!Uri.IsWellFormedUriString(apiUrl, UriKind.Absolute))
        {
            BaseUrl = "";
            // throw new InvalidOperationException("POKEMON_API_URL environment variable is not a valid URL.");
        }
        else
        {
            BaseUrl = string.Join("", apiUrl.TrimEnd('/'));
        }
    }

    public async Task<Model.Pokemon> GetPokemonByNumberAsync(int number)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{number}");
        response.EnsureSuccessStatusCode();

        var responseAsString = response.Content.ReadAsStream();
        using (JsonDocument doc = JsonDocument.Parse(responseAsString))
        {
            var root = doc.RootElement;

            var name = root.GetProperty("name").GetString();

            // Inspect the element `types[0].type.name`
            var typeName = root
                .GetProperty("types")[0]
                .GetProperty("type")
                .GetProperty("name")
                .GetString();

            var hp = root.TryGetProperty("stats", out var stats) && stats.GetArrayLength() > 0
                ? stats[0].GetProperty("base_stat").GetInt32()
                : 0;

            var typesArray = root.GetProperty("types");
            var type1 = typesArray[0].GetProperty("type").GetProperty("name").GetString();
            var type2 = typesArray.GetArrayLength() > 1
                ? typesArray[1].GetProperty("type").GetProperty("name").GetString()
                : null;

            return new Model.Pokemon
            {
                Number = number,
                Name = name,
                Types = new[] { type1, type2 },
                Hp = hp
            };
        }
    }
}