using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Data.Services
{
    public interface IDataService
    {
        Task<string> GetHtml(Uri url);
    }
}
