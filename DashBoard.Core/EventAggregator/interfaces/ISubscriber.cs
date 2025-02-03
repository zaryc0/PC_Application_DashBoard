using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.interfaces
{
    public interface ISubscriber<TEventType>
    {
        void OnEventHandler(TEventType e);
    }
}
