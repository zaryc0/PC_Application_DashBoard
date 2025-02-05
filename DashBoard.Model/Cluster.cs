using DashBoard.Model.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DashBoard.Model
{
    internal class Cluster : ICluster
    {
        #region Local Variables
        private Guid _id;
        private string _name;
        private string _version;
        private string _description;
        private string _iconPath;
        private string _creationDate;
        private XElement _backGroundColour;
        #endregion

        #region Constructors

        #endregion

        #region Access Properties
        public Guid ClusterId => _id;
        public string Name
        { 
            get => _name;
            set => _name = value;
        }
        public string Version 
        {
            get => _version;
            set => _version = value;
        }
        public string IconPath
        {
            get => _iconPath;
            set => _iconPath = value;
        }
        public string CreationDate
        { 
            get => _creationDate;
            set => _creationDate = value;
        }
        public List<Guid> ApplicationIds { get; set; }
        public XElement BackgroundColour 
        {
            get => _backGroundColour;
            set => _backGroundColour = value;
        }
        public string Description 
        { 
            get => _description;
            set =>_description = value;
        }


        #endregion

        #region Public Functions

        #endregion

    }


    }
