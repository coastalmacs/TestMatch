using System.Collections.Generic;

namespace TestMatch.Models
{
    public class Innings
    {
        public Player bowler = new Player("Pat", "Cummins", true, 9, 8, 8, 6, "straight", "short", 8, 7, "outswinger", "inswinger");

        public int ballCondition = 9;
        public int pitchCondition;
        public int weather;
        public bool startingEnd = true;

        public List<Over> Overs { get; set; } = new List<Over>();

        public decimal OverProgress
        {
            get
            {
                if (Overs.Count == 0) return 0.0m;

                int completedOvers = Overs.Count - 1;
                var currentOver = Overs[Overs.Count - 1];
                int currentLegalBalls = currentOver.Deliveries.Count(d => d.Legality == Legality.Legal);

                if (currentOver.IsComplete)
                {
                    completedOvers++;
                    currentLegalBalls = 0;
                }

                return completedOvers + (currentLegalBalls * 0.1m);
            }
        }

        public bool CanBowl(Player bowlerToUse, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 1. Check if the active over is in progress and bowler is being changed
            if (Overs.Count > 0 && !Overs[Overs.Count - 1].IsComplete)
            {
                var activeOverBowler = Overs[Overs.Count - 1].Bowler;
                if (activeOverBowler.Name != bowlerToUse.Name)
                {
                    errorMessage = $"Cannot change bowlers mid-over. Active over belongs to {activeOverBowler.Name}.";
                    return false;
                }
            }

            // 2. Check if the bowler is trying to bowl consecutive completed overs
            if (Overs.Count > 0 && Overs[Overs.Count - 1].IsComplete && Overs[Overs.Count - 1].Bowler.Name == bowlerToUse.Name)
            {
                errorMessage = $"Bowler {bowlerToUse.Name} cannot bowl consecutive overs.";
                return false;
            }

            return true;
        }

        public Delivery? Bowl(bool effort, bool variation, int target)
        {
            if (!CanBowl(bowler, out _))
            {
                return null;
            }

            // Start a new over if none exist, or the latest one is complete
            if (Overs.Count == 0 || Overs[Overs.Count - 1].IsComplete)
            {
                Overs.Add(new Over(bowler));
            }

            var currentOver = Overs[Overs.Count - 1];
            var activeBowler = currentOver.Bowler;

            // Recalculate active bowler's health dynamically before delivering the ball
            activeBowler.Health = GetBowlerHealth(activeBowler);

            // Create the delivery
            var delivery = new Delivery();
            delivery.Bowl(activeBowler, effort, variation, target);
            currentOver.Deliveries.Add(delivery);

            return delivery;
        }

        public int GetBowlerHealth(Player player)
        {
            int health = 9;
            int spellCount = 0;
            int consecutiveOversByOthers = 0;
            int oversByOthersForRest = 0;

            foreach (var ov in Overs)
            {
                if (ov.Bowler.Name == player.Name)
                {
                    spellCount++;
                    if (spellCount >= player.Fitness)
                    {
                        health--;
                    }
                    consecutiveOversByOthers = 0;
                    oversByOthersForRest = 0;
                }
                else
                {
                    if (spellCount > 0)
                    {
                        consecutiveOversByOthers++;
                        if (consecutiveOversByOthers >= 2)
                        {
                            spellCount = 0;
                            oversByOthersForRest = 2;
                        }
                    }
                    else
                    {
                        oversByOthersForRest++;
                    }

                    if (oversByOthersForRest >= 2)
                    {
                        health++;
                        oversByOthersForRest -= 2;
                    }
                }

                health = System.Math.Clamp(health, 1, 9);
            }

            return health;
        }
    }
}