namespace TestMatch.Models
{
    public class Delivery
    {
        public int Speed { get; set; } = 0;
        public int Line { get; set; } = 0;
        public int Length { get; set; } = 0;
        public int Movement { get; set; } = 0;

        public Delivery()
        {
        }

        private string Bowl(Player bowler, int effort, int target)
        {
            int speed = bowler.Pace + effort * 10;
            
            int modifier = Random.Shared.Next(1,10);
            
            int line = bowler.Accuracy + effort * 10;
            // int length = bowler.Fitness + effort * 10;
            // int movement = bowler.Variation + effort * 10;

            return $"Speed {speed} Line {line} Length {length} Movement {movement}";
        }



    }
}