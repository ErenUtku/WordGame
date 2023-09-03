using System.Collections.Generic;

public class ScoringSystem
{
    // Define a dictionary that maps letters to their point values.
    private Dictionary<char, int> letterValues = new Dictionary<char, int>
    {
        {'E', 1}, {'A', 1}, {'O', 1}, {'N', 1}, {'R', 1}, {'T', 1}, {'L', 1}, {'S', 1}, {'U', 1}, {'I', 1},
        {'D', 2}, {'G', 2},
        {'B', 3}, {'C', 3}, {'M', 3}, {'P', 3},
        {'F', 4}, {'H', 4}, {'V', 4}, {'W', 4}, {'Y', 4},
        {'K', 5},
        {'J', 8}, {'X', 8},
        {'Q', 10}, {'Z', 10}
    };

    public int CalculateWordScore(string word)
    {
        int score = 0;

        foreach (char letter in word.ToUpper()) // Convert the word to uppercase for consistency.
        {
            if (letterValues.ContainsKey(letter))
            {
                score += letterValues[letter];
            }
        }

        return score;
    }

    public int CalculateTotalScore(string[] words, int unusedLetterCount)
    {
        int totalScore = 0;

        foreach (string word in words)
        {
            int wordScore = CalculateWordScore(word);
            totalScore += wordScore * (10 * word.Length);
        }

        // Subtract 100 points for each unused letter at the end of the level.
        totalScore -= unusedLetterCount * 100;

        return totalScore;
    }
}