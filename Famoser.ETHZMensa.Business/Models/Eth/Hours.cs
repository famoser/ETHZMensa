using System.Collections.Generic;

namespace Famoser.ETHZMensa.Business.Models.Eth
{
    public class Hours
    {
        public List<Opening> opening { get; set; }
        public List<Mealtime> mealtime { get; set; }
    }
}