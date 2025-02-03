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

namespace DashBoard.View
{
    /// <summary>
    /// Interaction logic for RegisterApplicationDialogue.xaml
    /// </summary>
    public partial class ApplicationDialog : Window
    {
        public ApplicationDialog(IApplicationDialogVM vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            // Handle save logic
            DialogResult = true;  // Indicate that the user saved the data
            Close();
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
                var viewModel = (ApplicationDialogVM)DataContext;
                viewModel.ExecutablePath = openFileDialog.FileName;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the entered character is a valid digit or decimal point
            if (e.Text == "." && !(DataContext as ApplicationDialogVM).VersionNumber.Any(c => c == '.') || char.IsDigit(e.Text, 0))
            {
                return; // Allow the input
            }

            // If the character is not valid, cancel the event
            e.Handled = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // Handle save logic
            DialogResult = false;  // Indicate that the user did not save the data
            Close();
        }
    }
}
