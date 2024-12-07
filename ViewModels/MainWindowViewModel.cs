/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;
using Verdict.Services;

namespace Verdict.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        (_items, _verdicts) = SetItems([new TextItem("Item 1"), new TextItem("Item 2")]);
        MenuBarVm = new MenuBarViewModel(LoadData, SaveVerdicts);
    }

    private List<IItemModelViewModel> _items;
    private bool?[] _verdicts;

    // Initializes to a default value of 0
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentItemModel))]
    private int _currentItemIndex;

    private static IItemModelViewModel? ConvertModels(ItemModel item)
    {
        return item switch
        {
            TextItem textItem => new TextItemViewModel(textItem),
            ImageItem imageItem => new ImageItemViewModel(imageItem),
            _ => null,
        };
    }

    private static (List<IItemModelViewModel>, bool?[]) SetItems(List<ItemModel> items)
    {
        var viewModels = items.ConvertAll(ConvertModels).OfType<IItemModelViewModel>().ToList();
        return (viewModels, new bool?[viewModels.Count]);
    }

    public IItemModelViewModel CurrentItemModel => _items[CurrentItemIndex];

    private void RegisterNextItem()
    {
        if (CurrentItemIndex < _items.Count - 1)
            CurrentItemIndex++;
    }

    public void RegisterYesItem()
    {
        _verdicts[CurrentItemIndex] = true;
        RegisterNextItem();
    }

    public void RegisterNoItem()
    {
        _verdicts[CurrentItemIndex] = false;
        RegisterNextItem();
    }

    public void RegisterPreviousItem()
    {
        if (CurrentItemIndex > 0)
            CurrentItemIndex--;
    }

    private async Task SaveVerdicts()
    {
        // To hell with good MVVM practices
        // magical `GetService` is completely unclear
        if (
            Application.Current?.ApplicationLifetime
                is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow?.StorageProvider is not { } provider
        )
            throw new NullReferenceException("Missing StorageProvider instance.");

        await JsonService.Write(provider, _verdicts);
    }

    private async Task LoadData()
    {
        // To hell with good MVVM practices
        // magical `GetService` is completely unclear
        if (
            Application.Current?.ApplicationLifetime
                is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow?.StorageProvider is not { } provider
        )
            throw new NullReferenceException("Missing StorageProvider instance.");

        var data = await JsonService.Read(provider);
        (_items, _verdicts) = SetItems(data);
        CurrentItemIndex = 0;
    }

    public MenuBarViewModel MenuBarVm { get; set; }
}
