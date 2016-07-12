using System;
using Windows.UI.Xaml.Data;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
    class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dat = (DateTime) value;
            if (dat == DateTime.MinValue)
                return "never";
            return dat.ToString("ddd dd.MM.yy HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
