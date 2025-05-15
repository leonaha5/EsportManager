using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportManager.Models;
using EsportManager.Windows;

// using EsportManager.Windows;

namespace EsportManager.ViewModels;

public partial class PlayersViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<Player> _players = [];
    [ObservableProperty] private Player? _selectedPlayer;
    [ObservableProperty] private string _title = "Players";


    [RelayCommand]
    public void AddPlayerWindow()
    {
        var addPlayerWindow = new AddPlayerWindow();
        addPlayerWindow.Show();
    }
}