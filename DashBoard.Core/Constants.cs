using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DashBoard.Core
{
    public static class  Constants
    {
        public const string CONFIG_FILE_NAME = "Dashboard_Application_Config";
        public const string CONFIG_FILE_EXTENSION = ".xml";
        public const string CONFIG_FOLDER_PATH = "\\config\\";
        public static readonly string CONFIG_FILE_RELATIVE_PATH = $"{CONFIG_FOLDER_PATH}{CONFIG_FILE_NAME}{CONFIG_FILE_EXTENSION}";

        //config Element Tags

        public const string CONFIG_APPLICATION_TAG = "Application";
        public const string CONFIG_APPLICATIONS_TAG = "Applications";
        public const string CONFIG_DASHBOARDCONFIG_TAG = "DashBoardConfig";
        public const string CONFIG_TITLE_TAG = "Title";
        public const string CONFIG_GUID_TAG = "Guid";
        public const string CONFIG_FREINDLYNAME_TAG = "FreindlyName";
        public const string CONFIG_EXE_TAG = "exe";
        public const string CONFIG_DATE_TAG = "Date";
        public const string CONFIG_VERSION_TAG = "Ver";
        public const string CONFIG_DESCRIPTION_TAG = "Description";
        public const string CONFIG_COLOR_TAG = "Color";
        public const string CONFIG_CLUSTER_TAG = "Cluster";
        public const string CONFIG_CLUSTERS_TAG = "Clusters";
        public const string CONFIG_IMAGE_PATH_TAG = "Img_source";

    }
}
