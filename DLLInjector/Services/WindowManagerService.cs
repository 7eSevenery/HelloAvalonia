using System;
using System.Collections.Generic;
using Avalonia.Controls;
using DLLInjector.Services.Interfaces;
using DLLInjector.ViewModels;
using DLLInjector.Views;

namespace DLLInjector.Services;

public class WindowManagerService : IWindowManagerService
{
    private readonly Dictionary<Type, Type> _allWindowManager = new()
    {
        // 初始化 ViewModel 和 View
        { typeof(ProcessWindowViewModel), typeof(ProcessWindow) },
    };

    private readonly Dictionary<Type, Window> _openingWindowManager = new();

    public void Show<TViewModel>()
    {
        // 获取 ViewModel 的 Type
        Type wt = typeof(TViewModel);
        // 遍历 _AllWindowManager 中的键，获取各个 View 的 Type
        if (_allWindowManager.TryGetValue(wt, out var windowType))
        {
            // 先判断 Window 是否打开
            if (_openingWindowManager.TryGetValue(wt, out var window))
            {
                // 存在则直接 Activate
                window.Activate();
            }
            else
            {
                // 不存在则通过 依赖注入 获取 View
                var view = App.ServiceProvider.GetService(windowType);
                // View 就是 Window
                if (view is Window w)
                {
                    // 追加 Window 的关闭事件
                    w.Closed += (_, _) => { _openingWindowManager.Remove(wt); };

                    // 添加至 _OpeningWindowManager 并 Show
                    _openingWindowManager.Add(wt, w);
                    w.Show();
                }
            }
        }
    }

    public void Hide<TViewModel>()
    {
        // 获取 ViewModel 的 Type
        Type wt = typeof(TViewModel);
        // 判断 wt 是否在 _OpeningWindowManager 中
        if (_openingWindowManager.TryGetValue(wt, out var window))
        {
            // 存在就隐藏
            window.Hide();
        }
    }

    public void Close<TViewModel>()
    {
        // 获取 ViewModel 的 Type
        Type wt = typeof(TViewModel);
        // 判断 wt 是否在 _OpeningWindowManager 中
        if (_openingWindowManager.TryGetValue(wt, out var window))
        {
            // 存在就关闭
            window.Close();
        }
    }
}