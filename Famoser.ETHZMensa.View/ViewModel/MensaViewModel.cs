using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IInteractionService _interactionService;
        public MensaViewModel(IMensaRepository mensaRepository, IInteractionService interactionService)
        {
            _interactionService = interactionService;
            _openInBrowser = new RelayCommand<Uri>(OpenInBrowser);
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

        private RelayCommand<Uri> _openInBrowser;
        public ICommand OpenInBrowserCommand => _openInBrowser;

        private void OpenInBrowser(Uri uri)
        {
            _interactionService.OpenInBrowser(uri);
        }
    }
}
