using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportManager.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EsportManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    [ObservableProperty] private object? _currentView;

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        CurrentView = _serviceProvider.GetRequiredService<PlayersView>();
    }

    [RelayCommand]
    private void ShowPlayersView()
    {
        CurrentView = _serviceProvider.GetRequiredService<PlayersView>();
    }

    [RelayCommand]
    private void ShowTournamentsView()
    {
        CurrentView = _serviceProvider.GetRequiredService<TournamentsView>();
    }

    [RelayCommand]
    private void ShowTrainingsView()
    {
        CurrentView = _serviceProvider.GetRequiredService<TrainingsView>();
    }
}