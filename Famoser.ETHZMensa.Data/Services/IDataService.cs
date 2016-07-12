using System;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Data.Services
{
    public interface IDataService
    {
        Task<string> GetHtml(Uri url);
    }
}
