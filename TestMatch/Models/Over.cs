using System.Collections.Generic;
using System.Linq;

namespace TestMatch.Models
{
    public class Over
    {
        public Player Bowler { get; set; }
        public List<Delivery> Deliveries { get; set; } = new List<Delivery>();

        public Over(Player bowler)
        {
            Bowler = bowler;
        }

        // An over is complete when there are at least 6 legal deliveries
        public bool IsComplete => Deliveries.Count(d => d.IsLegal) >= 6;
    }
}
