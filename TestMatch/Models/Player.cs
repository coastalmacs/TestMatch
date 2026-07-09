namespace TestMatch.Models
{
    public class Player
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Name => $"{FirstName} {LastName}".Trim();

        // Batting attributes
        public int Concentration { get; set; } = 0;
        public int Aggression { get; set; } = 0;
        public string BattingStrength { get; set; } = string.Empty;
        public string BattingWeakness { get; set; } = string.Empty;
        
        // Bowling attributes
        public int Pace { get; set; } = 0;
        public int Accuracy { get; set; } = 0;
        public string StockBall { get; set; } = string.Empty;
        public string VariationBall { get; set; } = string.Empty;
        
        // Fielding attributes
        public int Fielding { get; set; } = 0;
        public int Catching { get; set; } = 0;
        public int Keeping { get; set; } = 0;
        public int Captaincy { get; set; } = 0;

        // General attributes
        public int Fitness { get; set; } = 0;
        public int Health { get; set; } = 9;
        public bool RightHanded { get; set; } = true;
        public int BattingOrder { get; set; } = 0;

        public Player()
        {
        }

        public Player(
            string lastName,
            string firstName,
            int concentration,
            int aggression,
            string battingStrength,
            string battingWeakness,
            int pace,
            int accuracy,
            string stockBall,
            string variationBall,
            int fielding,
            int catching,
            int keeping,
            int captaincy,
            int fitness,
            bool rightHanded,
            int battingOrder)
        {
            LastName = lastName;
            FirstName = firstName;
            Concentration = concentration;
            Aggression = aggression;
            BattingStrength = battingStrength;
            BattingWeakness = battingWeakness;
            Pace = pace;
            Accuracy = accuracy;
            StockBall = stockBall;
            VariationBall = variationBall;
            Fielding = fielding;
            Catching = catching;
            Keeping = keeping;
            Captaincy = captaincy;
            Fitness = fitness;
            RightHanded = rightHanded;
            BattingOrder = battingOrder;

            Health = 9;
        }
    }
}