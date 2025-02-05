using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IApplicationDetailsVM: IDialogContentVM
    {
        string Title { get; }
        string ApplicationName { get; set; }

        string ExecutablePath { get; set; }

        string Description { get; set; }

        string VersionNumber { get; set; }
          
        string Date { get; set; }
    }
}
