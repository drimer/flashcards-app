namespace FlashcardApp.Service;

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
        else if (question.Field == "name")
        {
            return question.Pokemon.Name.Equals(answer.Value.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            throw new NotSupportedException($"Field {question.Field} is not supported for Pokemon questions.");
        }
    }
}