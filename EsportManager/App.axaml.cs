using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EsportManager.Commands;
using EsportManager.Services;
using EsportManager.ViewModels;
using EsportManager.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EsportManager;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;
    public static IServiceCollection Services { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>(),
            };

            var databaseRepository = _serviceProvider.GetRequiredService<DatabaseCommands>();
            databaseRepository.InitializeDatabase();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        const string connectionString = """
                                        Host=localhost;
                                        Port=5432;
                                        Username="postgres";
                                        Password="postgres";
                                        Database="esport_manager_db";
                                        """;
        services.AddSingleton<IPlayerCommands, PlayerCommands>(provider => new PlayerCommands(connectionString));
 
        services.AddSingleton<IPlayerService, PlayerService>();
        
        services.AddTransient<PlayersViewModel>();

        services.AddTransient<TrainingsView>();
        services.AddTransient<TournamentsView>();
        services.AddTransient<PlayersView>();
        
        services.AddSingleton<MainWindowViewModel>();
        
        services.AddSingleton<DatabaseCommands>(provider => new DatabaseCommands(connectionString));
        
    }
    
    public new static App? Current => (App)Application.Current!;
    public T GetService<T>() => _serviceProvider.GetRequiredService<T>();

    public App()
    {
        Services = new ServiceCollection();
        ConfigureServices(Services);
        _serviceProvider = Services.BuildServiceProvider();
    }
}