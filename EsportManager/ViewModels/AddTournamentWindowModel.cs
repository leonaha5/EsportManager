using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportManager.Models;
using EsportManager.Services;
using IMessengerExtensions = CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace EsportManager.ViewModels;

public partial class AddTournamentWindowModel : ObservableObject
{
    private readonly ITournamentService _tournamentService;
    private Window _addTournamentWindow;
    [ObservableProperty] private int _entryFee;
    [ObservableProperty] private int _minSkillRequired;
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _prizePool;

    public AddTournamentWindowModel(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    public void SetWindow(Window window)
    {
        _addTournamentWindow = window;
    }


    [RelayCommand]
    private async Task AddTournamentToDatabase()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        var newPlayer = new Tournament
        {
            Name = Name,
            EntryFee = EntryFee,
            PrizePool = PrizePool,
            MinSkillRequired = MinSkillRequired
        };

        await _tournamentService.AddTournamentAsync(newPlayer);

        IMessengerExtensions.Send(WeakReferenceMessenger.Default, new TournamentAddedMessage());

        _addTournamentWindow.Close();
    }

    public class TournamentAddedMessage;
}