using System.Collections.Generic;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class LocationConfigModel
    {
        public string Name { get; set; }
        public List<MensaConfigModel> Mensas { get; set; }
    }
}
