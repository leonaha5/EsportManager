using Avalonia.Controls;
using EsportManager.ViewModels;

namespace EsportManager.Windows;

public partial class AddTraining : Window
{
    public AddTraining()
    {
        InitializeComponent();
        DataContext = App.Current?.GetService<AddTrainingWindowModel>();
        if (DataContext is AddTrainingWindowModel vm) vm.SetWindow(this);
    }
}