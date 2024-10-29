using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ViewLocator.ViewModels;

public partial class MinePageViewModel : PageViewModelBase
{
    [ObservableProperty] private string? _username;

    [ObservableProperty] private string? _password;

    public MinePageViewModel()
    {
    }

    [RelayCommand]
    private void Login()
    {
        Console.WriteLine("Login");
    }

    [RelayCommand]
    private void Cancel()
    {
        Username = string.Empty;
        Password = string.Empty;
    }
}