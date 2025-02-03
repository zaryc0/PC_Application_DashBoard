using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DashBoard.ViewModel
{
    public class ApplicationDetailsDialogVM : BaseViewModel, IApplicationDetailsDialogVM
    {
        #region Local Variables
        private string _applicationName;
        private string _executablePath;
        private string _description;
        private string _date;
        private string _versionNumber;
        #endregion

        #region Constructors
        public ApplicationDetailsDialogVM()
        {
            _applicationName = "";
            _executablePath = "";
            _description = "";
            _versionNumber = "";
            _date = "";
        }
        #endregion

        #region Access Properties
        public string ApplicationName
        {
            get => _applicationName;
            set
            {
                _applicationName = value;
                NotifyPropertyChanged(nameof(ApplicationName));
            }
        }

        public string ExecutablePath
        {
            get => _executablePath;
            set
            {
                _executablePath = value;
                NotifyPropertyChanged(nameof(ExecutablePath));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged(nameof(Description));
            }
        }

        public string VersionNumber
        {
            get => _versionNumber;
            set
            {
                _versionNumber = value;
                NotifyPropertyChanged(nameof(VersionNumber));
            }
        }

        public string Date 
        {
            get => _date;
            set
            {
                _date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        public string Title => ApplicationName + " Details";

        #endregion

        #region Public Functions


        #endregion

        #region Bindable Commands

        #endregion

        #region Command Functions

        #endregion




    }
}
