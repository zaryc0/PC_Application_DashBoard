using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Model.Services.Interface
{
    public interface IConfigService : IService
    {
        void WriteConfig();

        void ReadConfig();
        List<IApplication> GetApplications();
    }
}
