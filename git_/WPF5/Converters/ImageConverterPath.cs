using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF5.Converters
{
    public class ImageConverterPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filename = value as string;
            if (filename == "нет")
            {
                return Environment.CurrentDirectory + @"\Images\nophoto.jpg";
            }
            return Environment.CurrentDirectory + @"\Images\" + filename;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
