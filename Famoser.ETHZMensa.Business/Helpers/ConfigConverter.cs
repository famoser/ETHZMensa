using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Models.ConfigModels;
using Famoser.FrameworkEssentials.Singleton;

namespace Famoser.ETHZMensa.Business.Helpers
{
    public class ConfigConverter : SingletonBase<ConfigConverter>
    {
        public MensaModel ConvertToModel(MensaConfigModel config)
        {
            return new MensaModel()
            {
                InfoUrl = new Uri(config.InfoUrl),
                Name = config.Name,
                TodayUrl = new Uri(config.TodayUrl)
            };
        }

        public LocationModel ConvertToModel(List<MensaConfigModel> mensas, string location)
        {
            var model = new LocationModel()
            {
                Name =  location
            };
            foreach (var mensaConfigModel in mensas)
            {
                model.Mensas.Add(ConvertToModel(mensaConfigModel));
            }
            return model;
        }
    }
}
