using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IClusterVM
    {
        public Guid ClusterId { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string CreationDate { get; set; }
        public ObservableCollection<IApplicationVM> Applications { get; set; }
        public ImageSource ImageSource { get; }
        public Brush BackGround { get; set; }

        void Update(string name, string description, string version, Brush bg, List<IApplicationVM> applications, string img_path = "");
        public ICommand LaunchClusterCommand { get; }
        public ICommand ShowClusterDetailsCommand { get; }
        public ICommand DeleteClusterCommand { get; }
        public ICommand EditClusterCommand { get; }
    }
}
