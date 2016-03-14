using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models.Base;

namespace Famoser.ETHZMensa.Business.Models
{
    public class MensaModel : ModelBase
    {
        public MensaModel()
        {
            MenuMittags = new ObservableCollection<MenuModel>();
            MenuAbends = new ObservableCollection<MenuModel>();
        }

        public string Name { get; set; }
        public string MealTime { get; set; }

        public Uri TodayUrl { get; set; }
        public Uri InfoUrl { get; set; }

        public ObservableCollection<MenuModel> MenuMittags { get; set; }
        public ObservableCollection<MenuModel> MenuAbends { get; set; }
    }
}
