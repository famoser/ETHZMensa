using System.Collections.ObjectModel;
using Famoser.ETHZMensa.Business.Models.Base;

namespace Famoser.ETHZMensa.Business.Models
{
    public class LocationModel : ModelBase
    {
        public LocationModel()
        {
            Mensas = new ObservableCollection<MensaModel>();
        }

        public string Name { get; set; }
        public ObservableCollection<MensaModel> Mensas { get; set; }
    }
}
