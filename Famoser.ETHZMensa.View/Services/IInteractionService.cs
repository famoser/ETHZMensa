using System;

namespace Famoser.ETHZMensa.View.Services
{
    public interface IInteractionService
    {
        void OpenInBrowser(Uri uri);
        void CheckBeginInvokeOnUi(Action action);
        void CopyToClipboard(string richText);
    }
}
