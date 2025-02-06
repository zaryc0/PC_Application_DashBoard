using DashBoard.Core;
using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services.Interface;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashBoard.ViewModel
{
    public class ShellViewModel : BaseViewModel,  IShellViewModel , 
        ISubscriber<DeRegisterApplicationEvent>, ISubscriber<ClosingEvent>,
        ISubscriber<DeRegisterClusterEvent>
    {
        #region Local Variables
        private IViewModelFactory _viewModelFactory;
        private IEventAggregator _eventAggregator;
        private IConfigService _configService;
        private string _title;
        private bool _showTitleOnly;
        private List<IApplicationVM> _applicationVMs;
        private List<IClusterVM> _clusterVMs;
        private IAdditionVM _addAppVM;
        private IAdditionVM _addClusterVM;
        #endregion

        #region Constructors
        public ShellViewModel(IViewModelFactory vm, IEventAggregator ea, IConfigService cs)
        {
            _viewModelFactory = vm;
            _eventAggregator = ea;
            _configService = cs;
            
            _showTitleOnly = false;
            _title = "Dashboard V0.1";
            _applicationVMs = [];
            _clusterVMs = [];
            _addAppVM = vm.CreateAdditionVM(Constants.APP_TYPE);
            _addClusterVM = vm.CreateAdditionVM(Constants.CLUSTER_TYPE);


            //SEtup Commands
            RegisterNewClusterCommand = new RelayCommand(o => OpenRegisterNewClusterDialog());
            RegisterNewApplicationCommand = new RelayCommand(o => OpenRegisterNewApplicationDialog());
            ToggleDisplayApplicationTitlesOnlyCommand = new RelayCommand(o => ToggleShowTitlesOnly());
            OpenConfigFolderCommand = new RelayCommand(o => OpenConfigFolder());
            ShowAboutCommand = new RelayCommand(o => ShowAboutInfo());
            ShowHelpCommand = new RelayCommand(o => ShowHelp());
            ChangeThemeCommand = new RelayCommand(o => ChangeTheme());
            ExitApplicationCommand = new RelayCommand(o => CloseDashboard());

            _eventAggregator.Subscribe((ISubscriber<DeRegisterApplicationEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<ClosingEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<DeRegisterClusterEvent>)this);

            cs.ReadConfig();
            foreach (IApplication app in cs.GetApplications())
            {
                AddApplicationVM(_viewModelFactory.CreateApplicationVM(app));
            }
            foreach (ICluster cluster in cs.GetClusters())
            {
                AddClusterVM(_viewModelFactory.CreateClusterVM(cluster));
            }
            EnsureAdditionVMisLastTile();
        }
        #endregion

        #region Access Properties
        public bool DisplayApplicationTitlesOnly
        {
            get => _showTitleOnly;
            set
            {
                if (_showTitleOnly != value)
                {
                    _showTitleOnly = value;
                    NotifyPropertyChanged(nameof(DisplayApplicationTitlesOnly));
                }
            }
        }
        public string Title => _title;
        public ObservableCollection<object> Tiles { get; set; } = new ObservableCollection<object>();
        public ObservableCollection<object> Clusters { get; set; } = new ObservableCollection<object>();
        #endregion

        #region Public Functions
        public void RegisterNewApplication(IApplicationRegistrationVM dialogViewModel)
        {
            var app = _viewModelFactory.CreateApplicationVM(exePath: dialogViewModel.ExecutablePath,
                                                               description: dialogViewModel.Description,
                                                               freindlyname: dialogViewModel.ApplicationName,
                                                               version: dialogViewModel.VersionNumber,
                                                               bg: new SolidColorBrush(dialogViewModel.BackgroundColor));
            AddApplicationVM(app);
        }
        public void EditApplication(Guid ID, IApplicationRegistrationVM dialogViewModel)
        {
            if (_applicationVMs.Any(p => p.ApplicationGuid == ID))
            {
                IApplicationVM app = _applicationVMs.First(p => p.ApplicationGuid == ID);
                app.Update
                (
                    name: dialogViewModel.ApplicationName,
                    exePath: dialogViewModel.ExecutablePath,
                    version: dialogViewModel.VersionNumber,
                    description: dialogViewModel.Description,
                    bg: new SolidColorBrush(dialogViewModel.BackgroundColor)
                );
            }
        }

        public void RegisterNewCluster(IClusterRegistrationVM contentVM)
        {
            var cluster = _viewModelFactory.CreateClusterVM(name: contentVM.Name,
                                                        description: contentVM.Description,
                                                        version: contentVM.Version,
                                                        img_path: contentVM.ImagePath,
                                                        bg: new SolidColorBrush(contentVM.BackgroundColor),
                                                        apps: contentVM.Applications.Select(p => p.ApplicationGuid).ToList());
            AddClusterVM(cluster);
        }
        public void EditCluster(Guid ID, IClusterRegistrationVM contentVM)
        { 
            if(_clusterVMs.Any(p=>p.ClusterId == ID))
            {
                IClusterVM clusterVM = _clusterVMs.First(m => m.ClusterId == ID);
                clusterVM.Update
                (
                    name: contentVM.Name,
                    description: contentVM.Description,
                    version: contentVM.Version,
                    img_path: contentVM.ImagePath,
                    bg: new SolidColorBrush(contentVM.BackgroundColor),
                    applications: contentVM.Applications.ToList()
                );
            }
        }
        #endregion

        #region Bindable Commands
        public ICommand RegisterNewClusterCommand { get; private set; }
        public ICommand RegisterNewApplicationCommand { get; private set; }
        public ICommand ToggleDisplayApplicationTitlesOnlyCommand { get; private set; }
        public ICommand OpenConfigFolderCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }

        #endregion

        #region Command Functions
        private void OpenRegisterNewClusterDialog()
        {
            _eventAggregator.Publish(new DisplayClusterRegisterEvent());
        }
        private void OpenRegisterNewApplicationDialog()
        {
            _eventAggregator.Publish(new DisplayApplicationRegisterEvent());
        }

        private void ToggleShowTitlesOnly()
        {
            DisplayApplicationTitlesOnly = !DisplayApplicationTitlesOnly;
            _eventAggregator.Publish(new UpdateTitleOnlyFlagEvent(){ Flag = DisplayApplicationTitlesOnly });
        }

        private void OpenConfigFolder()
        {
            Process.Start("explorer.exe", Constants.CONFIG_FOLDER_PATH);
        }

        private void ShowHelp()
        {

        }

        private void ShowAboutInfo() 
        {

        }

        private void ChangeTheme() 
        {

        }

        private void CloseDashboard()
        {
            _eventAggregator.Publish(new CloseSystemEvent());
        }
        #endregion

        #region Local Functions
        private void AddApplicationVM(IApplicationVM app)
        {
            _applicationVMs.Add(app);
            if (Tiles.Count > 0)
            {
                Tiles.Insert(Tiles.Count - 1, app);
            }
            else
            {
                Tiles.Add(app);
            }
            EnsureAdditionVMisLastTile();
            
        }
        private void EnsureAdditionVMisLastTile()
        {
            if(!Tiles.Contains(_addAppVM))
            {
                Tiles.Add(_addAppVM);
            }
            if(!Clusters.Contains(_addClusterVM))
            {
                Clusters.Add(_addClusterVM);
            }
        }
        private void RemoveApplicationVM(IApplicationVM app)
        {
             _applicationVMs.Remove(app);
            Tiles.Remove(app);
            EnsureAdditionVMisLastTile();
        }
        private void AddClusterVM(IClusterVM clusterVM)
        {
            _clusterVMs.Add(clusterVM);
            if (Clusters.Count > 0)
            {
                Clusters.Insert(Clusters.Count - 1, clusterVM);
            }
            else
            {
                Clusters.Add(clusterVM);
            }
            EnsureAdditionVMisLastTile();
        }

        private void RemoveClusterVM(IClusterVM cluster2rm)
        {
            _clusterVMs.Remove(cluster2rm);
            Clusters.Remove(cluster2rm);
            EnsureAdditionVMisLastTile();
        }
        #endregion

        #region Interface Implementations
        #region ISubscriber
        public void OnEventHandler(DeRegisterApplicationEvent e)
        {
            var app2rm = _applicationVMs.First(p => p.ApplicationGuid == e.ID);
            _configService.Unregister(e.ID);
            RemoveApplicationVM(app2rm);
        }

        public void OnEventHandler(ClosingEvent e)
        {
            _configService.WriteConfig();
        }

        public void OnEventHandler(DeRegisterClusterEvent e)
        {
            IClusterVM cluster2rm = _clusterVMs.First(p => p.ClusterId == e.ID);
            _configService.Unregister(e.ID);
            RemoveClusterVM(cluster2rm);
        }

        #endregion
        #endregion

    }
}
