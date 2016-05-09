using System.Collections.Generic;

namespace Famoser.ETHZMensa.Business.Models.Eth
{
    public class Menu
    {
        public string date { get; set; }
        public string day { get; set; }
        public List<Meal> meals { get; set; }
    }
}