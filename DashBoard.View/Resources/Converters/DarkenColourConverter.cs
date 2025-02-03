using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DashBoard.View.Resources.Converters
{
    public class DarkenColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                double factor = 0.3; // Default darken factor
                if (parameter != null)
                {
                    double.TryParse(parameter.ToString(), out factor);
                }

                Color color = brush.Color;
                byte r = (byte)(color.R * (1 - factor));
                byte g = (byte)(color.G * (1 - factor));
                byte b = (byte)(color.B * (1 - factor));

                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}