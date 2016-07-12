using System.Collections.Generic;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    internal class ConfigModel
    {
        public List<LocationConfigModel> Locations { get; set; }
        public int Version { get; set; }
        public LinkModel Links { get; set; }
    }
}
