using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.View.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MensaPage : Page
    {
        public MensaPage()
        {
            this.InitializeComponent();
        }

        private MensaViewModel ViewModel => DataContext as MensaViewModel;

        private void Mensa_Holding(object sender, HoldingRoutedEventArgs e)
        {
            ShowFlyout(sender as Grid);
        }

        private void UIElement_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ShowFlyout(sender as Grid);
        }

        private void ShowFlyout(Grid grid)
        {
            if (grid != null)
            {
                FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(grid);
                flyoutBase.ShowAt(grid);
            }
        }
    }
}
