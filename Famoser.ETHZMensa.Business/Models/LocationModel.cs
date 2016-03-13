using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models
{
    public class LocationModel
    {
        public ObservableCollection<MensaModel> Mensas { get; set; }
    }
}
