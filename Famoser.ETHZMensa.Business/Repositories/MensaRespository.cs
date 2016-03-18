using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Enums;
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

        private SaveModel _saveModel = new SaveModel();
        public async Task<ObservableCollection<LocationModel>> GetLocations()
        {
            try
            {
                var cache = await _storageService.GetCachedData();
                if (cache != null)
                {
                    try
                    {
                        _saveModel = JsonConvert.DeserializeObject<SaveModel>(cache);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogException(ex);
                        _saveModel = new SaveModel();
                    }
                }
                if (_saveModel.Locations == null)
                    _saveModel.Locations = new ObservableCollection<LocationModel>();

                var config = await _storageService.GetLocationJson();
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


        public async Task<bool> Refresh()
        {
            try
            {
                var list = new List<Task>();
                _progressService.InitializeProgressBar(_saveModel.Locations.Sum(l => l.Mensas.Count));
                foreach (var locationModel in _saveModel.Locations)
                {
                    list.Add(RefreshTask(locationModel.Mensas));
                }

                foreach (var task in list)
                {
                    if (task.Status <= TaskStatus.Running)
                        await task;
                }

                await Cache();
                _progressService.HideProgress();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
                return false;
            }
            return true;
        }

        public Task<bool> SaveState()
        {
            return Cache();
        }

        private async Task RefreshTask(ObservableCollection<MensaModel> mensas)
        {
            foreach (var mensaModel in mensas)
            {
                if (mensaModel.Type == LocationType.Uzh)
                    mensaModel.TodayUrl = new Uri(mensaModel.LogicUrl.AbsoluteUri.Replace("[DAY_SHORT]", GetTodayShortDay()));
                else
                {
                    mensaModel.TodayUrl = mensaModel.LogicUrl;
                }

                var html = await _dataService.GetHtml(mensaModel.TodayUrl);
                if (html != null)
                {
                    bool res = false;
                    if (mensaModel.Type == LocationType.Eth)
                        res = HtmlParser.Instance.ParseEthHtml(html, mensaModel);
                    else if (mensaModel.Type == LocationType.EthAbendessen)
                        res = HtmlParser.Instance.ParseEthAbendessenHtml(html, mensaModel);
                    else if (mensaModel.Type == LocationType.Uzh)
                        res = HtmlParser.Instance.ParseUzhHtml(html, mensaModel);
                    if (res)
                        mensaModel.LastTimeRefreshed = DateTime.Now;
                }

                _progressService.IncrementProgress();
            }
        }

        private string GetTodayShortDay()
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                return "mo";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday)
                return "di";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
                return "mi";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday)
                return "do";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                return "fre";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                return "sa";
            return "so";
        }

        private async Task<bool> Cache()
        {
            try
            {
                var locationJson = JsonConvert.SerializeObject(_saveModel);
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
