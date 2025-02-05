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

namespace DashBoard.ViewModel
{
    public class ShellViewModel : BaseViewModel,  IShellViewModel , 
        ISubscriber<DeRegisterApplicationEvent>, ISubscriber<ClosingEvent>
    {

        #region Local Variables
        private IViewModelFactory _viewModelFactory;
        private IEventAggregator _eventAggregator;
        private IConfigService _configService;
        private string _title;
        private bool _showTitleOnly;
        private List<IApplicationVM> _applicationVMs;
        private IAdditionVM _additionVM;
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
            _additionVM = vm.CreateAdditionVM();

            //SEtup Commands
            RegisterNewApplicationCommand = new RelayCommand(o => OpenRegisterNewApplicationDialog());
            ToggleDisplayApplicationTitlesOnlyCommand = new RelayCommand(o => ToggleShowTitlesOnly());
            OpenConfigFolderCommand = new RelayCommand(o => OpenConfigFolder());
            ShowAboutCommand = new RelayCommand(o => ShowAboutInfo());
            ShowHelpCommand = new RelayCommand(o => ShowHelp());
            ChangeThemeCommand = new RelayCommand(o => ChangeTheme());
            ExitApplicationCommand = new RelayCommand(o => CloseDashboard());

            _eventAggregator.Subscribe((ISubscriber<DeRegisterApplicationEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<ClosingEvent>)this);

            cs.ReadConfig();
            foreach (IApplication app in cs.GetApplications())
            {
                AddApplicationVM(_viewModelFactory.CreateNewApplicationVM(app));
            }
            EnsureAdditonVMisLastTile();
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
        #endregion

        #region Public Functions
        public void RegisterNewApplication(ApplicationRegistrationVM dialogViewModel)
        {
            var app = _viewModelFactory.CreateNewApplicationVM(exePath: dialogViewModel.ExecutablePath,
                                                               description: dialogViewModel.Description,
                                                               freindlyname: dialogViewModel.ApplicationName,
                                                               version: dialogViewModel.VersionNumber,
                                                               bg: new SolidColorBrush(dialogViewModel.BackgroundColor));
            AddApplicationVM(app);
        }

        #endregion

        #region Bindable Commands
        public ICommand RegisterNewApplicationCommand { get; private set; }
        public ICommand ToggleDisplayApplicationTitlesOnlyCommand { get; private set; }
        public ICommand OpenConfigFolderCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ShowHelpCommand { get; private set; }
        public ICommand ChangeThemeCommand { get; private set; }
        public ICommand ExitApplicationCommand { get; private set; }

        #endregion

        #region Command Functions
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

        private void ShowAboutInfo() { }

        private void ChangeTheme() { }

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
            EnsureAdditonVMisLastTile();
            
        }
        private void EnsureAdditonVMisLastTile()
        {
            if(!Tiles.Contains(_additionVM))
            {
                Tiles.Add(_additionVM);
            }
        }
        private void RemoveApplicationVM(IApplicationVM app)
        {
             _applicationVMs.Remove(app);
            Tiles.Remove(app);
            EnsureAdditonVMisLastTile();
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

        public void EditApplication(Guid ID, ApplicationRegistrationVM dialogViewModel)
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
        #endregion
        #endregion

    }
}
