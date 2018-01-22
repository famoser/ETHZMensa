using System;
using System.Collections.ObjectModel;
using System.Linq;
using Famoser.ETHZMensa.Business.Enums;
using Famoser.ETHZMensa.Business.Helpers;
using Famoser.ETHZMensa.Business.Models.Base;

namespace Famoser.ETHZMensa.Business.Models
{
    public class MensaModel : ModelBase
    {
        public MensaModel()
        {
            Menus = new ObservableCollection<MenuModel>();
        }

        /* start deserialized properties */
        public string Name { get; set; }
        public string MealTime { get; set; }
        public string IdSlug { get; set; }
        public string TimeSlug { get; set; }
        public string ApiUrlSlug { get; set; }
        public string InfoUrlSlug { get; set; }
        public bool InfoDayDependent { get; set; }
        public LocationType Type { get; set; }
        /* end deserialized properties */

        public Uri TodayApiUrl => UriHelper.GetTodayApiUrl(this);
        public Uri TodayMenuUrl => UriHelper.GetTodayMenuUrl(this);
        public Uri InfoUrl => UriHelper.GetInfoUrl(this);

        private DateTime _lastTimeRefreshed;
        public DateTime LastTimeRefreshed
        {
            get => _lastTimeRefreshed;
            set
            {
                if (Set(ref _lastTimeRefreshed, value))
                {
                    RaisePropertyChanged(() => HasMenus);
                }
            }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set { Set(ref _isFavorite, value); }
        }

        public bool HasMenus => LastTimeRefreshed > DateTime.Today && Menus.Any();

        public ObservableCollection<MenuModel> Menus { get; set; }
    }
}
