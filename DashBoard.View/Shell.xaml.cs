using DashBoard.ViewModel;
using System.Windows;
using DashBoard.ViewModel.interfaces;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.EventAggregator.Events;

namespace DashBoard.View
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, 
        ISubscriber<OpenRegisterDialogEvent>, 
        ISubscriber<OpenEditDialogEvent>,
        ISubscriber<OpenDetailsDialogEvent>,
        ISubscriber<CloseApplicationEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IViewModelFactory _viewModelFactory;
        public Shell(IShellViewModel vm, IViewModelFactory vmf, IEventAggregator ea)
        {
            this.DataContext = vm;
            InitializeComponent();
            _eventAggregator = ea;
            _viewModelFactory = vmf;
            _eventAggregator.Subscribe((ISubscriber<OpenRegisterDialogEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<OpenEditDialogEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<OpenDetailsDialogEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<CloseApplicationEvent>)this);
            Application.Current.Exit += ExitEventHandler;
        }

        public void OnEventHandler(OpenRegisterDialogEvent e)
        {    
            // Open the dialog here
            var contentVM = _viewModelFactory.CreateNewApplicationRegistrationVM("Register New Application");
            bool? result = LaunchDialog(contentVM);
            if (result is not null && result == true)
            {
                IShellViewModel vm = (ShellViewModel)DataContext;
                vm.RegisterNewApplication((ApplicationRegistrationVM)contentVM);
            }
        }

        public void OnEventHandler(OpenEditDialogEvent e)
        {
            var contentVM = _viewModelFactory.CreateNewApplicationRegistrationVM(name: e.Name,
                                                                                 ver: e.Version,
                                                                                 bg: e.BackGround,
                                                                                 path: e.Path,
                                                                                 desc: e.Desc);
            bool? result = LaunchDialog(contentVM);
            if (result is not null && result == true)
            {
                IShellViewModel vm = (ShellViewModel)this.DataContext;
                vm.EditApplication(e.ID, (ApplicationRegistrationVM)contentVM);
            }
        }

        public void OnEventHandler(OpenDetailsDialogEvent e)
        {
            var vm = _viewModelFactory.CreateApplicationDetailsVM();
            vm.ApplicationName = e.Name;
            vm.VersionNumber = e.Version;
            vm.Date = e.Date;
            vm.Description = e.Desc;
            vm.ExecutablePath = e.Path;
            LaunchDialog(vm);
        }

        public void OnEventHandler(CloseApplicationEvent e)
        {
            this.Close();
        }

        private bool? LaunchDialog(IDialogContentVM VM)
        {
            var dialogVM = _viewModelFactory.CreateDialogVM(VM);
            var dialog = new DialogWindow(_eventAggregator,dialogVM);
            return  dialog.ShowDialog();
        }
        private void ExitEventHandler(object sender, ExitEventArgs e)
        {
            _eventAggregator.Publish(new ApplicationExitingEvent());
        }
    }
}