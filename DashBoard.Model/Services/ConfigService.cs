using DashBoard.Core;
using DashBoard.Model.interfaces;
using DashBoard.Model.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DashBoard.Model.Services
{
    public class ConfigService : IConfigService
    {
        private IModelFactory _modelFactory;
        private List<IApplication> _applications;
        private List<ICluster> _clusters;
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
            if (!_loaded)
            {
                ReadConfig();
            }
            return _applications;
        }
        public List<ICluster> GetClusters()
        {
            if (!_loaded)
            {
                ReadConfig();
            }
            return _clusters;
        }

        public void ReadConfig()
        {
            var root = XElement.Load(Constants.CONFIG_FILE_RELATIVE_PATH);
            var apps = root.Element(Constants.CONFIG_APPLICATIONS_TAG);
            if (apps is not null)
            {
                foreach (var app in apps.Elements(Constants.CONFIG_APPLICATION_TAG))
                {
                    _applications.Add(_modelFactory.CreateApplication(id: new Guid(app.Attribute(Constants.CONFIG_GUID_TAG).Value),
                                                                      freindlyName: app.Attribute(Constants.CONFIG_FREINDLYNAME_TAG).Value,
                                                                      description: app.Element(Constants.CONFIG_DESCRIPTION_TAG)?.Value.Trim(),
                                                                      exePath: app.Attribute(Constants.CONFIG_EXE_TAG).Value,
                                                                      bgColor: app.Element(Constants.CONFIG_COLOR_TAG).Element("Brush"),
                                                                      dateAdded: app.Attribute(Constants.CONFIG_DATE_TAG).Value,
                                                                      version: app.Attribute(Constants.CONFIG_VERSION_TAG).Value));
                }
            }
            var clusters = root.Element(Constants.CONFIG_CLUSTERS_TAG);
            if (clusters is not null)
            {
                foreach (var cluster in clusters.Elements(Constants.CONFIG_CLUSTER_TAG))
                {
                    List<Guid> ids = [];
                    apps = cluster.Element(Constants.CONFIG_APPLICATIONS_TAG);
                    if (apps is not null)
                    {
                        foreach (var app in apps.Elements(Constants.CONFIG_APPLICATION_TAG))
                        {
                            ids.Add(new Guid(app.Attribute(Constants.CONFIG_GUID_TAG).Value));
                        }
                    }
                    _clusters.Add(_modelFactory.CreateCluster(id: new Guid(),
                                                              name: cluster.Attribute(Constants.CONFIG_TITLE_TAG).Value,
                                                              description: cluster.Element(Constants.CONFIG_DESCRIPTION_TAG).Value,
                                                              apps: ids,
                                                              imgPath: cluster.Attribute(Constants.CONFIG_IMAGE_PATH_TAG).Value,
                                                              dateAdded: cluster.Attribute(Constants.CONFIG_DATE_TAG).Value,
                                                              version: cluster.Attribute(Constants.CONFIG_VERSION_TAG).Value,
                                                              bgColor: cluster.Element(Constants.CONFIG_COLOR_TAG).Element("Brush")));
                }
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
            else if (typeof(ICluster).IsAssignableFrom(typeof(TType)))
            {
                if( !_clusters.Any(p=>p.ClusterId == ((ICluster)type).ClusterId))
                {
                    _clusters.Add(type as ICluster);
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
                else if (_clusters.Any(p => p.ClusterId == guid))
                {
                    _clusters.Remove(_clusters.First(p=>p.ClusterId == guid));
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
            var clusters = new XElement(Constants.CONFIG_CLUSTERS_TAG);
            foreach (IApplication app in _applications)
            {
                XElement application = new (Constants.CONFIG_APPLICATION_TAG,
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
            foreach (ICluster cluster in _clusters)
            {
                var cluster_apps = new XElement(Constants.CONFIG_APPLICATIONS_TAG);
                foreach (Guid id in cluster.ApplicationIds)
                {
                    cluster_apps.Add(new XElement(Constants.CONFIG_APPLICATION_TAG),new XAttribute(Constants.CONFIG_GUID_TAG, id));
                }
                XElement clusterxml = new (Constants.CONFIG_CLUSTER_TAG,
                                           new XAttribute(Constants.CONFIG_TITLE_TAG, cluster.Name),
                                           new XAttribute(Constants.CONFIG_GUID_TAG, cluster.ClusterId),
                                           new XAttribute(Constants.CONFIG_DATE_TAG, cluster.CreationDate),
                                           new XAttribute(Constants.CONFIG_VERSION_TAG, cluster.Version),
                                           new XAttribute(Constants.CONFIG_IMAGE_PATH_TAG, cluster.IconPath),
                                           new XElement(Constants.CONFIG_COLOR_TAG, cluster.BackgroundColour),
                                           cluster_apps);


            }

            //other setting configs added here

            config.Add(apps);
            config.Add(clusters);
          //config.add(others...)
          config.Save(Constants.CONFIG_FILE_RELATIVE_PATH);
        }
        private static void CreateDefaultConfig()
        {
            XElement defaultConfig = new (Constants.CONFIG_DASHBOARDCONFIG_TAG,
                                          new XElement(Constants.CONFIG_APPLICATIONS_TAG),
                                          new XElement(Constants.CONFIG_CLUSTERS_TAG)
            );

            defaultConfig.Save(Constants.CONFIG_FILE_RELATIVE_PATH);
        } 
    }
}
