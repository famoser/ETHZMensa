using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class MensaConfigModel
    {
        public string Name { get; set; }
        public string MealTime { get; set; }
        
        public string TodayUrl { get; set; }
        public string InfoUrl { get; set; }
    }
}
