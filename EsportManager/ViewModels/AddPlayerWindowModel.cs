using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;

namespace EsportManager.ViewModels;

public partial class AddPlayerWindowModel : ObservableObject
{
    private readonly IPlayerService _playerService;
    private Window _addPlayerWindow;

    [ObservableProperty] private string _game;
    [ObservableProperty] private string _nickname;

    public AddPlayerWindowModel(IPlayerService playerService)
    {
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

        await _playerService.AddPlayerAsync(newPlayer);

        WeakReferenceMessenger.Default.Send(new PlayerAddedMessage());

        _addPlayerWindow.Close();
    }

    public class PlayerAddedMessage;
}