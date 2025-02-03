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
    public class ApplicationDialogVM : BaseViewModel, IApplicationDialogVM
    {
        #region Local Variables
        private string _applicationName;
        private string _executablePath;
        private string _description;
        private string _versionNumber;
        private Color _backgroundColor;
        #endregion

        #region Constructors
        public ApplicationDialogVM()
        {
            _applicationName = "";
            _executablePath = "";
            _description = "";
            _versionNumber = "";
            _backgroundColor = Colors.Red;
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

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                    _backgroundColor = value;
                    NotifyPropertyChanged(nameof(BackgroundColor));
            }
        }

        #endregion

        #region Public Functions


        #endregion

        #region Bindable Commands

        #endregion

        #region Command Functions

        #endregion




    }
}
