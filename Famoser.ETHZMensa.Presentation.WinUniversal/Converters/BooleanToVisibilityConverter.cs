using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
    class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = (bool)value;
            if (str)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
