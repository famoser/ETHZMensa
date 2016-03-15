using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Enums;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class MensaConfigModel
    {
        public LocationType Type { get; set; }

        public string Name { get; set; }
        public string MealTime { get; set; }
        
        public string InfoUrl { get; set; }
        public string LogicUrl { get; set; }
    }
}
