using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LearnHowToMakeCustomControl.Converters
{
    public class PointMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is double x && values[1] is double y && values[2] is double thickness)
            {
                return new Point(x + thickness / 2, y + thickness / 2);
            }
            return new Point();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
