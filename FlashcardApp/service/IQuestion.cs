using FlashcardApp.Model;

namespace FlashcardApp.Service;

public interface IQuestion
{
    ITopic getTopic();
    string AsString();
}
