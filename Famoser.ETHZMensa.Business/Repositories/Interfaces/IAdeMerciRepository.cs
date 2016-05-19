using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Repositories.Interfaces
{
    public interface IAdeMerciRepository
    {
        Task<string> GetAdeMerci();
    }
}
