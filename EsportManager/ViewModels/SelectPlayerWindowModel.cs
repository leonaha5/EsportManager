using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;

namespace EsportManager.ViewModels;

public partial class SelectPlayerWindowModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<Player> _players;
    private Window? _playerSelectionWindow;
    private IPlayerService _playerService;
    [ObservableProperty] private Player? _selectedPlayer;
    private Tournament? _selectedTournament;
    private ITournamentService _tournamentService;

    public SelectPlayerWindowModel(IPlayerService playerService, ITournamentService tournamentService)
    {
        _playerService = playerService;
        _tournamentService = tournamentService;
    }

    public Tournament? SelectedTournament
    {
        set => SetProperty(ref _selectedTournament, value);
    }

    public void InitializeTournament(IPlayerService playerService, Tournament selectedTournament)
    {
        _playerService = playerService;
        SelectedTournament = selectedTournament;
        LoadPlayersAsync();
    }

    public void SetWindow(Window window)
    {
        _playerSelectionWindow = window;
    }

    private async void LoadPlayersAsync()
    {
        var players = await _playerService.GetAllPlayersAsync();
        Players = new ObservableCollection<Player>(players);
    }

    [RelayCommand]
    private void SelectPlayer()
    {
        if (SelectedPlayer == null || _playerSelectionWindow == null) return;
        if (_selectedTournament == null) return;
        WeakReferenceMessenger.Default.Send(new PlayerTournamentMessage(SelectedPlayer, _selectedTournament));
        _playerSelectionWindow.Close();
    }

    public class PlayerTournamentMessage
    {
        public PlayerTournamentMessage(Player tournamentPlayer, Tournament chosenTournament)
        {
            TournamentPlayer = tournamentPlayer;
            ChosenTournament = chosenTournament;
        }

        public Player TournamentPlayer { get; set; }
        public Tournament ChosenTournament { get; set; }
    }
}