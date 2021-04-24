using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TreeListViewSample.Converters
{
    public class FileIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;

            if (value is bool isDirectory)
            {
                visibility = isDirectory ? Visibility.Collapsed : Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
