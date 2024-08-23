using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DLLInjector.Services;
using DLLInjector.Services.Interfaces;
using DLLInjector.ViewModels;
using DLLInjector.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DLLInjector;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = ConfigureServices();

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<MainWindowViewModel>();

        services.AddTransient<ProcessWindowViewModel>();
        services.AddTransient<ProcessWindow>(sp => new ProcessWindow
        {
            DataContext = sp.GetService<ProcessWindowViewModel>()
        });

        services.AddSingleton<IWindowManagerService, WindowManagerService>();
        services.AddTransient<IInjectService, InjectService>();

        return services.BuildServiceProvider();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var vm = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}