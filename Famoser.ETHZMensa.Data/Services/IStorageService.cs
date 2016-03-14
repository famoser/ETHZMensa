using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Data.Services
{
    public interface IStorageService
    {
        /// <summary>
        /// Get cached Data (saved on every Device)
        /// </summary>
        /// <returns></returns>
        Task<string> GetCachedData();

        /// <summary>
        /// Get User informations, the same for all devices of a single User
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserInformations();

        /// <summary>
        /// Get User informations, the same for all devices of a single User
        /// </summary>
        /// <returns></returns>
        Task<string> GetLocationJson();

        Task<bool> SetCachedData(string data);
        Task<bool> SetUserInformations(string info);

        Task<bool> ResetApplication();
    }
}
