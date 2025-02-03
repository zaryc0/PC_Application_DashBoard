using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace DashBoard.Model
{
    internal class Appl : IApplication
    {
        #region Local Variables
        private Guid _appGuid;
        private string _appTitle;
        private string _appDescription;
        private string _appName;
        private string _appVersion;
        private string _appFolderPath;
        private string _appExePath;
        private XElement _appBgColour;
        private string _appDateAdded;
        #endregion

        #region Constructors
        public Appl()
        {
            _appGuid = Guid.NewGuid();
            _appTitle = string.Empty;
            _appDescription = string.Empty;
            _appName = string.Empty;
            _appVersion = string.Empty;
            _appFolderPath = string.Empty;
            _appExePath = string.Empty;
            _appBgColour = new XElement("NULL");
            _appDateAdded = string.Empty;
        }
        #endregion

        #region Access Properties
        public Guid ApplicationGuid
        {
            get => _appGuid;
            set => _appGuid = value;
        }
        public string ApplicationTitle
        {
            get => _appTitle;
            set => _appTitle = value;
        }
        public string ApplicationDescription
        {
            get => _appDescription;
            set => _appDescription = value;
        }
        public string ApplicationVersion
        {
            get => _appVersion;
            set => _appVersion = value;
        }
        public string ApplicationFreindlyName
        {
            get => _appName;
            set => _appName = value;
        }
        public string ApplicationFolderPath
        {
            get => _appFolderPath;
            set => _appFolderPath = value;
        }
        public string ApplicationExecutablePath
        {
            get => _appExePath;
            set => _appExePath = value;
        }
        public XElement ApplicationBackgroundColour
        {
            get => _appBgColour;
            set => _appBgColour = value;
        }
        public string ApplicationDateAdded 
        { 
            get =>_appDateAdded; 
            set =>_appDateAdded = value; 
        }
        #endregion

        #region Public Functions
        #endregion
    }
}
