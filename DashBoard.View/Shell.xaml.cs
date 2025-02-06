using DashBoard.ViewModel;
using System.Windows;
using DashBoard.ViewModel.interfaces;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.EventAggregator.Events;
using DashBoard.Model.interfaces;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DashBoard.View
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, 
        ISubscriber<DisplayApplicationRegisterEvent>, 
        ISubscriber<DisplayApplicationEditEvent>,
        ISubscriber<DisplayApplicationDetailsEvent>,
        ISubscriber<DisplayClusterRegisterEvent>,
        ISubscriber<DisplayClusterEditEvent>,
        ISubscriber<DisplayClusterDetailsEvent>,
        ISubscriber<DisplayClusterAppSelectorEvent>,
        ISubscriber<CloseSystemEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IViewModelFactory _viewModelFactory;
        public Shell(IShellViewModel vm, IViewModelFactory vmf, IEventAggregator ea)
        {
            this.DataContext = vm;
            InitializeComponent();
            _eventAggregator = ea;
            _viewModelFactory = vmf;
            _eventAggregator.Subscribe((ISubscriber<DisplayApplicationRegisterEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayApplicationEditEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayApplicationDetailsEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayClusterRegisterEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayClusterEditEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayClusterDetailsEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DisplayClusterAppSelectorEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<CloseSystemEvent>)this);
            Application.Current.Exit += ExitEventHandler;
        }

        public void OnEventHandler(DisplayApplicationRegisterEvent e)
        {    
            // Open the dialog here
            var contentVM = _viewModelFactory.CreateApplicationRegistrationVM("Register New Application");
            bool? result = LaunchDialog(contentVM);
            if (result is not null && result == true)
            {
                IShellViewModel vm = (ShellViewModel)DataContext;
                vm.RegisterNewApplication((ApplicationRegistrationVM)contentVM);
            }
        }

        public void OnEventHandler(DisplayApplicationEditEvent e)
        {
            var contentVM = _viewModelFactory.CreateApplicationRegistrationVM(name: e.Name,
                                                                                 ver: e.Version,
                                                                                 bg: e.BackGround,
                                                                                 path: e.Path,
                                                                                 desc: e.Desc);
            bool? result = LaunchDialog(contentVM);
            if (result is not null && result == true)
            {
                IShellViewModel vm = (ShellViewModel)this.DataContext;
                vm.EditApplication(e.ID, contentVM);
            }
        }

        public void OnEventHandler(DisplayApplicationDetailsEvent e)
        {
            var vm = _viewModelFactory.CreateApplicationDetailsVM();
            vm.ApplicationName = e.Name;
            vm.VersionNumber = e.Version;
            vm.Date = e.Date;
            vm.Description = e.Desc;
            vm.ExecutablePath = e.Path;
            LaunchDialog(vm);
        }

        public void OnEventHandler(CloseSystemEvent e)
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
            _eventAggregator.Publish(new ClosingEvent());
        }

        public void OnEventHandler(DisplayClusterRegisterEvent e)
        {
            // Open the dialog here
            var contentVM = _viewModelFactory.CreateClusterRegistrationVM("Register New Cluster");
            bool? result = LaunchDialog(contentVM);
            if (result is not null && result == true)
            {
                IShellViewModel vm = (ShellViewModel)DataContext;
                vm.RegisterNewCluster(contentVM);
            }
        }

        public void OnEventHandler(DisplayClusterEditEvent e)
        {
            var contentVM = _viewModelFactory.CreateClusterRegistrationVM(name: e.Name,
                                                                          version: e.Version,
                                                                          bg: e.BackGround,
                                                                          img_path: e.IconPath,
                                                                          desc: e.Desc,
                                                                          apps: e.App_ids);
            bool? result = LaunchDialog(contentVM);
            if (result == true)
            {
                IShellViewModel vm = (IShellViewModel)this.DataContext;
                vm.EditCluster(e.ID, contentVM);
            }
        }

        public void OnEventHandler(DisplayClusterDetailsEvent e)
        {
            throw new NotImplementedException();
        }
        public void OnEventHandler(DisplayClusterAppSelectorEvent e)
        {
            IShellViewModel vm = (IShellViewModel)this.DataContext;
            List<IApplicationVM> appVms = [];
            foreach (var appVM in vm.Tiles)
            {
                if(appVM != null && appVM is IApplicationVM)
                {
                    appVms.Add(appVM as IApplicationVM);
                }
            }
            var contentVM = _viewModelFactory.CreateApplicationSelectorVM(appVms);
            bool? result = LaunchDialog(contentVM);
            if (result == true)
            {
                _eventAggregator.Publish(new UpdateSelectedAppsEvent()
                {
                    app_ids = contentVM.SelectedApplications.Select(o => o.ApplicationGuid).ToList(),
                });
            }
        }
    }
}