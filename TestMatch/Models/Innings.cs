using System.Collections.Generic;

namespace TestMatch.Models
{
    public class Innings
    {
        public Player bowler = new Player("Cummins", 9, 9, 9, 9, 9);

        public int ballCondition = 9;
        public int pitchCondition;
        public int weather;

        public List<Over> Overs { get; set; } = new List<Over>();

        public decimal OverProgress
        {
            get
            {
                if (Overs.Count == 0) return 0.0m;

                int completedOvers = Overs.Count - 1;
                var currentOver = Overs[Overs.Count - 1];
                int currentLegalBalls = currentOver.Deliveries.Count(d => d.IsLegal);

                if (currentOver.IsComplete)
                {
                    completedOvers++;
                    currentLegalBalls = 0;
                }

                return completedOvers + (currentLegalBalls * 0.1m);
            }
        }

        public Delivery Bowl(bool effort, int target)
        {
            // Start a new over if none exist, or the latest one is complete
            if (Overs.Count == 0 || Overs[Overs.Count - 1].IsComplete)
            {
                Overs.Add(new Over(bowler));
            }

            var currentOver = Overs[Overs.Count - 1];

            // Create the delivery
            var delivery = new Delivery(bowler, effort, target);  
            currentOver.Deliveries.Add(delivery);

            return delivery;
        }
    }
}