namespace EsportManager.ViewModels;

using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EsportManager.Views;
using Microsoft.Extensions.DependencyInjection;


public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PlayersView _playersView;
    [ObservableProperty] private object? _currentView;
    private readonly IServiceProvider _serviceProvider;

    public MainWindowViewModel(IServiceProvider serviceProvider, PlayersView playersView)
    {
        _serviceProvider = serviceProvider;
        _playersView = playersView;
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