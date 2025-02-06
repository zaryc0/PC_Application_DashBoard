using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.ViewModel;
using DashBoard.ViewModel.interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DashBoard.View.UI
{
    /// <summary>
    /// Interaction logic for ClusterRegistration.xaml
    /// </summary>
    public partial class ClusterRegistration : UserControl
    {
        public ClusterRegistration() 
        {
            InitializeComponent();
        }
        public ClusterRegistration(IEventAggregator ea)
        {
            InitializeComponent();
        }
        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            // Handle save logic
            var viewModel = (IClusterRegistrationVM)DataContext;
            viewModel.Result = true;  // Indicate that the user saved the data
        }
        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff;*.ico",
                Title = "Select Icon"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the ExecutablePath in the ViewModel to the selected file path
                var viewModel = (IClusterRegistrationVM)DataContext;
                viewModel.ImagePath = openFileDialog.FileName;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the entered character is a valid digit or decimal point
            if (e.Text == "." || char.IsDigit(e.Text, 0))
            {
                return; // Allow the input
            }

            // If the character is not valid, cancel the event
            e.Handled = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // Handle save logic
            var viewModel = (IClusterRegistrationVM)DataContext;
            viewModel.Result = false;  // Indicate that the user cancelled
        }
    }
}
