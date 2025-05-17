using Avalonia.Controls;
using EsportManager.ViewModels;

namespace EsportManager.Windows;

public partial class AddTournament : Window
{
    public AddTournament()
    {
        InitializeComponent();
        DataContext = App.Current?.GetService<AddTournamentWindowModel>();
        if (DataContext is AddTournamentWindowModel vm) vm.SetWindow(this);
    }
}