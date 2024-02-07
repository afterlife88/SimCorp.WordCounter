using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace SimCorp.WordCounter.Domain;

public static class WordFrequencyCounter
{
    public static ResultOr<IReadOnlyCollection<KeyValuePair<string, int>>> CountWords(IEnumerable<string> lines)
    {
        try
        {
            var wordCounts = new Dictionary<string, int>();

            foreach (var line in lines)
            {
                var words = Regex.Split(line.ToLowerInvariant(), @"\W+");
                foreach (var word in words)
                {
                    if (string.IsNullOrWhiteSpace(word))
                        continue;
                    wordCounts.TryGetValue(word, out var count);
                    wordCounts[word] = count + 1;
                }
            }

            var sortedWordCounts = wordCounts
                .OrderByDescending(pair => pair.Value)
                .ThenBy(pair => pair.Key)
                .ToImmutableList();

            return ResultOr<IReadOnlyCollection<KeyValuePair<string, int>>>.Success(sortedWordCounts);
        }
        catch (Exception e)
        {
            return ResultOr<IReadOnlyCollection<KeyValuePair<string, int>>>.Failure(e.Message);
        }
    }
}