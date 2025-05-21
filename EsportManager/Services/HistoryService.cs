using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EsportManager.Models;

namespace EsportManager.Services;

public class HistoryService
{
    private static HistoryService? _instance;
    public static HistoryService Instance => _instance ??= new HistoryService();

    public ObservableCollection<HistoryRecord> HistoryRecords { get; } = [];

    public Task AddRecord(string operation)
    {
        HistoryRecords.Add(new HistoryRecord
        {
            Timestamp = DateTime.Now,
            Operation = operation
        });
        return Task.CompletedTask;
    }
}