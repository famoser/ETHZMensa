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
                Name = config.Name,
                MealTime = config.MealTime,
                Type =  config.Type,
                ApiUrlSlug = config.ApiUrlSlug,
                InfoUrlSlug = config.InfoUrlSlug,
                IdSlug = config.IdSlug,
                TimeSlug = config.TimeSlug,
                InfoDayDependent = config.InfoDayDependent
            };
        }

        public LocationModel ConvertToModel(LocationConfigModel config)
        {
            var model = new LocationModel()
            {
                Name = config.Name,
                Mensas = ConvertToModel(config.Mensas)
            };
            return model;
        }

        public ObservableCollection<MensaModel> ConvertToModel(List<MensaConfigModel> config)
        {
            var list = new ObservableCollection<MensaModel>();
            foreach (var mensaConfigModel in config)
            {
                list.Add(ConvertToModel(mensaConfigModel));
            }
            return list;
        }
    }
}
