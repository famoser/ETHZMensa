using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;

namespace Famoser.ETHZMensa.Business.Repositories.Interfaces
{
    public interface IMensaRepository
    {
        Task<ObservableCollection<LocationModel>> GetLocations();
        LocationModel GetFavorites();

        Task<bool> Refresh();
        Task<bool> SaveState();
    }
}
