namespace EvoSC.Common.Util.Algorithms;

public static class StringEditDistance
{
    /// <summary>
    /// Find the number of operations required to edit one string to another.
    /// Essentially how "similar" are the two strings.
    /// O(n*m + n + m) - DP Levenshtein distance
    /// </summary>
    /// <param name="search">The text to search for.</param>
    /// <param name="text">The text to search in.</param>
    /// <returns></returns>
    public static int GetDistance(string search, string text)
    {
        var distances = new int[search.Length + 1, text.Length + 1];

        if (search.Length == 0)
        {
            return text.Length;
        }

        if (text.Length == 0)
        {
            return search.Length;
        }
        
        for (int i = 1; i <= search.Length; i++)
        {
            distances[i, 0] = i;
        }

        for (int i = 1; i <= text.Length; i++)
        {
            distances[0, i] = i;
        }

        for (int j = 1; j <= text.Length; j++)
        {
            for (int i = 1; i <= search.Length; i++)
            {
                var cost = text[j - 1] == search[i - 1] ? 0 : 1;
                var deletionDistance = distances[i - 1, j] + 1;
                var insertionDistance = distances[i, j - 1] + 1;
                var substitutionDistance = distances[i - 1, j - 1] + cost;

                distances[i, j] = Math.Min(deletionDistance, Math.Min(insertionDistance, substitutionDistance));
            }
        }
        
        return distances[search.Length, text.Length];
    }
}
