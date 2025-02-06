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
        private readonly int _e;
        public AdditionVM(IEventAggregator ea, int eventype) 
        {
            _eventAggregator = ea;
            _e = eventype;
            RegisterNewItemCommand = new RelayCommand(o => RegisterNewItem());
        }
        public ICommand RegisterNewItemCommand { get ; private set; }

        private void RegisterNewItem()
        {
            switch (_e)
            {
                case 1:
                    _eventAggregator.Publish(new DisplayApplicationRegisterEvent());
                    break;
                case 2:
                    _eventAggregator.Publish(new DisplayClusterRegisterEvent());
                    break;
            }
        }
    }
}
