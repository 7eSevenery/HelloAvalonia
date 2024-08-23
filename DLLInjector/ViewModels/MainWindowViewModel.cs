using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DLLInjector.Services.Interfaces;
using DLLInjector.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DLLInjector.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IWindowManagerService? _windowManagerService;
    private readonly IInjectService? _injectService;

    public ObservableCollection<DllItemViewModel> DllItems { get; } = [];

    public DllItemViewModel? SelectedDllItem { set; get; }

    [ObservableProperty] private ProcessItemViewModel? _selectedProcess;

    public MainWindowViewModel()
    {
        if (Design.IsDesignMode)
        {
            DllItems = new ObservableCollection<DllItemViewModel>(new[]
            {
                new DllItemViewModel() { Name = "Hello", Filepath = "Hello, DLL Injector." },
            });
        }
    }

    public MainWindowViewModel(IWindowManagerService wms, IInjectService ins)
    {
        _windowManagerService = wms;
        _injectService = ins;

        WeakReferenceMessenger.Default.Register<ProcessItemViewModel, string>(this, "GetProcId",
            (_, m) => { SelectedProcess = m; });
    }

    [RelayCommand]
    private void ShowProcessWindow()
    {
        _windowManagerService?.Show<ProcessWindowViewModel>();
    }

    [RelayCommand]
    private async Task Add(Window window)
    {
        var result = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select File",
            AllowMultiple = true
        });

        if (result.Count > 0)
        {
            foreach (var selected in result)
            {
                DllItems.Add(new DllItemViewModel() { Name = selected.Name, Filepath = selected.Path.AbsolutePath });
            }
        }
    }

    [RelayCommand]
    private void Remove()
    {
        if (SelectedDllItem != null)
        {
            DllItems.Remove(SelectedDllItem);
        }
    }

    [RelayCommand]
    private void Clear()
    {
        DllItems.Clear();
    }

    [RelayCommand]
    private async Task InjectDll()
    {
        if (_injectService == null || SelectedProcess?.Pid == null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Info", "Error.");
            await box.ShowWindowAsync();
            return;
        }
        
        foreach (var dll in DllItems)
        {
            if (dll.Filepath == null)
            {
                continue;
            }

            var ret = _injectService.Inject(SelectedProcess.Pid.Value, dll.Filepath);
            if (ret == true)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Info", "Success.");
                await messageBox.ShowWindowAsync();
            }
            else
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Info", "Failure.");
                await messageBox.ShowWindowAsync();
            }
        }
    }
}