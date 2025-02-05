using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.Helpers;
using DashBoard.Model.interfaces;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel
{
    public class ClusterVM: BaseViewModel, IClusterVM
    {
        #region Local Variables
        private readonly IEventAggregator _eventAggregator;
        private ICluster _cluster;
        private ImageSource _img_src;
        private Brush _brush;
        #endregion

        #region Constructors
        public ClusterVM(ICluster cluster, IEventAggregator ea)
        {
            _eventAggregator = ea;
            _cluster = cluster;
            _brush = BrushConverterHelper.XMLToBrush(cluster.BackgroundColour);
            SetIconImageSource();

            LaunchClusterCommand = new RelayCommand(o => LaunchCluster());
            ShowClusterDetailsCommand = new RelayCommand(o => ShowClusterDetails());
            EditClusterCommand = new RelayCommand(o => EditCluster());
            DeleteClusterCommand = new RelayCommand(o => DeleteCluster());
        }

        #endregion

        #region Access Properties
        public string Name
        {
            get => _cluster.Name;
            set
            {
                if (_cluster.Name != value)
                {
                    _cluster.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _cluster.Description;
            set
            {
                if (_cluster.Description != value)
                {
                    _cluster.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        public string Version
        {
            get => _cluster.Version;
            set
            {
                if (value != _cluster.Version)
                {
                    _cluster.Version = value;
                    NotifyPropertyChanged(nameof(Version));
                }
            }
        }

        public string CreationDate
        {
            get => _cluster.CreationDate;
            set
            {
                if (value != _cluster.CreationDate)
                {
                    _cluster.CreationDate = value;
                    NotifyPropertyChanged(nameof(CreationDate));
                }
            }
        }
        public ObservableCollection<IApplicationVM> Applications { get; set; }

        public ImageSource ImageSource
        {
            get => _img_src;
        }
        public Brush BackGround
        {
            get => _brush;
            set
            {
                if (_brush != value)
                {
                    _cluster.BackgroundColour = BrushConverterHelper.BrushToXML(value);
                    _brush = value;
                    NotifyPropertyChanged(nameof(BackGround));
                }
            }
        }
        #endregion

        #region Public Functions
        public void Update(string name, string description, string version, Brush bg, List<IApplicationVM> applications, string img_path = "")
        {
            Name = name;
            Description = description;
            Version = version;
            BackGround = bg;
            SyncDisplayedCollection(applications);
            _cluster.IconPath = img_path;
            SetIconImageSource();

        }
        #endregion

        #region Local Functions
        private void SetIconImageSource()
        {
            if (_cluster.IconPath == "" || _cluster.IconPath == string.Empty || _cluster.IconPath is null)
            {
                List<string> exes = [];
                foreach (IApplication app in _cluster.Applications)
                {
                    exes.Add(app.ApplicationExecutablePath);
                }
                _img_src = IconHelper.ExtractIconImageSource(exes);
            }
            else
            {
                _img_src = IconHelper.LoadImageFromFile(_cluster.IconPath);
            }
            NotifyPropertyChanged(nameof(ImageSource));
        }

        private  void SyncDisplayedCollection(List<IApplicationVM> applications)
        {
            if (Applications == null || applications == null) return;

            // Step 1: Remove items from A that are NOT in B
            for (int i = Applications.Count - 1; i >= 0; i--) // Iterate backward to avoid modifying during iteration
            {
                if (!applications.Contains(Applications[i]))
                {
                    Applications.RemoveAt(i);
                }
            }

            // Step 2: Add items to A that are in B but NOT in A
            foreach (var item in applications)
            {
                if (!Applications.Contains(item))
                {
                    Applications.Add(item);
                }
            }
        }

        private List<Guid> GetAppIds()
        {
            var appids = new List<Guid>();
            foreach (var app in Applications)
            {
                appids.Add(app.ApplicationGuid);
            }
            return appids;
        }
        #endregion

        #region Bindable Commands
        public ICommand LaunchClusterCommand { get; }
        public ICommand ShowClusterDetailsCommand { get; }
        public ICommand DeleteClusterCommand { get; }
        public ICommand EditClusterCommand { get; }
        #endregion

        #region Command Functions
        private void LaunchCluster()
        {
            foreach (var app in Applications)
            {
                app.RunApplicationCommand.Execute(null);
            }
        }

        private void ShowClusterDetails()
        {

            _eventAggregator.Publish(new DisplayClusterDetailsEvent()
            {
                ID = _cluster.ClusterId,
                Name = _cluster.Name,
                Version = _cluster.Version,
                Desc = _cluster.Description,
                IconPath = _cluster.IconPath,
                Date = _cluster.CreationDate,
                App_ids = GetAppIds()
            });
        }

        private void DeleteCluster()
        {
            _eventAggregator.Publish(new DeRegisterClusterEvent() { ID = _cluster.ClusterId });
        }

        private void EditCluster()
        {

            _eventAggregator.Publish(new DisplayClusterEditEvent()
            {
                ID = _cluster.ClusterId,
                Name = _cluster.Name,
                Version = _cluster.Version,
                Desc = _cluster.Description,
                IconPath = _cluster.IconPath,
                BackGround = _brush,
                App_ids = GetAppIds()
            });
        }
        #endregion

        #region interface implementations

        #endregion
    }
}
