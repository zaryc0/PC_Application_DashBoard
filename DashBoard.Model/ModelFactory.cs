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
        public IApplication CreateApplication(Guid id, string Freindlyname, string Description, string ExecutablePath, XElement BackGroundColour, string DateAdded, string version)
        {
            string title = Path.GetFileNameWithoutExtension(ExecutablePath);
            string folder = Path.GetDirectoryName(ExecutablePath);
            IApplication app = new Appl()
            {
                ApplicationGuid = id,
                ApplicationTitle = title,
                ApplicationBackgroundColour = BackGroundColour,
                ApplicationDateAdded = DateAdded,
                ApplicationDescription = Description,
                ApplicationExecutablePath = ExecutablePath,
                ApplicationFolderPath = folder,
                ApplicationFreindlyName = Freindlyname,
                ApplicationVersion = version
            };

            return app;
        }
    }
}
