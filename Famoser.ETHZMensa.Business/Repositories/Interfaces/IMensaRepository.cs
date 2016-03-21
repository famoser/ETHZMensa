using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;

namespace Famoser.ETHZMensa.Business.Repositories.Interfaces
{
    public interface IMensaRepository
    {
        Task<ObservableCollection<LocationModel>> GetLocations();
        LocationModel GetFavorites();
        ObservableCollection<LocationModel> GetExampleLocations();

        Task<bool> Refresh();
        Task<bool> SaveState();
    }
}
