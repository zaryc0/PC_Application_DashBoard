using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.Events
{
    public class DeRegisterApplicationEvent : IEvent
    {
        public Guid ID { get; set; }
    }
}
