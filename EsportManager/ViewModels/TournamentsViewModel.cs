using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;
using EsportManager.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace EsportManager.ViewModels;

public partial class TournamentsViewModel : ViewModelBase, IRecipient<SelectPlayerWindowModel.PlayerTournamentMessage>,
    IRecipient<AddTournamentWindowModel.TournamentAddedMessage>
{
    private readonly IPlayerService _playerService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITournamentService _tournamentService;
    [ObservableProperty] private Tournament? _selectedTournament;
    [ObservableProperty] private ObservableCollection<Tournament> _tournaments = [];

    public TournamentsViewModel(ITournamentService tournamentService, IServiceProvider serviceProvider,
        IPlayerService playerService)
    {
        _tournamentService = tournamentService;
        _serviceProvider = serviceProvider;
        _playerService = playerService;
        _ = LoadTournamentAsync();
        WeakReferenceMessenger.Default.Register<SelectPlayerWindowModel.PlayerTournamentMessage>(this);
        WeakReferenceMessenger.Default.Register<AddTournamentWindowModel.TournamentAddedMessage>(this);
    }

    public async void Receive(SelectPlayerWindowModel.PlayerTournamentMessage message)
    {
        await _playerService.JoinTournamentAsync(message.TournamentPlayer, message.ChosenTournament);
        await LoadTournamentAsync();
        WeakReferenceMessenger.Default.Send(new PlayerJoinedTournamentMessage());

        SelectedTournament = null;
    }

    public async void Receive(AddTournamentWindowModel.TournamentAddedMessage message)
    {
        await LoadTournamentAsync();
    }

    [RelayCommand]
    private async Task DeleteTournamentFromDatabase()
    {
        if (SelectedTournament is null) return;
        await _tournamentService.DeleteTournamentAsync(SelectedTournament);
        Tournaments.Clear();
        await LoadTournamentAsync();
    }


    private async Task LoadTournamentAsync()
    {
        var tournaments = await _tournamentService.GetAllTournamentAsync();
        Tournaments.Clear();
        foreach (var tournament in tournaments) Tournaments.Add(tournament);
    }

    [RelayCommand]
    private void ParticipateInTournament(Tournament? tournament)
    {
        if (tournament == null) return;

        var playerSelection = _serviceProvider.GetRequiredService<SelectPlayer>();

        if (playerSelection.DataContext is not SelectPlayerWindowModel vm) return;

        var playerService = _serviceProvider.GetRequiredService<IPlayerService>();
        vm.InitializeTournament(playerService, tournament);
        playerSelection.Show();
    }

    [RelayCommand]
    private void AddTournamentWindow()
    {
        var addTournamentWindow = new AddTournament();
        addTournamentWindow.Show();
    }

    public class PlayerJoinedTournamentMessage;
}