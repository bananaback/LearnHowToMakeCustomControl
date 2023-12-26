using System;
using System.Globalization;
using System.Windows.Data;

namespace LearnHowToMakeCustomControl.Converters
{
    public class RadiusToSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double radius && values[1] is double arcThickness)
            {
                return radius * 2 + arcThickness;
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
