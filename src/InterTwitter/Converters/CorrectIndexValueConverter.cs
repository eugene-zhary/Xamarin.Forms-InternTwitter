using System;
using System.Globalization;
using Xamarin.Forms;

namespace InterTwitter.Converters
{
    public class CorrectIndexValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (int)value + 1;

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
