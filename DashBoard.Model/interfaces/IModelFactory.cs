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
                                        string Freindlyname,
                                        string Description,
                                        string ExecutablePath,
                                        XElement BackGroundColour,
                                        string DateAdded,
                                        string version);
    }
}
