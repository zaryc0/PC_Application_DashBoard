using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Model.interfaces
{
    public interface IModelRegistry
    {
        public void Register(IApplication app);

        public IApplication GetById(Guid id);

        public void Remove(IApplication app);
    }
}
