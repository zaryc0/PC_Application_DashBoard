using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
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
    public class ApplicationDetailsVM : BaseViewModel, IApplicationDetailsVM
    {
        #region Local Variables
        private readonly IEventAggregator _eventAggregator;
        private string _applicationName;
        private string _executablePath;
        private string _description;
        private string _date;
        private string _versionNumber;
        private bool? _result;
        #endregion

        #region Constructors
        public ApplicationDetailsVM(IEventAggregator ea)
        {
            _applicationName = "";
            _executablePath = "";
            _description = "";
            _versionNumber = "";
            _date = "";
            _result = null;

            _eventAggregator = ea;
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

        public bool? Result
        {
            get => _result;
            set
            {
                if (value != _result)
                {
                    _result = value;
                    NotifyPropertyChanged(nameof(Result));
                    _eventAggregator.Publish(new CloseDialogEvent());
                }
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
