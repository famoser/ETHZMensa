using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.View.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace Famoser.ETHZMensa.View.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IMensaRepository _mensaRepository;
        private readonly INavigationService _navigationService;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMensaRepository mensaRepository, INavigationService navigationService)
        {
            _mensaRepository = mensaRepository;
            _navigationService = navigationService;
            _refreshCommand = new RelayCommand(Refresh, () => CanExecuteRefreshCommand);
            _navigateTo = new RelayCommand<MensaModel>(NavigateTo);

            if (IsInDesignMode)
                Locations = _mensaRepository.GetExampleLocations();
            else
                Initialize();
        }

        private bool _initialized;
        private bool _refreshRequested;
        private async void Initialize()
        {
            Locations = await _mensaRepository.GetLocations();
            Favorites = _mensaRepository.GetFavorites();
            if (Favorites.Mensas.Count > 0)
                SelectedLocation = Favorites;
            else
                SelectedLocation = _locations.FirstOrDefault();

            _initialized = true;
            if (_refreshRequested)
            {
                _refreshRequested = false;
                Refresh();
            }
        }

        private ObservableCollection<LocationModel> _locations;
        public ObservableCollection<LocationModel> Locations
        {
            get { return _locations; }
            set { Set(ref _locations, value); }
        }

        private LocationModel _selectedLocation;
        public LocationModel SelectedLocation
        {
            get { return _selectedLocation; }
            set { Set(ref _selectedLocation, value); }
        }

        private LocationModel _favorites;
        public LocationModel Favorites
        {
            get { return _favorites; }
            set { Set(ref _favorites, value); }
        }

        private readonly RelayCommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand;

        private bool CanExecuteRefreshCommand => !_isRefreshing;

        private bool _isRefreshing = false;
        private async void Refresh()
        {
            if (_initialized)
            {
                _isRefreshing = true;
                _refreshCommand.RaiseCanExecuteChanged();
                await _mensaRepository.Refresh();

                _isRefreshing = false;
                _refreshCommand.RaiseCanExecuteChanged();
            }
            else
                _refreshRequested = true;
        }

        private readonly RelayCommand<MensaModel> _navigateTo;
        public ICommand NavigateToCommand => _navigateTo;

        private void NavigateTo(MensaModel mensaModel)
        {
            _navigationService.NavigateTo(Pages.MensaPage.ToString());
            Messenger.Default.Send(mensaModel, Messages.Select);
        }
    }
}