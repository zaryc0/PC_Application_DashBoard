using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DashBoard.Model
{
    public class ModelFactory : IModelFactory
    {
        private readonly IModelRegistry _applicationRegistry;

        public ModelFactory(IModelRegistry applicationRegistry)
        {
            _applicationRegistry = applicationRegistry;
        }

        public IApplication CreateApplication(Guid id, string Freindlyname, string Description, string ExecutablePath, XElement BackGroundColour, string DateAdded, string version)
        {
            string title = Path.GetFileNameWithoutExtension(ExecutablePath);
            string folder = Path.GetDirectoryName(ExecutablePath);
            IApplication app = new Appl(id)
            {
                ApplicationTitle = title,
                ApplicationBackgroundColour = BackGroundColour,
                ApplicationDateAdded = DateAdded,
                ApplicationDescription = Description,
                ApplicationExecutablePath = ExecutablePath,
                ApplicationFolderPath = folder,
                ApplicationFreindlyName = Freindlyname,
                ApplicationVersion = version
            };
            _applicationRegistry.Register(app);
            return app;
        }

        public ICluster CreateCluster(Guid id, string name, string description, string imgPath, string dateAdded, string version, XElement bgColor, List<Guid> applications)
        {
            var cluster = new Cluster(id)
            {
                Name = name,
                Description = description,
                IconPath = imgPath,
                CreationDate = dateAdded,
                Version = version,
                BackgroundColour = bgColor,
                ApplicationIds = applications
            };
            return cluster;
        }
    }
}
