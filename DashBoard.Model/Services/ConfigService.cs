using DashBoard.Core;
using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace DashBoard.Model.Services
{
    public class ConfigService : IConfigService
    {
        private IModelFactory _modelFactory;
        private IEventAggregator _eventAggregator;
        private List<IApplication> _applications;
        private bool _loaded;

        public ConfigService(IModelFactory mf)
        {
            _modelFactory = mf;
            _loaded = false;
            _applications = [];
            string directoryPath = Path.GetDirectoryName(Constants.CONFIG_FILE_RELATIVE_PATH);

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Ensure the file exists
            if (!File.Exists(Constants.CONFIG_FILE_RELATIVE_PATH))
            {
                CreateDefaultConfig();
            }
        }

        public List<IApplication> GetApplications()
        {
            if (_loaded) return _applications;
            else return [];
        }

        public void ReadConfig()
        {
            var root = XElement.Load(Constants.CONFIG_FILE_RELATIVE_PATH);
            var apps = root.Element(Constants.CONFIG_APPLICATIONS_TAG);
            foreach (var app in apps.Elements(Constants.CONFIG_APPLICATION_TAG))
            {
                _applications.Add(_modelFactory.CreateApplication(id               : new Guid(app.Attribute(Constants.CONFIG_GUID_TAG).Value),
                                                                  Freindlyname     : app.Attribute(Constants.CONFIG_FREINDLYNAME_TAG).Value,
                                                                  Description      : app.Element(Constants.CONFIG_DESCRIPTION_TAG)?.Value.Trim(),
                                                                  ExecutablePath   : app.Attribute(Constants.CONFIG_EXE_TAG).Value,
                                                                  BackGroundColour : app.Element(Constants.CONFIG_COLOR_TAG).Element("Brush"),
                                                                  DateAdded        : app.Attribute(Constants.CONFIG_DATE_TAG).Value,
                                                                  version          : app.Attribute(Constants.CONFIG_VERSION_TAG).Value));
            }
            _loaded = true;
        }

        public int Register<TType>(TType type)
        {
            if (type is null) return -1;
            else if (typeof(IApplication).IsAssignableFrom(typeof(TType)))
            {
                if (!_applications.Any(p=>p.ApplicationGuid == ((IApplication)type).ApplicationGuid))
                { 
                    _applications.Add(type as IApplication); 
                }
            }
            return 0;
        }

        public int Unregister<TType>(TType type)
        {
            if (type is null) return -1;
            else if (type is Guid guid)
            {
                if (_applications.Any(p => p.ApplicationGuid == guid))
                {
                    _applications.Remove(_applications.First(p => p.ApplicationGuid == guid));
                }
                else
                {
                    return -2;
                }
            }
            return 0;
        }

        public void WriteConfig()
        {
            var config = new XElement(Constants.CONFIG_DASHBOARDCONFIG_TAG);
            var apps = new XElement(Constants.CONFIG_APPLICATIONS_TAG);
            foreach (IApplication app in _applications)
            {
                XElement application = new XElement(Constants.CONFIG_APPLICATION_TAG,
                                           new XAttribute(Constants.CONFIG_TITLE_TAG, app.ApplicationTitle),
                                           new XAttribute(Constants.CONFIG_GUID_TAG, app.ApplicationGuid),
                                           new XAttribute(Constants.CONFIG_FREINDLYNAME_TAG, app.ApplicationFreindlyName),
                                           new XAttribute(Constants.CONFIG_EXE_TAG, app.ApplicationExecutablePath),
                                           new XAttribute(Constants.CONFIG_DATE_TAG, app.ApplicationDateAdded),
                                           new XAttribute(Constants.CONFIG_VERSION_TAG, app.ApplicationVersion),
                                           new XElement(Constants.CONFIG_DESCRIPTION_TAG, app.ApplicationDescription),
                                           new XElement(Constants.CONFIG_COLOR_TAG, app.ApplicationBackgroundColour));

                apps.Add(application);
            }

            //other setting configs added here

            config.Add(apps);
          //config.add(others...)
          config.Save(Constants.CONFIG_FILE_RELATIVE_PATH);
        }

        private static void CreateDefaultConfig()
        {
            var defaultConfig = new XElement("DashBoardConfig",
                new XElement("Applications")
            );

            defaultConfig.Save(Constants.CONFIG_FILE_RELATIVE_PATH);
        } 
    }
}
