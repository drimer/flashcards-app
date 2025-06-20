namespace FlashcardApp.Service;

using FlashcardApp.Model;

public class HistoricalFigureAnswerEvaluator : IAnswerEvaluator
{
    public bool IsCorrect(IQuestion question, Answer answer)
    {
        try
        {
            var hfQuestion = question as HistoricalFigureQuestion ?? throw new ArgumentException("The question is not a HistoricalFigureQuestion.", nameof(question));
            switch (hfQuestion.Field.ToLower())
            {
                case "conflicts":
                    return hfQuestion.Topic.Conflicts.Any(c => StringsAreNotVeryDifferent(c, answer.Value));
                case "occupation":
                    return hfQuestion.Topic.Occupation.Any(o => o.Equals(answer.Value.Trim(), StringComparison.OrdinalIgnoreCase));
                case "causeofdeath":
                    return StringsAreNotVeryDifferent(hfQuestion.Topic.CauseOfDeath, answer.Value);
                default:
                    throw new NotSupportedException($"Field {hfQuestion.Field} is not supported for Historical Figure questions.");
            }
        }
        catch (InvalidCastException)
        {
            throw new ArgumentException("The question is not a Pokemon question.", nameof(question));
        }
    }

    private bool StringsAreNotVeryDifferent(string str1, string str2)
    {
        List<string> diff;
        IEnumerable<string> set1 = str1.Trim().ToLower().ToCharArray().Distinct().Select(c => c.ToString());
        IEnumerable<string> set2 = str2.Trim().ToLower().ToCharArray().Distinct().Select(c => c.ToString());

        if (set2.Count() > set1.Count())
        {
            diff = set2.Except(set1).ToList();
        }
        else
        {
            diff = set1.Except(set2).ToList();
        }

        // Allow for a 20% difference. This is one typo per 5-letter word on average.
        return diff.Count() / (double)Math.Max(set1.Count(), set2.Count()) < 0.2;
    }
}