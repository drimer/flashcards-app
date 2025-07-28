using System.Globalization;
using FlashcardApp.Model;

namespace FlashcardApp.Service
{
    public class HistoricalFigureAnswerEvaluator : IAnswerEvaluator
    {
        public bool IsCorrect(IQuestion question, Answer answer)
        {
            try
            {
                HistoricalFigureQuestion hfQuestion = question as HistoricalFigureQuestion ?? throw new ArgumentException("The question is not a HistoricalFigureQuestion.", nameof(question));
                return hfQuestion.Field.ToLower(CultureInfo.InvariantCulture) switch
                {
                    "conflicts" => hfQuestion.Topic.Conflicts.Any(c => StringsAreNotVeryDifferent(c, answer.Value)),
                    "occupation" => hfQuestion.Topic.Occupation.Any(o => o.Equals(answer.Value.Trim(), StringComparison.OrdinalIgnoreCase)),
                    "causeofdeath" => StringsAreNotVeryDifferent(hfQuestion.Topic.CauseOfDeath, answer.Value),
                    _ => throw new NotSupportedException($"Field {hfQuestion.Field} is not supported for Historical Figure questions."),
                };
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("The question is not a Pokemon question.", nameof(question));
            }
        }

        private static bool StringsAreNotVeryDifferent(string str1, string str2)
        {
            List<string> diff;
            IEnumerable<string> set1 = str1.Trim().ToLower(CultureInfo.InvariantCulture).ToCharArray().Distinct().Select(c => c.ToString());
            IEnumerable<string> set2 = str2.Trim().ToLower(CultureInfo.InvariantCulture).ToCharArray().Distinct().Select(c => c.ToString());

            diff = set2.Count() > set1.Count() ? [.. set2.Except(set1)] : [.. set1.Except(set2)];

            // Allow for a 20% difference. This is one typo per 5-letter word on average.
            return diff.Count / (double)Math.Max(set1.Count(), set2.Count()) < 0.2;
        }
    }
}
