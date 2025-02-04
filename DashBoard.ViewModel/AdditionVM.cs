using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DashBoard.ViewModel
{
    public class AdditionVM : BaseViewModel, IAdditionVM
    {
        private readonly IEventAggregator _eventAggregator;
        public AdditionVM(IEventAggregator ea) 
        {
            _eventAggregator = ea;
            RegisterNewApplicationCommand = new RelayCommand(o => RegisterNewApplication());
        }
        public ICommand RegisterNewApplicationCommand { get ; private set; }

        private void RegisterNewApplication()
        {
            _eventAggregator.Publish(new OpenRegisterDialogEvent());
        }
    }
}
