/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Verdict.Services;

namespace Verdict.ViewModels;

public class MenuBarViewModel(Func<Task> loadData, Func<Task> saveVerdicts) : ViewModelBase
{
    // Doing this ugliness because assigning methods from constructor to `Action`
    // properties doesn't count as `Command`s
    public async Task LoadData()
    {
        await loadData();
    }

    public async Task SaveVerdicts()
    {
        await saveVerdicts();
    }

    public async Task AggregateImageFolder()
    {
        // The method really can't be static even though Rider says otherwise
        // To hell with good MVVM practices
        // magical `GetService` is completely unclear
        if (
            Application.Current?.ApplicationLifetime
                is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow?.StorageProvider is not { } provider
        )
            throw new NullReferenceException("Missing StorageProvider instance.");

        await JsonService.AggregateImages(provider);
    }
}
