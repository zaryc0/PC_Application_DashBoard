using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel.interfaces
{
    public interface IApplicationVM
    {
        public Guid ApplicationGuid { get;  }
        public string ApplicationName { get; }
        public string ApplicationDescription { get; set; }
        public string ApplicationVersion { get; set; }
        public ImageSource ApplicationIcon { get; }
        public string ApplicationExecutablePath { get; set; }
        public string ApplicationFolderPath { get; set; }
        public Brush ApplicationBackgroundColour { get; set; }
        public string ApplicationDateAdded { get; set; }
        public bool DisplayTitleOnlyFlag { get; set; }

        void Update(string exePath, string name, string description, string version, Brush bg);
        IApplication RetrieveModel();

        public ICommand RunApplicationCommand { get; }
        public ICommand ShowApplicationDetailsCommand { get; }
        public ICommand DeleteApplicationCommand { get; }
        public ICommand EditApplicationCommand { get; }
        public ICommand OpenApplicationFolderCommand { get; }

    }
}
