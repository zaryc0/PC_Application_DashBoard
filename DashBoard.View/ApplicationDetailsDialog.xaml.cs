using DashBoard.ViewModel.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DashBoard.View
{
    /// <summary>
    /// Interaction logic for ApplicationDetailsDialog.xaml
    /// </summary>
    public partial class ApplicationDetailsDialog : Window
    {
        public ApplicationDetailsDialog(IApplicationDetailsDialogVM v)
        {
            InitializeComponent();
            this.DataContext = v;
        }
    }
}
