using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EsportManager.Commands;
using EsportManager.Services;
using EsportManager.ViewModels;
using EsportManager.Views;
using EsportManager.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace EsportManager;

public class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        Services = new ServiceCollection();
        ConfigureServices(Services);
        _serviceProvider = Services.BuildServiceProvider();
    }

    public static IServiceCollection Services { get; private set; }

    public new static App? Current => (App)Application.Current!;

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
                DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>()
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
                                        Include Error Detail=true;
                                        """;
        services.AddSingleton<IPlayerCommands, PlayerCommands>(provider => new PlayerCommands(connectionString));

        services.AddSingleton<IPlayerService, PlayerService>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<PlayersViewModel>();
        services.AddTransient<AddPlayerWindowModel>();

        services.AddTransient<TrainingsView>();
        services.AddTransient<TournamentsView>();
        services.AddTransient<PlayersView>();

        services.AddTransient<PlayersView>(provider =>
        {
            var view = new PlayersView();
            view.DataContext = provider.GetRequiredService<PlayersViewModel>();
            return view;
        });


        services.AddSingleton<DatabaseCommands>(provider => new DatabaseCommands(connectionString));

        services.AddTransient<AddPlayer>();
    }

    public T GetService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}