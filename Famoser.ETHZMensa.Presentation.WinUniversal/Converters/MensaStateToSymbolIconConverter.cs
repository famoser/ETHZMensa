using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Famoser.ETHZMensa.Business.Models;

namespace Famoser.ETHZMensa.Presentation.WinUniversal.Converters
{
    public class MensaStateToSymbolIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Symbol.Accept : Symbol.Clear;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
