using DashBoard.Core.EventAggregator.Events;
using DashBoard.Core.EventAggregator.interfaces;
using DashBoard.ViewModel.interfaces;
using MVVM_FrameWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace DashBoard.ViewModel;
public class ApplicationSelectorVM : BaseViewModel, IApplicationSelectorVM
{
    private readonly IEventAggregator _eventAggregator;
    private readonly ObservableCollection<IApplicationVM> _allApplications;
    private string _selectedSortOption;
    private string _searchQuery;
    private bool? _result;

    public ApplicationSelectorVM(string title, List<IApplicationVM> applications, IEventAggregator eventAggregator)
    {
        Title = title;
        _eventAggregator = eventAggregator;

        _allApplications = [];
        _result = null;
        FilteredApplications = new ObservableCollection<IApplicationVM>(applications);

        foreach (IApplicationVM app in _allApplications) 
        { 
            _allApplications.Add(app); 
        }

        AcceptCommand = new RelayCommand( o => Accept());
        CancelCommand = new RelayCommand( o => Cancel());
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            NotifyPropertyChanged(nameof(SearchQuery));
            ApplyFilters();
        }
    }
    public ObservableCollection<IApplicationVM> FilteredApplications { get; set; }
    public ObservableCollection<IApplicationVM> SelectedApplications { get; set; }

    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set
        {
            _selectedSortOption = value;
            NotifyPropertyChanged(nameof(SelectedSortOption));
            ApplyFilters();
        }
    }

    public ICommand AcceptCommand { get; }
    public ICommand CancelCommand { get; }

    public string Title { get; private set; }
    public Guid guid { get; } = Guid.NewGuid();

    public bool? Result
    {
        get => _result;
        set
        {
            if( _result != value )
            {
                _result = value;
                NotifyPropertyChanged(nameof(Result));
                _eventAggregator.Publish(new CloseDialogEvent() { DialogID = guid});
            }
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allApplications.Where(a => string.IsNullOrEmpty(SearchQuery) || a.ApplicationName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));

        if (SelectedSortOption == "Title")
            filtered = filtered.OrderBy(a => a.ApplicationName);
        else if (SelectedSortOption == "Date Added")
            filtered = filtered.OrderBy(a => a.ApplicationDateAdded);

        FilteredApplications.Clear();
        foreach (var app in filtered)
        {
            FilteredApplications.Add(app);
        }
    }

    private void Accept()
    {
        var e = new AcceptSelectedAppsEvent();
        e.SelectedApps = [];
        foreach( var app in SelectedApplications)
        {
            e.SelectedApps.Add(app.ApplicationGuid); 
        }
        Result = true;
        _eventAggregator.Publish(e);
    }

    private void Cancel()
    {
        Result = false;
        _eventAggregator.Publish(new CloseDialogEvent());
    }
}

