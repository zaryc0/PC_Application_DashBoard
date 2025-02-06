using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
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
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, ISubscriber<CloseDialogEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Guid _id;
        public DialogWindow(IEventAggregator ea, IDialogVM vm)
        {
            _eventAggregator = ea;
            InitializeComponent();
            DataContext = vm;
            
            //subscribe to events
            _eventAggregator.Subscribe((ISubscriber<CloseDialogEvent>)this);
        }

        #region ISubscriber Event Handlers
        public void OnEventHandler(CloseDialogEvent e)
        {
            
            var vm = DataContext as IDialogVM;
            var vm_content = vm.VM as IDialogContentVM;
            if (vm_content.guid == e.DialogID)
            {
                this.DialogResult = vm_content.Result;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _eventAggregator.Unsubscribe((ISubscriber<CloseDialogEvent>)this);
        }
        #endregion
    }
}
