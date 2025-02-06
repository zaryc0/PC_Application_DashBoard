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
    private bool _sortDescending;
    private bool? _result;

    public ApplicationSelectorVM(string title, List<IApplicationVM> applications, IEventAggregator eventAggregator)
    {
        Title = title;
        _eventAggregator = eventAggregator;
        _allApplications = [];
        _result = null;
        _sortDescending = false;
        _selectedSortOption = "Title";

        FilteredApplications = new ObservableCollection<IApplicationVM>(applications);
        SelectedApplications = new ObservableCollection<IApplicationVM>();
        foreach (IApplicationVM app in applications) 
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
    public bool SortDescending
    {
        get => _sortDescending;
        set
        {
            if (_sortDescending != value)
            {
                _sortDescending = value;
                NotifyPropertyChanged(nameof(SortDescending));
                ApplyFilters();
            }
        }
    }
    public bool? Result
    {
        get => _result;
        set
        {
            if( _result != value )
            {
                _result = value;
                NotifyPropertyChanged(nameof(Result));
                _eventAggregator.Publish(new CloseDialogEvent(guid));
            }
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allApplications.Where(a => string.IsNullOrEmpty(SearchQuery) || a.ApplicationName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));

        if (SelectedSortOption == "Title")
        {
            if (_sortDescending)
            {
                filtered = filtered.OrderByDescending(a => a.ApplicationName).ToList();
            }
            else
            {
                filtered = filtered.OrderBy(a => a.ApplicationName).ToList();
            }
        }
        else if (SelectedSortOption == "Date Added")
        {
            if (_sortDescending)
            {
                filtered = filtered.OrderByDescending(a => DateTime.Parse(a.ApplicationDateAdded)).ToList();
            }
            else
            {
            filtered = filtered.OrderBy(a => DateTime.Parse(a.ApplicationDateAdded)).ToList();
            }
        }

        FilteredApplications.Clear();
        foreach (var app in filtered)
        {
            FilteredApplications.Add(app);
        }
    }

    private void Accept()
    {
        var e = new UpdateSelectedAppsEvent
        {
            app_ids = []
        };
        foreach ( var app in SelectedApplications)
        {
            e.app_ids.Add(app.ApplicationGuid); 
        }
        Result = true;
        _eventAggregator.Publish(e);
    }

    private void Cancel()
    {
        Result = false;
        _eventAggregator.Publish(new CloseDialogEvent(guid));
    }
}

