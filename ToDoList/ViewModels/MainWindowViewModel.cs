using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ToDoList.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new ObservableCollection<ToDoItemViewModel>();
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _newItemContent;

    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    public MainWindowViewModel()
    {
        if (Design.IsDesignMode)
        {
            ToDoItems = new ObservableCollection<ToDoItemViewModel>(new[]
            {
                new ToDoItemViewModel() { Content = "Hello" },
                new ToDoItemViewModel() { Content = "Avalonia", IsChecked = true }
            });
        }
    }

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        ToDoItems.Add(new ToDoItemViewModel() { Content = NewItemContent });
        NewItemContent = null;
    }

    [RelayCommand]
    private void RemoveItem(ToDoItemViewModel item)
    {
        ToDoItems.Remove(item);
    }
}