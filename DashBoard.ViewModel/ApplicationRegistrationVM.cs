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
    public class ApplicationRegistrationVM : BaseViewModel, IApplicationRegistrationVM
    {
        #region Local Variables
        private readonly IEventAggregator _eventAggregator;
        private string _applicationName;
        private string _executablePath;
        private string _description;
        private string _versionNumber;
        private Color _backgroundColor;
        private bool? _result;
        #endregion

        #region Constructors
        public ApplicationRegistrationVM(string title,IEventAggregator ea)
        {
            Title = title;
            _applicationName = "";
            _executablePath = "";
            _description = "";
            _versionNumber = "";
            _backgroundColor = Colors.Red;
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

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                    _backgroundColor = value;
                    NotifyPropertyChanged(nameof(BackgroundColor));
            }
        }

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

        public string Title { get; private set; }

        #endregion

        #region Public Functions


        #endregion

        #region Bindable Commands

        #endregion

        #region Command Functions

        #endregion




    }
}
