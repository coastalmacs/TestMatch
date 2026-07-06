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
        public int Speed { get; set; } = 0;
        public int Line { get; set; } = 0;
        public int Length { get; set; } = 0;
        public int Movement { get; set; } = 0;

        public bool Effort { get; set; } = false;
        public Legality Legality { get; set; } = Legality.Legal;

        // Keep IsLegal for backward compatibility with Innings and Over logic
        public bool IsLegal => Legality == Legality.Legal;

        public Delivery()
        {
        }

        public Delivery(Player bowler, bool effort, int target)
        {
            Effort = effort;

            // Calculate attributes using bowler stats and effort
            int effortModifier = effort ? 10 : 0;

            Speed = bowler.Pace + effortModifier;
            Line = bowler.Accuracy + effortModifier;
            Length = bowler.Fitness + effortModifier;
            Movement = bowler.Variation + effortModifier;

            // Determine no-balls based on bowler pace and effort
            int noBallRange = bowler.Pace >= 6 ? 100 : (bowler.Pace <= 3 ? 200 : 150);
            if (effort)
            {
                noBallRange /= 2;
            }
            bool isNoBall = Random.Shared.Next(0, noBallRange) == 0;

            if (isNoBall)
            {
                Legality = Legality.NoBall;
            }
            else
            {
                // Determine wides: can only occur if target is off side (1,4,7) or leg side (3,6,9)
                bool canBeWide = target == 1 || target == 4 || target == 7 || target == 3 || target == 6 || target == 9;
                if (canBeWide)
                {
                    int wideRange = bowler.Pace >= 6 ? 200 : (bowler.Pace <= 3 ? 500 : 350);
                    bool isWide = Random.Shared.Next(0, wideRange) == 0;
                    Legality = isWide ? Legality.Wide : Legality.Legal;
                }
                else
                {
                    Legality = Legality.Legal;
                }
            }
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