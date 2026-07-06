using System;
using System.Collections.Generic;
using System.IO;

namespace TestMatch.Models
{
    public class Team
    {
        public string Name { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new List<Player>();

        public Team()
        {
        }

        public Team(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Loads a Team sheet from a CSV file.
        /// </summary>
        /// <param name="filePath">Path to the CSV file.</param>
        /// <returns>A populated Team object.</returns>
        public static Team LoadFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Team sheet file not found: {filePath}");
            }

            string teamName = Path.GetFileNameWithoutExtension(filePath);
            if (!string.IsNullOrEmpty(teamName))
            {
                teamName = char.ToUpper(teamName[0]) + teamName.Substring(1);
            }

            Team team = new Team(teamName);

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
            {
                return team; // Empty file or header only
            }

            // Line 0 is header: Name,Pace,Accuracy,Fitness,Stock,Variation
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue; // Skip blank lines
                }

                string[] parts = line.Split(',');
                if (parts.Length >= 6)
                {
                    string name = parts[0].Trim();
                    int pace = int.TryParse(parts[1].Trim(), out int p) ? p : 0;
                    int accuracy = int.TryParse(parts[2].Trim(), out int a) ? a : 0;
                    int fitness = int.TryParse(parts[3].Trim(), out int f) ? f : 0;
                    int stock = int.TryParse(parts[4].Trim(), out int s) ? s : 0;
                    int variation = int.TryParse(parts[5].Trim(), out int v) ? v : 0;

                    Player player = new Player(name, pace, accuracy, fitness, stock, variation);
                    team.Players.Add(player);
                }
            }

            return team;
        }
    }
}
