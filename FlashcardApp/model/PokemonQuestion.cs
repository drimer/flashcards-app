namespace FlashcardApp.Model
{
    public sealed class PokemonQuestion(Pokemon pokemon, string field) : IQuestion
    {
        public string Type => "PokemonQuestion";
        public Pokemon Pokemon { get; init; } = pokemon;
        public string Field { get; init; } = field;

        public string AsString()
        {
            return Field switch
            {
                "type" => $"What is {Pokemon.CapitalisedName}'s type?",
                "hp" => $"What is {Pokemon.CapitalisedName}'s base HP?",
                _ => throw new ArgumentException($"Unknown field: {Field}"),
            };
        }

        public ITopic GetTopic()
        {
            return Pokemon;
        }
    }
}
