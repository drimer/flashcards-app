using FlashcardApp.Model;

namespace FlashcardApp.Service;

public interface IAnswerEvaluator
{
    bool IsCorrect(IQuestion question, Answer answer);
}
