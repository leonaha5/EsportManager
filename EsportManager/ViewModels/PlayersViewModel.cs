using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;
using EsportManager.Windows;

// using EsportManager.Windows;

namespace EsportManager.ViewModels;

public partial class PlayersViewModel : ViewModelBase, IRecipient<AddPlayerWindowModel.PlayerAddedMessage>
{
    private readonly IPlayerService _playerService;
    [ObservableProperty] private ObservableCollection<Player> _players = [];
    [ObservableProperty] private Player? _selectedPlayer;
    [ObservableProperty] private string _title = "Players";

    public PlayersViewModel(IPlayerService playerService)
    {
        _playerService = playerService;
        _ = LoadPlayersAsync();

        WeakReferenceMessenger.Default.Register(this);
    }

    public async void Receive(AddPlayerWindowModel.PlayerAddedMessage message)
    {
        await LoadPlayersAsync();
    }

    [RelayCommand]
    private async Task DeletePlayerFromDatabase()
    {
        if (SelectedPlayer is null) return;
        await _playerService.DeletePlayerAsync(SelectedPlayer.Id);
        Players.Clear();
        await LoadPlayersAsync();
    }

    private async Task LoadPlayersAsync()
    {
        try
        {
            var players = await _playerService.GetAllPlayersAsync();
            Players.Clear();
            foreach (var player in players) Players.Add(player);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Loading Players Async error!!: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddPlayerWindow()
    {
        var addPlayerWindow = new AddPlayer();
        addPlayerWindow.Show();
    }
}