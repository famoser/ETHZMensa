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

        public Uri TodayUrl { get; set; }
        public Uri InfoUrl { get; set; }

        private string _openingTimes;
        public string OpeningTimes
        {
            get { return _openingTimes; }
            set { Set(ref _openingTimes, value); }
        }

        private ObservableCollection<MenuModel> _menuMittags;
        public ObservableCollection<MenuModel> MenuMittags
        {
            get { return _menuMittags; }
            set { Set(ref _menuMittags, value); }
        }

        private ObservableCollection<MenuModel> _menuAbends;
        public ObservableCollection<MenuModel> MenuAbends
        {
            get { return _menuAbends; }
            set { Set(ref _menuAbends, value); }
        }
    }
}
