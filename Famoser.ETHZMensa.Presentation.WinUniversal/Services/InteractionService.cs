using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Famoser.ETHZMensa.View.Services;
using GalaSoft.MvvmLight.Threading;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Services
{
    public class InteractionService : IInteractionService
    {
        public void OpenInBrowser(Uri uri)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Windows.System.Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void CheckBeginInvokeOnUi(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }

        public void CopyToClipboard(string richText)
        {
            DataPackage myPackage = new DataPackage();
            myPackage.SetText(richText);
            Clipboard.SetContent(myPackage);
        }
    }
}
