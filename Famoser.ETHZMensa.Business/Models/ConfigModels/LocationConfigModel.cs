using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class LocationConfigModel
    {
        public string Name { get; set; }
        public List<MensaConfigModel> Mensas { get; set; }
    }
}
