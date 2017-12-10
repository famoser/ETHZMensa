using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
    class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visible = Visibility.Visible;
            Visibility collapsed = Visibility.Collapsed;
            if (parameter is string && (string)parameter == "invert")
            {
                visible = collapsed;
                collapsed = Visibility.Visible;
            }
            var val = (int)value;
            return val > 0 ? visible : collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
