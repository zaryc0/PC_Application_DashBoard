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
        ISubscriber<DeRegisterApplicationEvent>, ISubscriber<ApplicationExitingEvent>
    {

        #region Local Variables
        private IViewModelFactory _viewModelFactory;
        private IEventAggregator _eventAggregator;
        private IConfigService _configService;
        private string _title;
        private bool _showTitleOnly;
        #endregion

        #region Constructors
        public ShellViewModel(IViewModelFactory vm, IEventAggregator ea, IConfigService cs)
        {
            _viewModelFactory = vm;
            _eventAggregator = ea;
            _configService = cs;

            _showTitleOnly = false;
            _title = "Dashboard V0.1";

            RegisterNewApplicationCommand = new RelayCommand(o => OpenRegisterNewApplicationDialog());
            ToggleDisplayApplicationTitlesOnlyCommand = new RelayCommand(o => ToggleShowTitlesOnly());
            OpenConfigFolderCommand = new RelayCommand(o => OpenConfigFolder());
            ShowAboutCommand = new RelayCommand(o => ShowAboutInfo());
            ShowHelpCommand = new RelayCommand(o => ShowHelp());
            ChangeThemeCommand = new RelayCommand(o => ChangeTheme());
            ExitApplicationCommand = new RelayCommand(o => CloseDashboard());


        _eventAggregator.Subscribe((ISubscriber<DeRegisterApplicationEvent>)this);
            _eventAggregator.Subscribe((ISubscriber<ApplicationExitingEvent>)this);

            cs.ReadConfig();
            foreach (IApplication app in cs.GetApplications())
            {
                ApplicationVMs.Add(_viewModelFactory.CreateNewApplicationVM(app));
            }
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
        public ObservableCollection<IApplicationVM> ApplicationVMs { get; set; } = new ObservableCollection<IApplicationVM>();
        #endregion

        #region Public Functions
        public void RegisterNewApplication(ApplicationDialogVM dialogViewModel)
        {
            var app = _viewModelFactory.CreateNewApplicationVM(exePath: dialogViewModel.ExecutablePath,
                                                               description: dialogViewModel.Description,
                                                               freindlyname: dialogViewModel.ApplicationName,
                                                               version: dialogViewModel.VersionNumber,
                                                               bg: new SolidColorBrush(dialogViewModel.BackgroundColor));
            ApplicationVMs.Add(app);
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
            _eventAggregator.Publish(new OpenRegisterDialogEvent());
        }

        private void ToggleShowTitlesOnly()
        {
            DisplayApplicationTitlesOnly = !DisplayApplicationTitlesOnly;
            _eventAggregator.Publish(new ToggleApplicationTitleDisplay(){ Flag = DisplayApplicationTitlesOnly });
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
            _eventAggregator.Publish(new CloseApplicationEvent());
        }
        #endregion

        #region Interface Implementations
        #region ISubscriber
        public void OnEventHandler(DeRegisterApplicationEvent e)
        {
            var app2rm = ApplicationVMs.First(p => p.ApplicationGuid == e.ID);
            _configService.Unregister(e.ID);
            ApplicationVMs.Remove(app2rm);
        }

        public void OnEventHandler(ApplicationExitingEvent e)
        {
            _configService.WriteConfig();
        }

        public void EditApplication(Guid ID, ApplicationDialogVM dialogViewModel)
        {
            if (ApplicationVMs.Any(p => p.ApplicationGuid == ID))
            {
                IApplicationVM app = ApplicationVMs.First(p => p.ApplicationGuid == ID);
                app.Update(dialogViewModel);
            }

        }
        #endregion
        #endregion

    }
}
