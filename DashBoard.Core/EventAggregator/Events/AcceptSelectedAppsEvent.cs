using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.Events
{
    public class AcceptSelectedAppsEvent
    {
        public List<Guid> SelectedApps { get; set; }
    }
}
