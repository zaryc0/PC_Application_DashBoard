using DashBoard.ViewModel.interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public interface IApplicationSelectorVM : IDialogContentVM
{
    ObservableCollection<IApplicationVM> FilteredApplications { get; set; }
    ObservableCollection<IApplicationVM> SelectedApplications { get; set; }
    string SearchQuery { get; set; }
    string SelectedSortOption { get; set; }
    ICommand AcceptCommand { get; }
    ICommand CancelCommand { get; }
}
