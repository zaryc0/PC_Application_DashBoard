using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DashBoard.Core.EventAggregator.Events
{
    public class DisplayClusterEditEvent : IEvent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Brush BackGround { get; set; }
        public string Desc { get; set; }
        public string IconPath { get; set; }
        public List<Guid> App_ids { get; set; }
    }
}
