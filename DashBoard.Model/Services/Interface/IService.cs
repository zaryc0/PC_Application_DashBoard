using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DashBoard.Model.Services.Interface
{
    public interface IService
    {
        int Register<TType>(TType type);
        int Unregister<TType>(TType type);
    }
}
