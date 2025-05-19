using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;
using EsportManager.Windows;

namespace EsportManager.ViewModels;

public partial class TrainingsViewModel : ViewModelBase, IRecipient<SelectPlayerWindowModel.PlayerTrainedMessage>,
    IRecipient<AddTrainingWindowModel.TrainingAddedMessage>
{
    private readonly IPlayerService _playerService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITrainingService _trainingService;
    [ObservableProperty] private Training _selectedTraining;
    [ObservableProperty] private ObservableCollection<Training> _trainings = [];

    public TrainingsViewModel(ITrainingService trainingService, IPlayerService playerService,
        IServiceProvider serviceProvider)
    {
        _trainingService = trainingService;
        _playerService = playerService;
        _serviceProvider = serviceProvider;
        LoadTrainingAsync();

        WeakReferenceMessenger.Default.Register<SelectPlayerWindowModel.PlayerTrainedMessage>(this);
        WeakReferenceMessenger.Default.Register<AddTrainingWindowModel.TrainingAddedMessage>(this);
    }

    public async void Receive(SelectPlayerWindowModel.PlayerTrainedMessage message)
    {
        Debug.WriteLine($"Received training {message.ChosenTraining.Name} for player {message.TrainedPlayer.Nickname}");

        if (SelectedTraining.Id == message.ChosenTraining.Id)
        {
            await _playerService.TrainPlayerAsync(message.TrainedPlayer, message.ChosenTraining);
            SelectedTraining = null;
        }
        else
        {
            Debug.WriteLine("Training is corrupted");
        }
    }

    public void Receive(AddTrainingWindowModel.TrainingAddedMessage message)
    {
        LoadTrainingAsync();
    }

    public async void LoadTrainingAsync()
    {
        var trainings = await _trainingService.GetAllTrainingsAsync();
        Trainings.Clear();
        foreach (var training in trainings) Trainings.Add(training);
    }

    [RelayCommand]
    private void ParticipateInTraining(Training? training)
    {
        if (training == null) return;

        var playerSelectionWindow = App.Current?.GetService<SelectPlayer>();

        if (playerSelectionWindow?.DataContext is not SelectPlayerWindowModel vm) return;
        vm.InitializeTraining(App.Current?.GetService<IPlayerService>(), SelectedTraining);
        playerSelectionWindow.Show();
    }


    [RelayCommand]
    private void AddTrainingWindow()
    {
        var addTrainingWindow = new AddTraining();
        addTrainingWindow.Show();
    }
}