namespace FlashcardApp.Service;

using FlashcardApp.Model;

public class PokemonAnswerEvaluator : IAnswerEvaluator
{
    public bool IsCorrect(IQuestion question, PokemonAnswer answer)
    {
        try
        {
            var pokemonQuestion = question as PokemonQuestion;

            if (pokemonQuestion.Field == "type")
            {
                return pokemonQuestion.Pokemon.Types.Contains(answer.Value.Trim().ToLower());
            }
            else if (pokemonQuestion.Field == "hp")
            {
                return int.Parse(answer.Value) == pokemonQuestion.Pokemon.Hp;
            }
            else
            {
                throw new NotSupportedException($"Field {pokemonQuestion.Field} is not supported for Pokemon questions.");
            }
        }
        catch (InvalidCastException)
        {
            throw new ArgumentException("The question is not a Pokemon question.", nameof(question));
        }
    }
}