using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpaceSurvival
{
    public class ScoreManager
    {
        private const string FilePath = "highscores.txt";
        public List<int> _highScores;

        public ScoreManager()
        {
            _highScores = new List<int>();
        }

        public List<int> LoadScores()
        {
            if (!File.Exists(FilePath))
            {
                return new List<int>();
            }

            try
            {
                return File.ReadAllLines(FilePath)
                           .Where(line => int.TryParse(line, out _)) // Filter valid numbers
                           .Select(int.Parse) // Parse valid numbers
                           .OrderByDescending(s => s) // Sort in descending order
                           .ToList();
            }
            catch (Exception ex)
            {
                // If there's an error, return an empty list
                Console.WriteLine($"Error loading scores: {ex.Message}");
                return new List<int>();
            }
        }

        public void SaveScore(int score)
        {
            _highScores.Add(score);  // Add the new score
            _highScores = _highScores.OrderByDescending(s => s).Take(5).ToList();  // Keep only the top 5 scores

            // Write the scores back to the file
            try
            {
                File.WriteAllLines(FilePath, _highScores.Select(s => s.ToString())); // Save to file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving scores: {ex.Message}");
            }                                                                       // Add code to save to a file if needed
        }
    }
}