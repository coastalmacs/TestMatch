using System;

namespace TestMatch.Models
{
    public enum Legality
    {
        Legal,
        NoBall,
        Wide
    }

    public class Delivery
    {
        // Core delivery attributes
        public int Speed { get; set; } = 0;
        public int Line { get; set; } = 0;
        public int Length { get; set; } = 0;
        public string Movement { get; set; } = string.Empty;

        // Attributes accepted as inputs
        public bool Effort { get; set; } = false;
        public bool Variation { get; set; } = false;
        
        public Legality Legality { get; set; } = Legality.Legal;

        public Delivery()
        {
        }

        public void Bowl(Player bowler, bool effort, bool variation, int target)
        {
            Effort = effort;
            Variation = variation;

            // 1. Determine no ball based on bowler pace and effort
            // Pace >= 4 is a quick/pace bowler, Pace <= 3 is a spin bowler
            int noBallRange = bowler.Pace >= 4 ? 100 : 200;
            if (effort)
            {
                noBallRange /= 2;
            }
            bool isNoBall = Random.Shared.Next(0, noBallRange) == 0;

            if (isNoBall)
            {
                Legality = Legality.NoBall;
            }

            // 2. Calculate speed
            int calculatedSpeed = bowler.Pace;
            if (effort)
            {
                calculatedSpeed += 1;
            }
            if (bowler.Health >= 1 && bowler.Health <= 3)
            {
                calculatedSpeed -= 2;
            }
            else if (bowler.Health >= 4 && bowler.Health <= 6)
            {
                calculatedSpeed -= 1;
            }
            Speed = Math.Clamp(calculatedSpeed, 1, 9);

            // 3. Calculate length
            // Map target (1 to 9) to base length:
            // Short: 7, 8, 9 -> length = 8
            // Good: 4, 5, 6 -> length = 5
            // Full: 1, 2, 3 -> length = 2
            int baseLength = target switch
            {
                7 or 8 or 9 => 8,
                4 or 5 or 6 => 5,
                1 or 2 or 3 => 2,
                _ => 5
            };

            // Accuracy and health modifier (50/50 chance of being applied)
            int lengthModifier = 0;
            if (Random.Shared.Next(0, 2) == 0)
            {
                int accuracyHealthSum = bowler.Accuracy + bowler.Health;
                lengthModifier = accuracyHealthSum switch
                {
                    >= 2 and <= 6 => 3,
                    >= 7 and <= 11 => 2,
                    >= 12 and <= 16 => 1,
                    >= 17 and <= 18 => 0,
                    _ => 0
                };
            }

            // Randomize fuller or shorter deviation (positive or negative length modifier)
            int lengthDeviation = Random.Shared.Next(-lengthModifier, lengthModifier + 1);
            int finalLength = baseLength + lengthDeviation;

            if (finalLength < 1)
            {
                Legality = Legality.NoBall;
            }
            else if (finalLength > 9)
            {
                if (Legality != Legality.NoBall)
                {
                    Legality = Legality.Wide;
                }
            }
            Length = Math.Clamp(finalLength, 1, 9);

            // 4. Calculate line
            // Map target (1 to 9) to base line:
            // Wide: 1, 4, 7 -> line = 2
            // Off: 2, 5, 8 -> line = 5
            // Leg: 3, 6, 9 -> line = 8
            int baseLine = target switch
            {
                1 or 4 or 7 => 2,
                2 or 5 or 8 => 5,
                3 or 6 or 9 => 8,
                _ => 5
            };

            int lineModifier = 0;
            if (Random.Shared.Next(0, 2) == 0)
            {
                int accuracyHealthSum = bowler.Accuracy + bowler.Health;
                lineModifier = accuracyHealthSum switch
                {
                    >= 2 and <= 6 => 3,
                    >= 7 and <= 11 => 2,
                    >= 12 and <= 16 => 1,
                    >= 17 and <= 18 => 0,
                    _ => 0
                };
            }

            int lineDeviation = Random.Shared.Next(-lineModifier, lineModifier + 1);
            int finalLine = baseLine + lineDeviation;

            if (finalLine < 1 || finalLine > 9)
            {
                if (Legality != Legality.NoBall)
                {
                    Legality = Legality.Wide;
                }
            }
            Line = Math.Clamp(finalLine, 1, 9);

            // 5. Calculate Movement
            Movement = bowler.VariationBall;
        }

        public override string ToString()
        {
            string legalityStr = Legality switch
            {
                Legality.Legal => "Legal",
                Legality.NoBall => "No-Ball",
                Legality.Wide => "Wide",
                _ => "Unknown"
            };
            return $"Speed {Speed} Line {Line} Length {Length} Movement {Movement} ({legalityStr})";
        }
    }
}