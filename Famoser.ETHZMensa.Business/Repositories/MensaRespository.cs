using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Helpers;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Models.ConfigModels;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Business.Services;
using Famoser.ETHZMensa.Data.Services;
using Famoser.FrameworkEssentials.Logging;
using Newtonsoft.Json;

namespace Famoser.ETHZMensa.Business.Repositories
{
    public class MensaRespository : IMensaRepository
    {
        private IStorageService _storageService;
        private IDataService _dataService;
        private IProgressService _progressService;

        public MensaRespository(IStorageService storageService, IDataService dataService, IProgressService progressService)
        {
            _storageService = storageService;
            _dataService = dataService;
            _progressService = progressService;
        }

        private ObservableCollection<LocationModel> _locations;
        public async Task<ObservableCollection<LocationModel>> GetLocations()
        {
            try
            {
                var cache = await _storageService.GetCachedData();
                if (cache != null)
                {
                    try
                    {
                        _locations = JsonConvert.DeserializeObject<ObservableCollection<LocationModel>>(cache);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogException(ex);
                    }
                }
                if (_locations == null)
                    _locations = new ObservableCollection<LocationModel>();

                var config = await _storageService.GetLocationJson();
                if (config == null)
                    return _locations;

                var configModel = JsonConvert.DeserializeObject<ConfigModel>(config);
                if (configModel == null)
                    return _locations;

                bool modified = false;

                if (_locations.Count != configModel.Locations.Count)
                {
                    _locations.Clear();
                    foreach (var locationConfigModel in configModel.Locations)
                    {
                        _locations.Add(ConfigConverter.Instance.ConvertToModel(locationConfigModel));
                    }
                    modified = true;
                }
                else
                {
                    for (int i = 0; i < _locations.Count; i++)
                    {
                        if (_locations[i].Name != configModel.Locations[i].Name)
                        {
                            _locations.Insert(i, ConfigConverter.Instance.ConvertToModel(configModel.Locations[i]));
                            _locations.RemoveAt(i + 1);
                            modified = true;
                        }
                        else if (_locations[i].Mensas.Count != configModel.Locations[i].Mensas.Count)
                        {
                            _locations[i].Mensas = ConfigConverter.Instance.ConvertToModel(configModel.Locations[i].Mensas);
                            modified = true;
                        }
                        else
                        {
                            for (int j = 0; j < _locations[i].Mensas.Count; j++)
                            {
                                if (_locations[i].Mensas[j].Name != configModel.Locations[i].Mensas[j].Name)
                                {
                                    _locations[i].Mensas[j] = ConfigConverter.Instance.ConvertToModel(configModel.Locations[i].Mensas[j]);
                                    modified = true;
                                }
                            }
                        }
                    }
                }

                if (modified)
                    await Cache();

                return _locations;
            }
            catch (Exception ex)
            {
                _locations = new ObservableCollection<LocationModel>();
                LogHelper.Instance.LogException(ex);
            }

            return _locations;
        }

        public ObservableCollection<LocationModel> GetExampleLocations()
        {
            return new ObservableCollection<LocationModel>()
            {
                GetExampleLocationModel(),
                GetExampleLocationModel(),
                GetExampleLocationModel()
            };
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
                MenuAbends = new ObservableCollection<MenuModel>()
                {
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel(),
                    GetExampleMenuModel()
                },
                MenuMittags = new ObservableCollection<MenuModel>()
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
            try
            {
                foreach (var locationModel in _locations)
                {
                    foreach (var mensaModel in locationModel.Mensas)
                    {
                        var html = await _dataService.GetHtml(mensaModel.TodayUrl);
                        if (html != null)
                            HtmlParser.Instance.ParseHtml(html, mensaModel);
                    }
                }
                await Cache();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
                return false;
            }
            return true;
        }

        private async Task<bool> Cache()
        {
            try
            {
                var locationJson = JsonConvert.SerializeObject(_locations);
                return await _storageService.SetCachedData(locationJson);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }
    }
}
