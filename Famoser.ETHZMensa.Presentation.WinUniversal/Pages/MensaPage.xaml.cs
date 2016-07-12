using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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
