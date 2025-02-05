using DashBoard.ViewModel;
using DashBoard.ViewModel.interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DashBoard.View.UI
{
    /// <summary>
    /// Interaction logic for ApplicationRegistration.xaml
    /// </summary>
    public partial class ApplicationRegistration : UserControl
    {
        public ApplicationRegistration()
        {
            InitializeComponent();
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            // Handle save logic
            var viewModel = (IApplicationRegistrationVM)DataContext;
            viewModel.Result = true;  // Indicate that the user saved the data
        }
        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                Title = "Select Executable"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Set the ExecutablePath in the ViewModel to the selected file path
                var viewModel = (IApplicationRegistrationVM)DataContext;
                viewModel.ExecutablePath = openFileDialog.FileName;
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
            var viewModel = (IApplicationRegistrationVM)DataContext;
            viewModel.Result = false;  // Indicate that the user cancelled
        }
    }
}
