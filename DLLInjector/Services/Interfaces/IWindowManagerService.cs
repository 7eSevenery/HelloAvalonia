namespace DLLInjector.Services.Interfaces;

public interface IWindowManagerService
{
    void Show<TViewModel>();
    void Hide<TViewModel>();
    void Close<TViewModel>();
}