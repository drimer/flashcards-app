using System.Globalization;
using FlashcardApp.Model;

namespace FlashcardApp.Service
{
    public class PokemonAnswerEvaluator : IAnswerEvaluator
    {
        public bool IsCorrect(IQuestion question, Answer answer)
        {
            try
            {
                PokemonQuestion? pokemonQuestion = question as PokemonQuestion;

                if (pokemonQuestion.Field == "type")
                {
                    return pokemonQuestion.Pokemon.Types.Contains(answer.Value.Trim().ToLower(CultureInfo.InvariantCulture));
                }
                else
                {
                    if (pokemonQuestion.Field == "hp")
                    {
                        return int.Parse(answer.Value, CultureInfo.InvariantCulture) == pokemonQuestion.Pokemon.Hp;
                    }
                    else
                    {
                        throw new NotSupportedException($"Field {pokemonQuestion.Field} is not supported for Pokemon questions.");
                    }
                }
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("The question is not a Pokemon question.", nameof(question));
            }
        }
    }
}
