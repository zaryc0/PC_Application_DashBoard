using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IViewModelFactory
    {
        IApplicationVM CreateApplicationVM(IApplication application);
        IApplicationVM CreateApplicationVM(string exePath, string description,string freindlyname = "" , string version = "0.1", Brush bg = null);
        IApplicationRegistrationVM CreateApplicationRegistrationVM(string title);
        IApplicationRegistrationVM CreateApplicationRegistrationVM(string name, string ver, Brush bg, string path, string desc);
        IApplicationDetailsVM CreateApplicationDetailsVM();
        IAdditionVM CreateAdditionVM(int i);
        IDialogVM CreateDialogVM(IDialogContentVM ViewModel);
        IClusterVM CreateClusterVM(ICluster cluster);
        IClusterVM CreateClusterVM(string name, string description, string version, Brush bg, List<Guid>apps, string img_path = "");
        IClusterRegistrationVM CreateClusterRegistrationVM(string title);
        IClusterRegistrationVM CreateClusterRegistrationVM(string name, string version, string img_path, Brush bg, string desc, List<Guid> apps);
        IApplicationSelectorVM CreateApplicationSelectorVM( List<IApplicationVM> applicationVMs);
    }
}
