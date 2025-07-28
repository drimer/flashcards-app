namespace FlashcardApp.Model
{
    public interface IQuestion
    {
        string Type { get; }
        ITopic GetTopic();
        string AsString();
        string Field { get; init; }
    }
}
