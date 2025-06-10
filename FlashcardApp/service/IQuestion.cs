using FlashcardApp.Model;

namespace FlashcardApp.Service;

public interface IQuestion
{
    string Type { get; }
    ITopic getTopic();
    string AsString();
    string Field { get; init; }
}
