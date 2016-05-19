using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Data.Services;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.ETHZMensa.Business.Repositories
{
    public class AdeMerciRepository : IAdeMerciRepository
    {
        private const string AdeMerciUrl = "https://api.ademerci.ch/";
        private readonly IDataService _dataService;

        public AdeMerciRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<string> GetAdeMerci()
        {
            try
            {
                return await _dataService.GetHtml(new Uri(AdeMerciUrl));
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return null;

        }
    }
}
