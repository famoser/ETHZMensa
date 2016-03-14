using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Services.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public string CurrentPageKey { get; }
    }
}
