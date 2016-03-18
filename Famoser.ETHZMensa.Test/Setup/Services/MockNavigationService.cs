﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;

namespace Famoser.ETHZMensa.Test.Setup.Services
{
    public class MockNavigationService : INavigationService
    {
        public void GoBack()
        {
            
        }

        public void NavigateTo(string pageKey)
        {

        }

        public void NavigateTo(string pageKey, object parameter)
        {

        }

        public string CurrentPageKey { get; }
    }
}
