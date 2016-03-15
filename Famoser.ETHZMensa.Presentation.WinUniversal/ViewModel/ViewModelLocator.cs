/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Famoser.ETHZMensa.View"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Famoser.ETHZMensa.Data.Services;
using Famoser.ETHZMensa.Presentation.WinUniversal.Services;
using Famoser.ETHZMensa.Presentation.WinUniversal.Services.Mocks;
using Famoser.ETHZMensa.View.Enums;
using Famoser.ETHZMensa.View.Services;
using Famoser.ETHZMensa.View.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator : BaseViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IInteractionService, InteractionService>();


            if (!ViewModelBase.IsInDesignModeStatic)
            {
                var service = new CustomNavigationService();
                service.Implementation.Configure(View.Enums.Pages.MainPage.ToString(), typeof (Pages.MainPage));
                service.Implementation.Configure(View.Enums.Pages.MensaPage.ToString(), typeof (Pages.MensaPage));
                SimpleIoc.Default.Register<INavigationService>(() => service);
            }
            else
            {
                SimpleIoc.Default.Register<INavigationService, MockNavigationService>();
            }
        }
    }
}