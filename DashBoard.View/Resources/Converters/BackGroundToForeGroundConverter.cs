using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DashBoard.View.Resources.Converters
{
    public class BackgroundToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SolidColorBrush solidBrush)
            {
                // Get the color from the brush
                Color backgroundColor = solidBrush.Color;

                // Calculate the brightness of the background color
                double brightness = 0.2126 * backgroundColor.R + 0.7152 * backgroundColor.G + 0.0722 * backgroundColor.B;

                // If brightness is above 128, use black text; otherwise, use white text
                if (brightness > 128)
                {
                    return (Brush)Application.Current.Resources["WindowBackGround1Brush"]; // Dark foreground for light backgrounds
                }
                else
                {
                    return (Brush)Application.Current.Resources["MaterialDesign.Brush.Primary.Foreground"]; // Light foreground for dark backgrounds
                }
            }

            // If the value is not a SolidColorBrush, return a default color (e.g., black)
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Not needed for this scenario as we don't need to convert back
            throw new NotImplementedException();
        }
    }
}
