using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.ViewModel.interfaces
{
    public interface IDialogVM
    {
        public string Title { get; }
        public object VM { get; }
    }
}
