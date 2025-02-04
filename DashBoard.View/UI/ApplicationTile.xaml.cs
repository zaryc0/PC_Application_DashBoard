using DashBoard.ViewModel;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DashBoard.View.UI
{
    /// <summary>
    /// Interaction logic for ApplicationTile.xaml
    /// </summary>
    public partial class ApplicationTile : UserControl
    {

        private IApplicationVM _vm;
        private DispatcherTimer _longPressTimer;
        private bool _isLongPressTriggered;
        public ApplicationTile()
        {
            InitializeComponent();
            _longPressTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _longPressTimer.Tick += OnLongPress;
        }
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isLongPressTriggered = false;
            _longPressTimer.Start();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _longPressTimer.Stop();
            if (!_isLongPressTriggered)
            {
                // Execute the launch command
                if (DataContext is ApplicationViewModel vm)
                {
                    vm.ApplicationCommand.Execute(null);
                }
            }
        }

        private void OnLongPress(object sender, EventArgs e)
        {
            _longPressTimer.Stop();
            _isLongPressTriggered = true;

            // Open the context menu manually
            if (ContextMenu != null)
            {
                ContextMenu.PlacementTarget = this;
                ContextMenu.IsOpen = true;
            }
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Ensure right-click behaves as expected
            if (ContextMenu != null)
            {
                ContextMenu.PlacementTarget = this;
                ContextMenu.IsOpen = true;
            }
        }
    }
}
