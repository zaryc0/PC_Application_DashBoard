using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DashBoard.Model.interfaces
{
    public interface IModelFactory
    {
        IApplication CreateApplication( Guid id,
                                        string freindlyName,
                                        string description,
                                        string exePath,
                                        XElement bgColor,
                                        string dateAdded,
                                        string version);
        ICluster CreateCluster(Guid id,
                                string name,
                                string description,
                                string imgPath,
                                string dateAdded,
                                string version,
                                XElement bgColor,
                                List<Guid> apps);
    }
}
