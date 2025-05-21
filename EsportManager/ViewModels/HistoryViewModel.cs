using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EsportManager.Models;
using EsportManager.Services;

namespace EsportManager.ViewModels;

public partial class HistoryViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<HistoryRecord> _records = [];

    public HistoryViewModel()
    {
        LoadHistory();
    }


    private void LoadHistory()
    {
        Records.Clear();
        foreach (var record in HistoryService.Instance.HistoryRecords
                     .OrderByDescending(record => record.Timestamp)) Records.Add(record);
    }
}