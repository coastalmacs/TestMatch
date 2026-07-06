namespace TestMatch.Models
{
    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public int Pace { get; set; } = 0;
        public int Accuracy { get; set; } = 0;
        public int Fitness { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public int Variation { get; set; } = 0;

        public int Health { get; set; } = 0;


        public Player()
        {

        }

        public Player(string name, int pace, int accuracy, int fitness, int stock, int variation)
        {
            Name = name;
            Pace = pace;
            Accuracy = accuracy;
            Fitness = fitness;
            Stock = stock;
            Variation = variation;

            // Set intial health value based on fitness to modified as game progresses
            Health = fitness;
        }

    }
}