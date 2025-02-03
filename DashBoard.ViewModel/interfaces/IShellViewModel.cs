using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.ViewModel.interfaces
{
    public interface IShellViewModel
    {
        public string Title { get; }

        void EditApplication(Guid ID, ApplicationDialogVM dialogViewModel);
        void RegisterNewApplication(ApplicationDialogVM dialogViewModel);
    }
}
