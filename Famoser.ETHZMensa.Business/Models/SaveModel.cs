using System.Collections.ObjectModel;

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
