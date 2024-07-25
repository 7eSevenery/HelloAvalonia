using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.Models;

namespace ToDoList.ViewModels;

public partial class ToDoItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isChecked;

    [ObservableProperty]
    private string? _content;

    public ToDoItemViewModel()
    {
    }

    public ToDoItemViewModel(ToDoItem item)
    {
        item.IsChecked = _isChecked;
        item.Content = _content;
    }
}