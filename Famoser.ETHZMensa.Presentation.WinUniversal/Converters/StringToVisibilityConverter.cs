using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
    class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            if (!string.IsNullOrEmpty(str))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
