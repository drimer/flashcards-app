namespace FlashcardApp.Model;

public sealed class HistoricalFigureQuestion : IQuestion
{
    public string Type => "HistoricalFigureQuestion";
    public HistoricalFigure Topic { get; init; }
    public string Field { get; init; }

    public HistoricalFigureQuestion(HistoricalFigure topic, string field)
    {
        Topic = topic;
        Field = field;
    }

    public ITopic getTopic() => Topic;
    public string AsString()
    {
        return Field.ToLower() switch
        {
            "conflicts" => $"Name one conflict where {Topic.Name} was involved in.",
            "occupation" => $"Name one occupation of {Topic.Name}.",
            "causeofdeath" => $"What was the cause of death of {Topic.Name}?",
            _ => throw new ArgumentException($"Unknown field: {Field}")
        };
    }
}