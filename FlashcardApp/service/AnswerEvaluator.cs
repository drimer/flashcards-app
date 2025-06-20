namespace FlashcardApp.Service;

using FlashcardApp.Model;

public class AnswerEvaluator
{
    public bool IsCorrect(IQuestion question, PokemonAnswer answer)
    {
        return question switch
        {
            PokemonQuestion pokemonQuestion => IsCorrectPokemonAnswer(pokemonQuestion, answer),
            _ => throw new NotSupportedException($"Question type {question.GetType().Name} is not supported."),
        };
    }

    private bool IsCorrectPokemonAnswer(PokemonQuestion question, PokemonAnswer answer)
    {
        if (question.Field == "type")
        {
            return question.Pokemon.Types.Contains(answer.Value.Trim().ToLower());
        }
        else if (question.Field == "hp")
        {
            return int.Parse(answer.Value) == question.Pokemon.Hp;
        }
        else
        {
            throw new NotSupportedException($"Field {question.Field} is not supported for Pokemon questions.");
        }
    }
}