using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using DashBoard.ViewModel;

namespace DashBoard.View.Resources.Selectors
{
    internal class TileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ApplicationTileTemplate { get; set; }
        public DataTemplate AddNewTileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is AdditionVM)
                return AddNewTileTemplate;
            return ApplicationTileTemplate;
        }
    }
}
