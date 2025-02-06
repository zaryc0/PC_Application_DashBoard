using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DashBoard.ViewModel.interfaces
{
    public interface IShellViewModel
    {
        public string Title { get; }
        public bool DisplayApplicationTitlesOnly { get; set; }
        public ObservableCollection<object> Tiles { get; set; }
        public ObservableCollection<object> Clusters { get; set; }
        void EditApplication(Guid ID, IApplicationRegistrationVM dialogViewModel);
        void RegisterNewApplication(IApplicationRegistrationVM dialogViewModel);
        void EditCluster(Guid ID, IClusterRegistrationVM dialogViewModel);
        void RegisterNewCluster(IClusterRegistrationVM contentVM);
        public ICommand RegisterNewClusterCommand { get; }
        public ICommand RegisterNewApplicationCommand { get; }
        public ICommand ToggleDisplayApplicationTitlesOnlyCommand { get; }
        public ICommand OpenConfigFolderCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand ChangeThemeCommand { get; }
        public ICommand ExitApplicationCommand { get; }
    }
}
