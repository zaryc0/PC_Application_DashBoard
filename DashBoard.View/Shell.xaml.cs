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
        ISubscriber<EditDialogEvent>,
        ISubscriber<ApplicationDetailsEvent>,
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
            _eventAggregator.Subscribe((ISubscriber<EditDialogEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<ApplicationDetailsEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<CloseApplicationEvent>)this);
            Application.Current.Exit += ExitEventHandler;
        }

        public void OnEventHandler(OpenRegisterDialogEvent e)
        {    
            // Open the dialog here
            var dialog = new ApplicationDialog(_viewModelFactory.CreateNewApplicationDialogVM());
            bool? result = dialog.ShowDialog();
            if (result is not null && result == true)
            {
                if (dialog.DataContext is ApplicationDialogVM dialogViewModel)
                {
                    IShellViewModel vm = (ShellViewModel)this.DataContext;
                    vm.RegisterNewApplication(dialogViewModel);
                }
            }
        }

        public void OnEventHandler(EditDialogEvent e)
        {
            var dialog = new ApplicationDialog(_viewModelFactory.CreateNewApplicationDialogVM(name: e.Name, ver: e.Version, bg: e.BackGround, path: e.Path, desc: e.Desc));
            bool? result = dialog.ShowDialog();
            if (result is not null && result == true)
            {
                ApplicationDialogVM? dialogViewModel = dialog.DataContext as ApplicationDialogVM;

                if (dialogViewModel != null)
                {
                    IShellViewModel vm = (ShellViewModel)this.DataContext;
                    vm.EditApplication(e.ID, dialogViewModel);
                }
            }
        }

        public void OnEventHandler(ApplicationDetailsEvent e)
        {
            var vm = _viewModelFactory.CreateDetailsVM();
            vm.ApplicationName = e.Name;
            vm.VersionNumber = e.Version;
            vm.Date = e.Date;
            vm.Description = e.Desc;
            vm.ExecutablePath = e.Path;

            var dialog = new ApplicationDetailsDialog(vm);
            dialog.ShowDialog();
        }

        public void OnEventHandler(CloseApplicationEvent e)
        {
            this.Close();
        }

        private void ExitEventHandler(object sender, ExitEventArgs e)
        {
            _eventAggregator.Publish(new ApplicationExitingEvent());
        }
    }
}