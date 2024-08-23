using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DLLInjector.Services.Interfaces;

namespace DLLInjector.ViewModels;

public partial class ProcessWindowViewModel : ViewModelBase
{
    private readonly IWindowManagerService? _windowManagerService;
    
    public ObservableCollection<ProcessItemViewModel> ProcessItems { get; } = [];

    public ProcessItemViewModel? SelectedProcess { set; get; }

    public ProcessWindowViewModel()
    {
        if (Design.IsDesignMode)
        {
            ProcessItems = new ObservableCollection<ProcessItemViewModel>(new[]
            {
                new ProcessItemViewModel() { Name = "Notepad.txt", Pid = 1024 },
                new ProcessItemViewModel() { Name = "Explorer.txt", Pid = 10000 },
            });
        }
    }
    
    public ProcessWindowViewModel(IWindowManagerService wms)
    {
        ProcessItems = new ObservableCollection<ProcessItemViewModel>();
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            try
            {
                if (process.MainModule != null &&
                    process.MainModule.FileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    ProcessItems.Add(
                        new ProcessItemViewModel()
                        {
                            Name = process.ProcessName,
                            Pid = process.Id
                        }
                    );
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Get MainModule Error.");
            }
        }
        
        _windowManagerService = wms;
    }

    [RelayCommand]
    private void SelectProcess()
    {
        if (SelectedProcess != null)
        {
            WeakReferenceMessenger.Default.Send(SelectedProcess,  "GetProcId");
        }
        _windowManagerService?.Close<ProcessWindowViewModel>();
    }
    
    [RelayCommand]
    private void CancelSelect()
    {
        _windowManagerService?.Close<ProcessWindowViewModel>();
    }
}