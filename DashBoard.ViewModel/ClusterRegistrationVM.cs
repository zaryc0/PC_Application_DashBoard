using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Model.interfaces;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel
{
    public class ClusterRegistrationVM : BaseViewModel, IClusterRegistrationVM,
        ISubscriber<UpdateSelectedAppsEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IModelRegistry _modelRegistry;
        private readonly IViewModelFactory _viewModelFactory;
        private string _name;
        private string _description;
        private string _version;
        private string _img_path;
        private Color _color;
        private bool? _result;

        public ClusterRegistrationVM(string title, IEventAggregator ea, IModelRegistry mr, IViewModelFactory vmf)
        {
            Title = title;
            _name = "";
            _description = "";
            _version = "";
            _color = Colors.Red;
            _result = null;
            _img_path = "";
            _eventAggregator = ea;
            _modelRegistry = mr;
            _viewModelFactory = vmf;
            Applications = new ObservableCollection<IApplicationVM>();
            EditClusterAppsCommand = new RelayCommand(o => EditClusterApps());
        }
        public string Name 
        { 
            get => _name; 
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }
        public string Version 
        {
            get => _version;
            set
            {
                if (value != _version)
                {
                    _version = value;
                    NotifyPropertyChanged(nameof(Version));
                }
            }
        }
        public ObservableCollection<IApplicationVM> Applications { get; set; }

        public string ImagePath 
        { 
            get => _img_path;
            set
            {
                if (_img_path != value)
                {
                    _img_path = value;
                    NotifyPropertyChanged(nameof(ImagePath));
                }
            }
        }

        public Color BackgroundColor
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    NotifyPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        public string Title { get; private set; }

        public bool? Result
        {
            get => _result;
            set
            {
                if (value != _result)
                {
                    _result = value;
                    NotifyPropertyChanged(nameof(Result));
                    _eventAggregator.Publish(new CloseDialogEvent());
                }
            }
        }
        public ICommand EditClusterAppsCommand { get; private set; }

        public Guid guid { get; } = Guid.NewGuid();

        public void OnEventHandler(UpdateSelectedAppsEvent e)
        {
            Applications.Clear();
            foreach (Guid id in e.app_ids)
            {
                Applications.Add(_viewModelFactory.CreateApplicationVM(_modelRegistry.GetById(id)));
            }
        }

        private void EditClusterApps()
        {
            _eventAggregator.Publish(new DisplayClusterAppSelectorEvent());
        }
    }
}
