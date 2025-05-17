using Avalonia.Controls;
using EsportManager.ViewModels;

namespace EsportManager.Windows;

public partial class SelectPlayer : Window
{
    public SelectPlayer()
    {
        InitializeComponent();
        DataContext = App.Current?.GetService<SelectPlayerWindowModel>();
        if (DataContext is SelectPlayerWindowModel vm) vm.SetWindow(this);
    }
}