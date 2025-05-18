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
    private Training? _selectedTraining;
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

    public Training? SelectedTraining
    {
        set => SetProperty(ref _selectedTraining, value);
    }

    public void InitializeTournament(IPlayerService playerService, Tournament selectedTournament)
    {
        _playerService = playerService;
        SelectedTournament = selectedTournament;
        LoadPlayersAsync();
    }

    public void InitializeTraining(IPlayerService playerService, Training selectedTraining)
    {
        _playerService = playerService;
        SelectedTraining = selectedTraining;
        SelectedTournament = null;
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
        if (_selectedTraining != null)
        {
            WeakReferenceMessenger.Default.Send(new PlayerTrainedMessage(SelectedPlayer, _selectedTraining));
            _playerSelectionWindow.Close();
        }
        else if (_selectedTournament != null)
        {
            WeakReferenceMessenger.Default.Send(new PlayerTournamentMessage(SelectedPlayer, _selectedTournament));
            _playerSelectionWindow.Close();
        }
    }

    public class PlayerTrainedMessage(Player trainedPlayer, Training chosenTraining)
    {
        public Player TrainedPlayer { get; set; } = trainedPlayer;
        public Training ChosenTraining { get; set; } = chosenTraining;
    }

    public class PlayerTournamentMessage(Player tournamentPlayer, Tournament chosenTournament)
    {
        public Player TournamentPlayer { get; set; } = tournamentPlayer;
        public Tournament ChosenTournament { get; set; } = chosenTournament;
    }
}