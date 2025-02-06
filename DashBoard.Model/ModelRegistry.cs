using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Model
{
    public class ModelRegistry : IModelRegistry
    {
        // A dictionary to store applications by GUID
        private Dictionary<Guid, IApplication> Applications = [];

        // Register an application by adding it to the dictionary
        public void Register(IApplication app)
        {
            if (!Applications.ContainsKey(app.ApplicationGuid))
            {
                Applications.Add(app.ApplicationGuid, app);
            }
        }

        // Get an application by its GUID
        public IApplication GetById(Guid id)
        {
            return Applications.TryGetValue(id, out var app) ? app : null;
        }

        // Optionally, you can remove applications from the registry
        public void Remove(IApplication app)
        {
            if (Applications.ContainsKey(app.ApplicationGuid))
            {
                Applications.Remove(app.ApplicationGuid);
            }
        }
    }
}
