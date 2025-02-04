using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IViewModelFactory
    {
        IApplicationVM CreateNewApplicationVM(IApplication application);
        IApplicationVM CreateNewApplicationVM(string exePath, string description,string freindlyname = "" , string version = "0.1", Brush bg = null);
        IApplicationDialogVM CreateNewApplicationDialogVM();
        IApplicationDialogVM CreateNewApplicationDialogVM(string name, string ver, Brush bg, string path, string desc);
        IApplicationDetailsDialogVM CreateDetailsVM();
        IAdditionVM CreateAdditionVM();
    }
}
