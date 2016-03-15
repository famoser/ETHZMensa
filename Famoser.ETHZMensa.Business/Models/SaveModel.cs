using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models
{
    public class SaveModel
    {
        public SaveModel()
        {
            Locations = new ObservableCollection<LocationModel>();
        }

        public ObservableCollection<LocationModel> Locations { get; set; }
        public int Version { get; set; }
    }
}
