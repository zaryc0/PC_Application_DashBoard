using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Core.Helpers;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services.Interface;
using DashBoard.ViewModel.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        private IEventAggregator _ea;
        private IModelFactory _mf;
        private IConfigService _cs;
        public ViewModelFactory(IModelFactory mf, IEventAggregator ea, IConfigService cs) 
        {
            _mf = mf;
            _ea = ea;
            _cs = cs;
        }
        public IApplicationVM CreateNewApplicationVM(IApplication application)
        {
            return new ApplicationViewModel(application, _ea);
        }

        public IApplicationVM CreateNewApplicationVM(string exePath, string description, string freindlyname = "", string version = "0.1", Brush bg = null)
        {
            Guid guid = Guid.NewGuid();
            IApplication application = _mf.CreateApplication( guid,
                                                              freindlyname,
                                                              description,
                                                              exePath,
                                                              BrushConverterHelper.BrushToXML(bg ?? new SolidColorBrush(Colors.Transparent)),
                                                              $"{DateTime.Now}",
                                                              version);
            _cs.Register(application);
            return new ApplicationViewModel(application, _ea);
        }

        public IApplicationRegistrationVM CreateNewApplicationRegistrationVM(string title)
        {
            return new ApplicationRegistrationVM(title,_ea);
        }
        public IApplicationRegistrationVM CreateNewApplicationRegistrationVM(string name, string ver, Brush bg, string path, string desc)
        {
            var vm = CreateNewApplicationRegistrationVM($"Edit {name}");
            vm.ApplicationName = name;
            vm.VersionNumber = ver;
            vm.BackgroundColor = ((SolidColorBrush)bg).Color;
            vm.ExecutablePath = path;
            vm.Description = desc;

            return vm;
        }

        public IApplicationDetailsVM CreateApplicationDetailsVM()
        {
            return new ApplicationDetailsVM(_ea);
        }

        public IAdditionVM CreateAdditionVM()
        {
            return new AdditionVM(_ea);
        }

        public IDialogVM CreateDialogVM(IDialogContentVM ViewModel)
        {
            return new DialogVM(ViewModel);
        }
    }
}
