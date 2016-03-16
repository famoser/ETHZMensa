using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Enums;
using Famoser.ETHZMensa.Business.Models.Base;

namespace Famoser.ETHZMensa.Business.Models
{
    public class MensaModel : ModelBase
    {
        public MensaModel()
        {
            Menus = new ObservableCollection<MenuModel>();
        }

        public string Name { get; set; }
        public string MealTime { get; set; }

        public Uri TodayUrl { get; set; }
        public Uri LogicUrl { get; set; }
        public Uri InfoUrl { get; set; }

        private DateTime _lastTimeRefreshed;
        public DateTime LastTimeRefreshed
        {
            get { return _lastTimeRefreshed; }
            set { Set(ref _lastTimeRefreshed, value); }
        }

        public LocationType Type { get; set; }

        public ObservableCollection<MenuModel> Menus { get; set; }
    }
}
