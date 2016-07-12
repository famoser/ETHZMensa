using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Enums;
using Famoser.ETHZMensa.Business.Helpers;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Models.ConfigModels;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Data.Services;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Newtonsoft.Json;
using IProgressService = Famoser.ETHZMensa.Business.Services.IProgressService;

namespace Famoser.ETHZMensa.Business.Repositories
{
    public class MensaRespository : IMensaRepository
    {
        private readonly IStorageService _storageService;
        private readonly IDataService _dataService;
        private readonly IProgressService _progressService;
        private const string CacheFileName = "cache.json";

        private const int MaxConcurrentTasks = 10;

        public MensaRespository(IStorageService storageService, IDataService dataService, IProgressService progressService)
        {
            _storageService = storageService;
            _dataService = dataService;
            _progressService = progressService;
        }

        private SaveModel _saveModel = new SaveModel();
        public async Task<ObservableCollection<LocationModel>> GetLocations()
        {
            try
            {
                var cache = await _storageService.GetCachedTextFileAsync(CacheFileName);
                if (cache != null)
                    _saveModel = JsonConvert.DeserializeObject<SaveModel>(cache);
                else
                    _saveModel = new SaveModel();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
                _saveModel = new SaveModel();
            }
            if (_saveModel.Locations == null)
                _saveModel.Locations = new ObservableCollection<LocationModel>();

            try
            {
                var config = await _storageService.GetAssetTextFileAsync("Assets/Configuration/Locations.json");
                if (config == null)
                    return _saveModel.Locations;

                var configModel = JsonConvert.DeserializeObject<ConfigModel>(config);
                if (configModel == null)
                    return _saveModel.Locations;

                if (_saveModel.Version != configModel.Version)
                {
                    _saveModel.Version = configModel.Version;
                    _saveModel.Locations.Clear();
                    foreach (var locationConfigModel in configModel.Locations)
                    {
                        _saveModel.Locations.Add(ConfigConverter.Instance.ConvertToModel(locationConfigModel));
                    }
                    await Cache();
                }

                return _saveModel.Locations;
            }
            catch (Exception ex)
            {
                _saveModel.Locations = new ObservableCollection<LocationModel>();
                LogHelper.Instance.LogException(ex);
            }

            return _saveModel.Locations;
        }

        private LocationModel _favorites;
        public LocationModel GetFavorites()
        {
            if (_favorites == null)
            {
                _favorites = new LocationModel()
                {
                    Name = "Favorites",
                    Mensas =
                        new ObservableCollection<MensaModel>(_saveModel.Locations.SelectMany(s => s.Mensas.Where(m => m.IsFavorite)))
                };
            }
            return _favorites;
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


        private readonly Queue<MensaModel> _refreshModels = new Queue<MensaModel>();
        public async Task<bool> Refresh()
        {
            var successful = true;
            try
            {
                _progressService.InitializeProgressBar(_saveModel.Locations.Sum(l => l.Mensas.Count));
                _refreshModels.Clear();
                var list = new List<Task>();
                foreach (var locationModel in _saveModel.Locations)
                {
                    foreach (var mensaModel in locationModel.Mensas)
                    {
                        _refreshModels.Enqueue(mensaModel);
                    }
                }

                for (int i = 0; i < MaxConcurrentTasks; i++)
                {
                    list.Add(RefreshTask());
                }

                foreach (var task in list)
                {
                    if (task.Status <= TaskStatus.Running)
                        await task;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
                successful = false;
            }
            finally
            {
                await Cache();
                _progressService.HideProgress();
            }
            return successful;
        }

        public Task<bool> SaveState()
        {
            return Cache();
        }

        private async Task RefreshTask()
        {
            while (_refreshModels.Count > 0)
            {
                var mensaModel = _refreshModels.Dequeue();

                var html = await _dataService.GetHtml(mensaModel.TodayApiUrl);
                if (html != null)
                {
                    bool res = false;
                    if (mensaModel.Type == LocationType.Eth)
                        res = HtmlParser.Instance.ParseEthHtml(html, mensaModel);
                    else if (mensaModel.Type == LocationType.Uzh)
                        res = HtmlParser.Instance.ParseUzhHtml(html, mensaModel);
                    if (res && mensaModel.Menus.Any())
                        mensaModel.LastTimeRefreshed = DateTime.Now;
                }

                _progressService.IncrementProgress();
            }
        }

        private async Task<bool> Cache()
        {
            try
            {
                var locationJson = JsonConvert.SerializeObject(_saveModel);
                return await _storageService.SetCachedTextFileAsync(CacheFileName, locationJson);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }
    }
}
