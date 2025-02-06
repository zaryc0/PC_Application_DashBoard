using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.Core.EventAggregator.Events
{
    public class DisplayApplicationDetailsEvent : IEvent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Date { get; set; }
        public string Path { get; set; }
        public string Desc { get; set; }
    }
}
