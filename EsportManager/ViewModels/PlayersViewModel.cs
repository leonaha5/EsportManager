using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;
using EsportManager.Windows;

// using EsportManager.Windows;

namespace EsportManager.ViewModels;

public partial class PlayersViewModel : ViewModelBase, IRecipient<AddPlayerWindowModel.PlayerAddedMessage>,
    IRecipient<TournamentsViewModel.PlayerJoinedTournamentMessage>
{
    private readonly IPlayerService _playerService;
    [ObservableProperty] private ObservableCollection<Player> _players = [];
    [ObservableProperty] private Player? _selectedPlayer;

    public PlayersViewModel(IPlayerService playerService)
    {
        _playerService = playerService;
        _ = LoadPlayersAsync();

        WeakReferenceMessenger.Default.Register<AddPlayerWindowModel.PlayerAddedMessage>(this);
        WeakReferenceMessenger.Default.Register<TournamentsViewModel.PlayerJoinedTournamentMessage>(this);
    }

    public async void Receive(AddPlayerWindowModel.PlayerAddedMessage message)
    {
        await LoadPlayersAsync();
    }

    public async void Receive(TournamentsViewModel.PlayerJoinedTournamentMessage message)
    {
        await LoadPlayersAsync();
    }

    [RelayCommand]
    private async Task DeletePlayerFromDatabase()
    {
        if (SelectedPlayer is null) return;

        _ = HistoryService.Instance.AddRecord($"""Player "{SelectedPlayer.Nickname}" Deleted! Bye!""");
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

            var sortedPlayers = players.OrderBy(p => p.Points);

            foreach (var player in sortedPlayers) Players.Add(player);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    private void AddPlayerWindow()
    {
        var addPlayerWindow = new AddPlayer();
        addPlayerWindow.Show();
    }
}