namespace FlashcardApp.Model
{
    public sealed class HistoricalFigureQuestion(HistoricalFigure topic, string field) : IQuestion
    {
        public string Type => "HistoricalFigureQuestion";
        public HistoricalFigure Topic { get; init; } = topic;
        public string Field { get; init; } = field;

        public ITopic GetTopic()
        {
            return Topic;
        }

        public string AsString()
        {
            return Field.ToLowerInvariant() switch
            {
                "conflicts" => $"Name one conflict where {Topic.Name} was involved in.",
                "occupation" => $"Name one occupation of {Topic.Name}.",
                "causeofdeath" => $"What was the cause of death of {Topic.Name}?",
                _ => throw new ArgumentException($"Unknown field: {Field}"),
            };
        }
    }
}
