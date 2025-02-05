using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.ViewModel
{
    public class DialogVM : BaseViewModel, IDialogVM
    {
        public DialogVM(IDialogContentVM vm)
        {
           VM = vm;
            Title = vm.Title;
        }

        public string Title { get; private set; }
        public object VM { get; private set; }
    }
}
