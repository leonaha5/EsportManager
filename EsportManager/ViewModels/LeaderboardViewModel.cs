using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EsportManager.Models;
using EsportManager.Services;

namespace EsportManager.ViewModels;

public partial class LeaderboardViewModel : ViewModelBase
{
    private readonly IPlayerService _playerService;
    [ObservableProperty] private ObservableCollection<Player> _players = [];

    public LeaderboardViewModel(IPlayerService playerService)
    {
        _playerService = playerService;
        _ = LoadPlayersAsync();
    }


    private async Task LoadPlayersAsync()
    {
        try
        {
            var players = await _playerService.GetAllPlayersAsync();
            Players.Clear();
            foreach (var player in players.OrderByDescending(p => p.Points)) Players.Add(player);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}