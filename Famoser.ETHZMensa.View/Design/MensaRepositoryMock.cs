using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;

namespace Famoser.ETHZMensa.View.Design
{
    public class MensaRepositoryMock : IMensaRepository
    {
        public async Task<ObservableCollection<LocationModel>> GetLocations()
        {
            return new ObservableCollection<LocationModel>()
            {
                GetExampleLocationModel(),
                GetExampleLocationModel(),
                GetExampleLocationModel()
            };
        }

        public LocationModel GetFavorites()
        {
            return GetExampleLocationModel();
        }

        private LocationModel GetExampleLocationModel()
        {
            return new LocationModel()
            {
                Name = "Hönggerbärg",
                Mensas = new ObservableCollection<MensaModel>()
                {
                    GetExampleMensaModel(),
                    GetExampleMensaModel(),
                    GetExampleMensaModel(),
                    GetExampleMensaModel()
                }
            };
        }

        private MensaModel GetExampleMensaModel()
        {
            return new MensaModel()
            {
                Name = "Clasiusbar",
                MealTime = "07:30 - 19:30",
                LastTimeRefreshed = DateTime.Now,
                Menus = new ObservableCollection<MenuModel>()
                {
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel()
                }
            };
        }

        private MenuModel GetExampleMenuModel()
        {
            return new MenuModel()
            {
                Title = "Salat",
                Description =
                    "Exotischer Fruchtsalat mit Kokosnussmilch und Palmzucker\auf Wunsch mit zusätzlichen Vitamenin",
                MenuName = "Salat Buah",
                Prices = "3.30/3.30/4.50"
            };
        }

        public async Task<bool> Refresh()
        {
            return true;
        }

        public async Task<bool> SaveState()
        {
            return true;
        }
    }
}
