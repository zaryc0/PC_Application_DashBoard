using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.Helpers;
using DashBoard.Model.interfaces;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace DashBoard.ViewModel
{
    public class ApplicationViewModel : BaseViewModel, IApplicationVM , 
        ISubscriber<ToggleApplicationTitleDisplay>
    {
        #region Local Variables
        IEventAggregator _eventAggregator;
        IApplication _application;
        bool _displayTitleOnly;
        private ImageSource _icon;
        private string _appName;
        private Brush _brush;
        #endregion

        #region Constructors
        public ApplicationViewModel(IApplication app, IEventAggregator ea) 
        {
            _eventAggregator = ea;
            _application = app;

            _displayTitleOnly = false;
            _brush = BrushConverterHelper.XMLToBrush(app.ApplicationBackgroundColour);
            
            LoadApplicationIcon(_application.ApplicationExecutablePath);
            SetApplicationName();

            ApplicationCommand = new RelayCommand(o => RunApplication());
            ApplicationEditCommand = new RelayCommand(o => ApplicationEdit());
            ApplicationDeleteCommand = new RelayCommand(o => ApplicationDelete());
            ApplicationDetailsCommand = new RelayCommand(o => ApplicationDetails());
            ApplicationOpenFolderCommand = new RelayCommand(o => ApplicationOpenFolder());
            _eventAggregator.Subscribe((ISubscriber<ToggleApplicationTitleDisplay>)this);
        }
        #endregion

        #region Access Properties
        public Guid ApplicationGuid
        {
            get => _application.ApplicationGuid; 
        }
        public string ApplicationName
        {
            get => _appName;
            private set
            {
                if (value != _appName)
                {
                    _appName = value;
                    NotifyPropertyChanged(nameof(ApplicationName));
                }
            }
        }
        public string ApplicationDescription
        {
            get => _application.ApplicationDescription;
            set
            {
                if (value != _application.ApplicationDescription)
                {
                    _application.ApplicationDescription = value;
                    NotifyPropertyChanged(nameof(ApplicationDescription));
                }
            }
        }
        public string ApplicationVersion
        {
            get => _application.ApplicationVersion;
            set
            {
                if (value != _application.ApplicationVersion)
                {
                    _application.ApplicationVersion = value;
                    NotifyPropertyChanged(nameof(ApplicationVersion));
                }
            }
        }
        public ImageSource ApplicationIcon
        {
            get => _icon;
        }
        public string ApplicationExecutablePath 
        { 
            get => _application.ApplicationExecutablePath;
            set
            {
                if (_application.ApplicationExecutablePath != value)
                {
                    _application.ApplicationExecutablePath = value;
                    LoadApplicationIcon(value);
                    NotifyPropertyChanged(nameof(ApplicationExecutablePath));
                }
            }
        }
        public string ApplicationFolderPath 
        {
            get => _application.ApplicationFolderPath;
            set
            {
                if (value != _application.ApplicationFolderPath)
                {
                    _application.ApplicationFolderPath = value;
                    NotifyPropertyChanged(nameof(ApplicationFolderPath));
                }
            }
        }
        public Brush ApplicationBackgroundColour 
        {
            get => _brush;
            set
            {
                if (_brush != value)
                {
                    _application.ApplicationBackgroundColour = BrushConverterHelper.BrushToXML(value);
                    _brush = value;
                    NotifyPropertyChanged(nameof(ApplicationBackgroundColour));
                }
            }        
        }

        public string ApplicationDateAdded
        {
            get => _application.ApplicationDateAdded;
            set
            {
                if (value != _application.ApplicationDateAdded)
                {
                    _application.ApplicationDateAdded = value;
                    NotifyPropertyChanged(nameof(ApplicationDateAdded));
                }
            }
        }

        public bool DisplayTitleOnlyFlag
        {
            get => _displayTitleOnly;
            set
            {
                if (value != _displayTitleOnly)
                {
                    _displayTitleOnly = value;
                    NotifyPropertyChanged(nameof(DisplayTitleOnlyFlag));
                }
            }
        }
        #endregion

        #region Public Functions

        #endregion

        #region Bindable Commands
        public ICommand ApplicationCommand { get; private set; }
        public ICommand ApplicationDetailsCommand { get; private set; }
        public ICommand ApplicationDeleteCommand { get; private set; }
        public ICommand ApplicationEditCommand { get; private set; }
        public ICommand ApplicationOpenFolderCommand { get; private set; }
        #endregion

        #region Local Functions
        private void LoadApplicationIcon(string exePath)
        {
            _icon = IconHelper.ExtractIconImageSource(exePath);
            NotifyPropertyChanged(nameof(ApplicationIcon));
        }

        private void SetApplicationName()
        {
            if (DisplayTitleOnlyFlag)
            {
                ApplicationName = _application.ApplicationTitle;
            }
            else
            {
                if (ApplicationName == null || ApplicationName == string.Empty)
                {
                    ApplicationName = _application.ApplicationTitle;
                }
                else
                {  
                    ApplicationName = _application.ApplicationFreindlyName; 
                }
            }
        }



        private void RunApplication()
        {
            // Handle the logic to launch the application
            string executablePath = ApplicationExecutablePath;

            if (File.Exists(executablePath))
            {
                // Flash the background color
                //FlashTileBackground(sender);

                // Play a sound (e.g., click sound)
                // Play a click sound (use the MediaPlayer or SoundPlayer class)
                DashBoard.Core.Helpers.SoundHelper.PlayEmbeddedWav("snap-274158.wav");

                // Start the process (launch the application)
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = executablePath,
                    WorkingDirectory = ApplicationFolderPath,
                    UseShellExecute = false,
                };
                Process.Start(psi);
            }
        }
        private void ApplicationOpenFolder()
        {
            Process.Start("explorer.exe",ApplicationFolderPath);
        }
        private void ApplicationEdit()
        {
            _eventAggregator.Publish(new EditDialogEvent() 
            { 
                ID = this.ApplicationGuid,
                Name = _application.ApplicationFreindlyName,
                Version = ApplicationVersion,
                BackGround = ApplicationBackgroundColour, 
                Path = ApplicationExecutablePath,
                Desc = ApplicationDescription
            });
        }

        private void ApplicationDelete()
        {
            _eventAggregator.Publish(new DeRegisterApplicationEvent() { ID = this.ApplicationGuid});
        }

        private void ApplicationDetails()
        {
            _eventAggregator.Publish(new ApplicationDetailsEvent()
            { 
                Name = ApplicationName,
                Version = ApplicationVersion,
                Date = ApplicationDateAdded,
                Path = ApplicationExecutablePath,
                Desc = ApplicationDescription,
                ID = ApplicationGuid
            });

        }
        #endregion

        #region Interface Implementations
        #region ISubscriber
        public void OnEventHandler(ToggleApplicationTitleDisplay e)
        {
            DisplayTitleOnlyFlag = e.Flag;
            SetApplicationName();
        }

        public void Update(ApplicationDialogVM dialogViewModel)
        {
            string title = Path.GetFileNameWithoutExtension(dialogViewModel.ExecutablePath);
            string folder = Path.GetDirectoryName(dialogViewModel.ExecutablePath);
            _application.ApplicationTitle = title;
            _application.ApplicationFreindlyName = dialogViewModel.ApplicationName;
            ApplicationFolderPath = folder;
            ApplicationDescription = dialogViewModel.Description;
            ApplicationVersion = dialogViewModel.VersionNumber;
            ApplicationExecutablePath = dialogViewModel.ExecutablePath;
            ApplicationBackgroundColour = new SolidColorBrush(dialogViewModel.BackgroundColor);
            LoadApplicationIcon(_application.ApplicationExecutablePath);
            SetApplicationName();
        }

        #endregion
        #endregion

    }
}
