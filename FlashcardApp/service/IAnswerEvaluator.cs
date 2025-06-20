namespace FlashcardApp.Service;

using FlashcardApp.Model;

public interface IAnswerEvaluator
{
    bool IsCorrect(IQuestion question, PokemonAnswer answer);
}