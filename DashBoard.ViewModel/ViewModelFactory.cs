using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.Helpers;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services.Interface;
using DashBoard.ViewModel.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DashBoard.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private IEventAggregator _eventAggregator;
        private IModelFactory _modelFactory;
        private IConfigService _configService;
        private IModelRegistry _modelRegistry;
        public ViewModelFactory(IModelFactory mf, IEventAggregator ea, IConfigService cs, IModelRegistry ar) 
        {
            _modelFactory = mf;
            _eventAggregator = ea;
            _configService = cs;
            _modelRegistry = ar;
        }
        public IApplicationVM CreateApplicationVM(IApplication application)
        {
            return new ApplicationViewModel(application, _eventAggregator);
        }

        public IApplicationVM CreateApplicationVM(string exePath, string description, string freindlyname = "", string version = "0.1", Brush bg = null)
        {
            Guid guid = Guid.NewGuid();
            IApplication application = _modelFactory.CreateApplication( guid,
                                                              freindlyname,
                                                              description,
                                                              exePath,
                                                              BrushConverterHelper.BrushToXML(bg ?? new SolidColorBrush(Colors.Transparent)),
                                                              $"{DateTime.Now}",
                                                              version);
            _configService.Register(application);
            return new ApplicationViewModel(application, _eventAggregator);
        }

        public IApplicationRegistrationVM CreateApplicationRegistrationVM(string title)
        {
            return new ApplicationRegistrationVM(title,_eventAggregator);
        }
        public IApplicationRegistrationVM CreateApplicationRegistrationVM(string name, string ver, Brush bg, string path, string desc)
        {
            var vm = CreateApplicationRegistrationVM($"Edit {name}");
            vm.ApplicationName = name;
            vm.VersionNumber = ver;
            vm.BackgroundColor = ((SolidColorBrush)bg).Color;
            vm.ExecutablePath = path;
            vm.Description = desc;

            return vm;
        }

        public IApplicationDetailsVM CreateApplicationDetailsVM()
        {
            return new ApplicationDetailsVM(_eventAggregator);
        }

        public IAdditionVM CreateAdditionVM(int i)
        {
            return new AdditionVM(_eventAggregator, i);
        }

        public IDialogVM CreateDialogVM(IDialogContentVM ViewModel)
        {
            return new DialogVM(ViewModel);
        }

        public IClusterVM CreateClusterVM(ICluster cluster)
        {
            var clvm = new ClusterVM(cluster, this, _eventAggregator, _modelRegistry);
            return clvm;
        }

        public IClusterVM CreateClusterVM(string name, string description, string version, Brush bg, List<Guid> apps, string img_path)
        {
            Guid guid = Guid.NewGuid();
            ICluster cluster = _modelFactory.CreateCluster( id: guid,
                                                  name: name,
                                                  description: description,
                                                  version: version,
                                                  imgPath: (img_path is null) ? "" : img_path,
                                                  bgColor: BrushConverterHelper.BrushToXML(bg),
                                                  dateAdded: $"{DateTime.Now}",
                                                  apps: apps);
            _configService.Register(cluster);
            var clVM = new ClusterVM(cluster, this, _eventAggregator, _modelRegistry);
            return clVM;
        }

        public IClusterRegistrationVM CreateClusterRegistrationVM(string title)
        {
            return new ClusterRegistrationVM(title, _eventAggregator, _modelRegistry, this);
        }

        public IClusterRegistrationVM CreateClusterRegistrationVM(string name, string version, string img_path, Brush bg, string desc, List<Guid> apps)
        {
            var vm = CreateClusterRegistrationVM($"Edit {name}");
            vm.Name = name;
            vm.Version = version;
            vm.BackgroundColor = ((SolidColorBrush)bg).Color;
            vm.ImagePath = img_path;
            vm.Description = desc;
            foreach (var app in apps)
            {
                vm.Applications.Add(CreateApplicationVM(_modelRegistry.GetById(app)));
            }
            return vm;
        }

        public IApplicationSelectorVM CreateApplicationSelectorVM( List<IApplicationVM> applicationVMs)
        {
            return new ApplicationSelectorVM(title: "Select Apps",
                                             applications: applicationVMs,
                                             eventAggregator: _eventAggregator);
        }
    }
}
