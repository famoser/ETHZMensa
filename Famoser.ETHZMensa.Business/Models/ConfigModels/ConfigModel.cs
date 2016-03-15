using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class ConfigModel
    {
        public List<LocationConfigModel> Locations { get; set; }
        public int Version { get; set; }
    }
}
