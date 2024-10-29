using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ViewLocator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private PageViewModelBase _CurrentPage;

    private readonly PageViewModelBase[] Pages =
    {
        new HomePageViewModel(),
        new CommunityPageViewModel(),
        new MinePageViewModel(),
    };

    public MainWindowViewModel()
    {
        _CurrentPage = Pages[0];
    }

    [RelayCommand]
    private void NavigateToHomePage()
    {
        CurrentPage = Pages[0];
    }
    
    [RelayCommand]
    private void NavigateToCommunityPage()
    {
        CurrentPage = Pages[1];
    }
    
    [RelayCommand]
    private void NavigateToMinePage()
    {
        CurrentPage = Pages[2];
    }
}