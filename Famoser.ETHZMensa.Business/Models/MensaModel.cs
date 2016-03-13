using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Models
{
    class MensaModel
    {
        public string OpeningTimes { get; set; }
        public ObservableCollection<MenuModel> MenuMittags { get; set; }
        public ObservableCollection<MenuModel> MenuAbends { get; set; }
    }
}
