using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportManager.Models;
using EsportManager.Services;
using IMessengerExtensions = CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;
using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;

namespace EsportManager.ViewModels;

public partial class AddPlayerWindowModel : ObservableObject
{
    private readonly IPlayerService _playerService;
    private Window? _addPlayerWindow;
    [ObservableProperty] private string _game;
    [ObservableProperty] private string _nickname;

    public AddPlayerWindowModel(IPlayerService playerService)
    {
        _game = string.Empty;
        _nickname = string.Empty;
        _playerService = playerService;
    }

    public void SetWindow(Window window)
    {
        _addPlayerWindow = window;
    }


    [RelayCommand]
    private async Task AddPlayerToDatabase()
    {
        if (string.IsNullOrWhiteSpace(Nickname) ||
            string.IsNullOrWhiteSpace(Game))
            return;

        var newPlayer = new Player
        {
            Nickname = Nickname,
            SkillLevel = 0,
            StressLevel = 0,
            FatigueLevel = 0,
            Game = Game,
            Points = 0,
            Money = 0
        };
        _ = HistoryService.Instance.AddRecord($"""Player "{Nickname}" Added!""");
        await _playerService.AddPlayerAsync(newPlayer);
        IMessengerExtensions.Send(WeakReferenceMessenger.Default, new PlayerAddedMessage());
        _addPlayerWindow?.Close();
    }

    public class PlayerAddedMessage;
}