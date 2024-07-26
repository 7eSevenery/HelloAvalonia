using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MouseClicker.Services;

namespace MouseClicker.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private int _count;

    [ObservableProperty] private int _interval;

    [ObservableProperty] private bool _canClick;

    public MainWindowViewModel()
    {
        Count = 100;
        Interval = 100;
        CanClick = true;
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    private void StartClick()
    {
        CanClick = false;

        Task.Run(() =>
        {
            for (int i = 0; i < Count; i++)
            {
                if (CanClick)
                {
                    break;
                }

                CursorService.MouseClick();
                Thread.Sleep(Interval);
            }

            CanClick = true;
        });
    }

    [RelayCommand]
    private void StopClick()
    {
        CanClick = true;
    }
}