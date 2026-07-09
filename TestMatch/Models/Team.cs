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

            // Line 0 is header: LastName,FirstName,Concentration,Aggression,BattingStrength,BattingWeakness,Pace,Accuracy,StockBall,VariationBall,Fielding,Catching,Keeping,Captaincy,Fitness,RightHanded,BattingOrder
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue; // Skip blank lines
                }

                string[] parts = line.Split(',');
                if (parts.Length < 17)
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Expected 17 values, but got {parts.Length}.");
                }

                string lastName = parts[0].Trim();
                if (string.IsNullOrEmpty(lastName))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Player LastName cannot be empty.");
                }

                string firstName = parts[1].Trim();
                if (string.IsNullOrEmpty(firstName))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Player FirstName cannot be empty.");
                }

                if (!int.TryParse(parts[2].Trim(), out int concentration))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Concentration integer '{parts[2]}'.");
                }

                if (!int.TryParse(parts[3].Trim(), out int aggression))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Aggression integer '{parts[3]}'.");
                }

                string battingStrength = parts[4].Trim();
                string battingWeakness = parts[5].Trim();

                if (!int.TryParse(parts[6].Trim(), out int pace))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Pace integer '{parts[6]}'.");
                }

                if (!int.TryParse(parts[7].Trim(), out int accuracy))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Accuracy integer '{parts[7]}'.");
                }

                string stockBall = parts[8].Trim();
                string variationBall = parts[9].Trim();

                if (!int.TryParse(parts[10].Trim(), out int fielding))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Fielding integer '{parts[10]}'.");
                }

                if (!int.TryParse(parts[11].Trim(), out int catching))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Catching integer '{parts[11]}'.");
                }

                if (!int.TryParse(parts[12].Trim(), out int keeping))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Keeping integer '{parts[12]}'.");
                }

                if (!int.TryParse(parts[13].Trim(), out int captaincy))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Captaincy integer '{parts[13]}'.");
                }

                if (!int.TryParse(parts[14].Trim(), out int fitness))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse Fitness integer '{parts[14]}'.");
                }

                if (!bool.TryParse(parts[15].Trim(), out bool rightHanded))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse RightHanded boolean value '{parts[15]}'.");
                }

                if (!int.TryParse(parts[16].Trim(), out int battingOrder))
                {
                    throw new InvalidDataException($"Line {i + 1} is invalid: Unable to parse BattingOrder integer '{parts[16]}'.");
                }

                Player player = new Player(lastName, firstName, concentration, aggression, battingStrength, battingWeakness, pace, accuracy, stockBall, variationBall, fielding, catching, keeping, captaincy, fitness, rightHanded, battingOrder);
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
