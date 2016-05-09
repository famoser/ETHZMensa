using System.Collections.Generic;

namespace Famoser.ETHZMensa.Business.Models.Eth
{
    public class Meal
    {
        public int id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public List<string> description { get; set; }
        public int position { get; set; }
        public Prices prices { get; set; }
        public List<Allergen> allergens { get; set; }
        public List<object> origins { get; set; }
    }
}