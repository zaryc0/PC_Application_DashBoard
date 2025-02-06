using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IClusterRegistrationVM : IDialogContentVM
    {
        string Name { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        ObservableCollection<IApplicationVM> Applications { get; set; }
        string ImagePath { get; set; }
        Color BackgroundColor { get; set; }
        ICommand EditClusterAppsCommand { get; }
    }
}
