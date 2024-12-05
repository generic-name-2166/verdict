/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: CC0-1.0
*/

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Verdict.ViewModels;

namespace Verdict;

public class ViewLocator : IDataTemplate
{
    private static Dictionary<Type, Func<Control>> _registration = new();

    public static void Register<TViewModel, TView>()
        where TView : Control, new()
    {
        _registration.Add(typeof(TViewModel), () => new TView());
    }

    public static void Register<TViewModel>(Func<Control> factory)
    {
        _registration.Add(typeof(TViewModel), factory);
    }

    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var type = param.GetType();

        return _registration.TryGetValue(type, out var factory)
            ? factory()
            : new TextBlock { Text = "Not Found: " + type.Name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
