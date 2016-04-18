using System;
using System.Windows.Input;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.View.Enums;
using Famoser.ETHZMensa.View.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Famoser.ETHZMensa.View.ViewModel
{
    public class MensaViewModel : ViewModelBase
    {
        private readonly IInteractionService _interactionService;
        private readonly IMensaRepository _mensaRepository;

        public MensaViewModel(IMensaRepository mensaRepository, IInteractionService interactionService)
        {
            _interactionService = interactionService;
            _mensaRepository = mensaRepository;
            _openInBrowser = new RelayCommand<Uri>(OpenInBrowser);
            _toggleFavorite = new RelayCommand(ToggleFavorite, () => CanExecuteToggleFavoriteCommand);
            _copyToClipboard = new RelayCommand<object>(CopyToClipboard, (e) => CanExecuteCopyToClipboardCommand);

            Messenger.Default.Register<MensaModel>(this, Messages.Select, EvaluateSelect);

            if (IsInDesignMode)
                Mensa = mensaRepository.GetExampleLocations()[0].Mensas[0];
        }

        private void EvaluateSelect(MensaModel obj)
        {
            Mensa = obj;
        }

        private MensaModel _mensa;
        public MensaModel Mensa
        {
            get { return _mensa; }
            set { Set(ref _mensa, value); }
        }

        private readonly RelayCommand<Uri> _openInBrowser;
        public ICommand OpenInBrowserCommand => _openInBrowser;

        private void OpenInBrowser(Uri uri)
        {
            _interactionService.OpenInBrowser(uri);
        }

        private readonly RelayCommand _toggleFavorite;
        public ICommand ToggleFavoriteCommand => _toggleFavorite;
        private bool CanExecuteToggleFavoriteCommand => !_isTogglingFavorite;

        private bool _isTogglingFavorite;
        private async void ToggleFavorite()
        {
            _isTogglingFavorite = true;
            _toggleFavorite.RaiseCanExecuteChanged();

            Mensa.IsFavorite = !Mensa.IsFavorite;
            var favs = _mensaRepository.GetFavorites();
            if (favs.Mensas.Contains(Mensa))
                favs.Mensas.Remove(Mensa);

            if (Mensa.IsFavorite)
                favs.Mensas.Insert(0, Mensa);

            await _mensaRepository.SaveState();

            _isTogglingFavorite = false;
            _toggleFavorite.RaiseCanExecuteChanged();
        }

        private readonly RelayCommand<object> _copyToClipboard;
        public ICommand CopyToClipboardCommand => _copyToClipboard;
        private bool CanExecuteCopyToClipboardCommand => true;
        private async void CopyToClipboard(object obj)
        {
            if (obj is MensaModel)
            {
                var mensa = (MensaModel) obj;
                _interactionService.CopyToClipboard(MensaToText(mensa));
            } else if (obj is MenuModel)
            {
                var menu = (MenuModel) obj;
                _interactionService.CopyToClipboard(MenuToText(menu));
            }
        }

        private string MensaToText(MensaModel mensa)
        {
            var str = "Menu of " + mensa.Name + " at " + mensa.LastTimeRefreshed.ToString("dd.mm.") + "\n\n";
            foreach (var menuModel in mensa.Menus)
            {
                str += MenuToText(menuModel) + "\n\n";
            }
            return str;
        }

        private string MenuToText(MenuModel menu)
        {
            return menu.Title + "\n" + menu.MenuName + "\n\n" + menu.Description + "\n";
        }
    }
}
