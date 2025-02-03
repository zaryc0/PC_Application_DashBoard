using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace DashBoard.Core.Helpers
{
    public static class BrushConverterHelper
    {
        public static XElement BrushToXML(Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                return GenerateSolidColourXml(solidBrush);
            }
            else if (brush is LinearGradientBrush linearBrush)
            {
                return GenerateLinearGradientXml(linearBrush);
            }
            else if (brush is RadialGradientBrush radialBrush)
            {
                return GenerateRadialGradientXml(radialBrush);
            }
            return new XElement("NULL");
        }

        // Converts a string (e.g., hex color or gradient) back to a Brush
        public static Brush XMLToBrush(XElement brushString)
        {
            try
            {
                var brushElement = brushString;

                if (brushElement == null) return Brushes.Transparent;

                var brushType = brushElement.Attribute("Type")?.Value;

                if (brushType == "SolidColorBrush")
                {
                    var color = brushElement.Element("Color")?.Value;
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                }

                if (brushType == "LinearGradientBrush")
                {
                    var gradientBrush = new LinearGradientBrush();
                    foreach (var gradientStop in brushElement.Elements("GradientStop"))
                    {
                        var offset = double.Parse(gradientStop.Attribute("Offset")?.Value ?? "0");
                        var color = (Color)ColorConverter.ConvertFromString(gradientStop.Attribute("Color")?.Value);
                        gradientBrush.GradientStops.Add(new GradientStop(color, offset));
                    }
                    return gradientBrush;
                }

                if (brushType == "RadialGradientBrush")
                {
                    var centerX = double.Parse(brushElement.Element("Center")?.Attribute("X")?.Value ?? "0.5");
                    var centerY = double.Parse(brushElement.Element("Center")?.Attribute("Y")?.Value ?? "0.5");
                    var radiusX = double.Parse(brushElement.Element("RadiusX")?.Value ?? "1.0");
                    var radiusY = double.Parse(brushElement.Element("RadiusY")?.Value ?? "1.0");
                    var radialBrush = new RadialGradientBrush
                    {
                        Center = new System.Windows.Point(centerX, centerY),
                        RadiusX = radiusX
                    };
                    foreach (var gradientStop in brushElement.Elements("GradientStop"))
                    {
                        var offset = double.Parse(gradientStop.Attribute("Offset")?.Value ?? "0");
                        var color = (Color)ColorConverter.ConvertFromString(gradientStop.Attribute("Color")?.Value);
                        radialBrush.GradientStops.Add(new GradientStop(color, offset));
                    }
                    return radialBrush;
                }

                // Default return transparent if unable to parse
                return Brushes.Transparent;
            }
            catch
            {
                // If parsing fails, return transparent
                return Brushes.Transparent;
            }
        }
        private static XElement GenerateRadialGradientXml(RadialGradientBrush radialGradientBrush)
        {
            /* 
             * example format
             * <Brush Type = RadialGradientBrush>
             *     <Center X="0.5" Y="0.5" />
             *     <RadiusX>0.5</RadiusX>
             *     <RadiusY>0.5</RadiusY>
             *     <GradientStop Offset="0.0" Color="#FF0000" />
             *     <GradientStop Offset="1.0" Color="#0000FF" />
             * </Brush>
             */

            var brushElement = new XElement("Brush",
                new XAttribute("Type", "RadialGradientBrush"),
                new XElement("Center", new XAttribute("X", radialGradientBrush.Center.X), new XAttribute("Y", radialGradientBrush.Center.Y)),
                new XElement("RadiusX", radialGradientBrush.RadiusX),
                new XElement("RadiusY", radialGradientBrush.RadiusY));

            foreach (var stop in radialGradientBrush.GradientStops)
            {
                brushElement.Add(new XElement("GradientStop",
                    new XAttribute("Offset", stop.Offset),
                    new XAttribute("Color", stop.Color.ToString())));
            }

            return brushElement;

        }

        private static XElement GenerateSolidColourXml(SolidColorBrush solidColorBrush)
        {
            /*
             * example format
             * <Brush Type = SolidColorBrush>
             *   <Color>#FF0000</Color> <!-- Red color -->
             * </Brush> 
             */
            var brushElement = new XElement("Brush",
                    new XAttribute("Type", "SolidColorBrush"),
                    new XElement("Color", solidColorBrush.Color.ToString())
                );
            return brushElement;
        }

        private static XElement GenerateLinearGradientXml(LinearGradientBrush linearGradientBrush)
        {
            /*
             * example format
             * <Brush Type = LinearGradientBrush>
             *     <GradientStop offset="0.0" color="#FF0000" /> <!-- Red -->
             *     <GradientStop offset="1.0" color="#0000FF" /> <!-- Blue -->
             * </Brush>
             */
            var brushElement = new XElement("Brush",
                    new XAttribute("Type", "LinearGradientBrush"));

            foreach (var stop in linearGradientBrush.GradientStops)
            {
                brushElement.Add(new XElement("GradientStop",
                    new XAttribute("Offset", stop.Offset),
                    new XAttribute("Color", stop.Color.ToString())
                ));
            }

            return brushElement;
        }

    }

}
