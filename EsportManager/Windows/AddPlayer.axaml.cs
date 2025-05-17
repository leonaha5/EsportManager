using Avalonia.Controls;
using EsportManager.ViewModels;

namespace EsportManager.Windows;

public partial class AddPlayer : Window
{
    public AddPlayer()
    {
        InitializeComponent();
        DataContext = App.Current.GetService<AddPlayerWindowModel>();
        if (DataContext is AddPlayerWindowModel vm) vm.SetWindow(this);
    }
}