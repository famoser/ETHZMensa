using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.View.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.RefreshCommand.CanExecute(null))
                ViewModel.RefreshCommand.Execute(null);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var obj = e.ClickedItem as MensaModel;
            if (ViewModel.NavigateToCommand.CanExecute(obj))
                ViewModel.NavigateToCommand.Execute(obj);
        }

        private void UIElement_OnTapped(object sender = null, TappedRoutedEventArgs e = null)
        {
            LocationSplitView.IsPaneOpen = !LocationSplitView.IsPaneOpen;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            UIElement_OnTapped();
        }
    }
}
