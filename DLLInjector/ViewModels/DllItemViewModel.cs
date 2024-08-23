using CommunityToolkit.Mvvm.ComponentModel;
using DLLInjector.Models;

namespace DLLInjector.ViewModels;

public partial class DllItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _filepath;

    public DllItemViewModel()
    {
    }

    public DllItemViewModel(DllItem item)
    {
        item.Name = _name;
        item.FilePath = _filepath;
    }
}