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

    private static IServiceCollection? Services { get; set; }


    public new static App Current => (App)Application.Current!;

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

    public void ConfigureServices(IServiceCollection? services)
    {
        const string connectionString = """
                                        Host=localhost;
                                        Port=5432;
                                        Username="postgres";
                                        Password="postgres";
                                        Database="esport_manager_db";
                                        Include Error Detail=true;
                                        """;

        // commands
        if (services == null) return;
        services.AddSingleton<IPlayerCommands, PlayerCommands>(_ =>
            new PlayerCommands(connectionString));
        services.AddSingleton<ITournamentCommands, TournamentCommands>(_ =>
            new TournamentCommands(connectionString));
        services.AddSingleton<ITrainingCommands, TrainingCommands>(_ =>
            new TrainingCommands(connectionString));


        // services
        services.AddSingleton<IPlayerService, PlayerService>();
        services.AddSingleton<ITournamentService, TournamentService>();
        services.AddSingleton<ITrainingService, TrainingService>();
        services.AddSingleton<HistoryService>();


        // view or window models
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<PlayersViewModel>();
        services.AddTransient<AddPlayerWindowModel>();
        services.AddTransient<TournamentsViewModel>();
        services.AddTransient<SelectPlayerWindowModel>();
        services.AddTransient<AddTournamentWindowModel>();
        services.AddTransient<TrainingsViewModel>();
        services.AddTransient<AddTrainingWindowModel>();
        services.AddTransient<LeaderboardViewModel>();
        services.AddTransient<HistoryViewModel>();


        // views
        services.AddTransient<TrainingsView>();
        services.AddTransient<TournamentsView>();
        services.AddTransient<PlayersView>();
        services.AddTransient<LeaderboardView>();
        services.AddTransient<HistoryView>();


        // views datacontexts
        services.AddTransient<TournamentsView>(provider =>
        {
            var view = new TournamentsView();
            view.DataContext = provider.GetRequiredService<TournamentsViewModel>();
            return view;
        });

        services.AddTransient<PlayersView>(provider =>
        {
            var view = new PlayersView();
            view.DataContext = provider.GetRequiredService<PlayersViewModel>();
            return view;
        });

        services.AddTransient<TrainingsView>(provider =>
        {
            var view = new TrainingsView();
            view.DataContext = provider.GetRequiredService<TrainingsViewModel>();
            return view;
        });
        services.AddTransient<LeaderboardView>(provider =>
        {
            var view = new LeaderboardView();
            view.DataContext = provider.GetRequiredService<LeaderboardViewModel>();
            return view;
        });
        services.AddTransient<HistoryView>(provider =>
        {
            var view = new HistoryView();
            view.DataContext = provider.GetRequiredService<HistoryViewModel>();
            return view;
        });

        // database
        services.AddSingleton<DatabaseCommands>(_ => new DatabaseCommands(connectionString));


        // windows
        services.AddTransient<AddPlayer>();
        services.AddTransient<SelectPlayer>();
        services.AddTransient<AddTournament>();
        services.AddTransient<AddTraining>();
    }

    public T GetService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}