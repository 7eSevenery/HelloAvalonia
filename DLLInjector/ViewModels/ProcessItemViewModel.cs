using CommunityToolkit.Mvvm.ComponentModel;
using DLLInjector.Models;

namespace DLLInjector.ViewModels;

public partial class ProcessItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int? _pid;

    public ProcessItemViewModel()
    {
    }

    public ProcessItemViewModel(ProcessItem item)
    {
        item.Name = _name;
        item.Pid = _pid;
    }
}