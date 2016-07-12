using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
   public class FavoriteToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (bool) value;
            if (val)
                return Symbol.SolidStar;
            return Symbol.OutlineStar;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
