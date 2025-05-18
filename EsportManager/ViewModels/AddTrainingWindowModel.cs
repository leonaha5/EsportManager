using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EsportManager.Models;
using EsportManager.Services;

namespace EsportManager.ViewModels;

public partial class AddTrainingWindowModel : ObservableObject
{
    private readonly ITrainingService _trainingService;
    private Window _addTrainingWindow;
    [ObservableProperty] private int _fatigueIncrease;
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _skillIncrease;
    [ObservableProperty] private int _stressIncrease;

    public AddTrainingWindowModel(ITrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    public void SetWindow(Window window)
    {
        _addTrainingWindow = window;
    }


    [RelayCommand]
    private async Task AddTrainingToDatabase()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        var newTraining = new Training
        {
            Name = Name,
            SkillIncrease = SkillIncrease,
            FatigueIncrease = FatigueIncrease,
            StressIncrease = StressIncrease
        };

        await _trainingService.AddTrainingAsync(newTraining);

        WeakReferenceMessenger.Default.Send(new TrainingAddedMessage());

        _addTrainingWindow.Close();
    }

    public class TrainingAddedMessage;
}