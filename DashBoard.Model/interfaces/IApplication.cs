using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace DashBoard.Model.interfaces
{
    public interface IApplication
    {
        public Guid ApplicationGuid {  get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationVersion { get; set; }
        public string ApplicationFreindlyName { get; set; }
        public string ApplicationFolderPath { get; set; }
        public string ApplicationExecutablePath { get; set; }
        public XElement ApplicationBackgroundColour { get; set; }
        public string ApplicationDateAdded { get; set; }
    }
}
