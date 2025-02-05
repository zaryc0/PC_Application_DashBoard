using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DashBoard.Model.interfaces
{
    public interface ICluster
    {
        public Guid ClusterId { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version {  get; set; }
        public string IconPath { get; set; }
        public string CreationDate { get; set; }
        public List<IApplication> Applications { get; set; } 
        public XElement BackgroundColour { get; set; }
    }
}
