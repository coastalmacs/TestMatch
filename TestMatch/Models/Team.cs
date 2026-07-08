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

            // Line 0 is header: Name,RightHanded,Fitness,BattingOrder,Concentration,Aggression,BattingStrength,BattingWeakness,Pace,Accuracy,StockBall,VariationBall
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue; // Skip blank lines
                }

                string[] parts = line.Split(',');
                if (parts.Length < 13)
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Expected 13 values, but got {parts.Length}.");
                }

                string firstName = parts[0].Trim();
                if (string.IsNullOrEmpty(firstName))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Player FirstName cannot be empty.");
                }

                string lastName = parts[1].Trim();
                if (string.IsNullOrEmpty(lastName))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Player LastName cannot be empty.");
                }

                if (!bool.TryParse(parts[2].Trim(), out bool rightHanded))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse RightHanded boolean value '{parts[2]}'.");
                }

                if (!int.TryParse(parts[3].Trim(), out int fitness))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Fitness integer '{parts[3]}'.");
                }

                if (!int.TryParse(parts[4].Trim(), out int battingOrder))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse BattingOrder integer '{parts[4]}'.");
                }

                if (!int.TryParse(parts[5].Trim(), out int concentration))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Concentration integer '{parts[5]}'.");
                }

                if (!int.TryParse(parts[6].Trim(), out int aggression))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Aggression integer '{parts[6]}'.");
                }

                string battingStrength = parts[7].Trim();
                string battingWeakness = parts[8].Trim();

                if (!int.TryParse(parts[9].Trim(), out int pace))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Pace integer '{parts[9]}'.");
                }

                if (!int.TryParse(parts[10].Trim(), out int accuracy))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Accuracy integer '{parts[10]}'.");
                }

                string stockBall = parts[11].Trim();
                string variationBall = parts[12].Trim();

                Player player = new Player(firstName, lastName, rightHanded, fitness, battingOrder, concentration, aggression, battingStrength, battingWeakness, pace, accuracy, stockBall, variationBall);
                team.Players.Add(player);
            }

            if (team.Players.Count < 11)
            {
                throw new InvalidDataException($"Team '{teamName}' must have at least 11 players. Loaded {team.Players.Count}.");
            }

            // Verify batting order sequentially from 1 to 11
            var sortedPlayers = System.Linq.Enumerable.ToList(System.Linq.Enumerable.OrderBy(team.Players, p => p.BattingOrder));
            for (int j = 0; j < 11; j++)
            {
                if (sortedPlayers[j].BattingOrder != j + 1)
                {
                    throw new InvalidDataException($"Invalid batting order sequence: Expected batting position {j + 1}.");
                }
            }

            return team;
        }
    }
}
