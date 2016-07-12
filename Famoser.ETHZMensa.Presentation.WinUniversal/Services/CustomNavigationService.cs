using Windows.UI.Core;
using GalaSoft.MvvmLight.Views;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Services
{
    public class CustomNavigationService : INavigationService
    {
        private int _backStack;
        private NavigationService _realNavigationService;

        public CustomNavigationService()
        {
            _realNavigationService = new NavigationService();
            _backStack = 0;
        }

        public NavigationService Implementation { get { return _realNavigationService; } }

        public void GoBack()
        {
            _backStack--;
            ConfigureButtons();

            _realNavigationService.GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            _backStack++;
            ConfigureButtons();

            _realNavigationService.NavigateTo(pageKey);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            _backStack++;
            ConfigureButtons();

            _realNavigationService.NavigateTo(pageKey, parameter);
        }

        public string CurrentPageKey
        {
            get { return _realNavigationService.CurrentPageKey; }
        }

        private void ConfigureButtons()
        {
            if (_backStack == 0)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
        }
    }
}
