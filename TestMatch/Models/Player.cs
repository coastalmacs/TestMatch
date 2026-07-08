namespace TestMatch.Models
{
    public class Player
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name => $"{FirstName} {LastName}".Trim();

        public bool RightHanded { get; set; } = true;
        public int Fitness { get; set; } = 0;
        public int BattingOrder { get; set; } = 0;
        public int Concentration { get; set; } = 0;
        public int Aggression { get; set; } = 0;
        public string BattingStrength { get; set; } = string.Empty;
        public string BattingWeakness { get; set; } = string.Empty;
        public int Pace { get; set; } = 0;
        public int Accuracy { get; set; } = 0;
        public string StockBall { get; set; } = string.Empty;
        public string VariationBall { get; set; } = string.Empty;

        public int Health { get; set; } = 9;

        public Player()
        {
        }

        public Player(
            string firstName,
            string lastName,
            bool rightHanded,
            int fitness,
            int battingOrder,
            int concentration,
            int aggression,
            string battingStrength,
            string battingWeakness,
            int pace,
            int accuracy,
            string stockBall,
            string variationBall)
        {
            FirstName = firstName;
            LastName = lastName;
            RightHanded = rightHanded;
            Fitness = fitness;
            BattingOrder = battingOrder;
            Concentration = concentration;
            Aggression = aggression;
            BattingStrength = battingStrength;
            BattingWeakness = battingWeakness;
            Pace = pace;
            Accuracy = accuracy;
            StockBall = stockBall;
            VariationBall = variationBall;

            Health = 9;
        }
    }
}