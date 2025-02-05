using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.Events
{
    public class DisplayClusterDetailsEvent : IEvent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Date { get; set; }
        public string IconPath { get; set; }
        public string Desc { get; set; }
        public List<Guid> App_ids { get; set; }
    }
}
