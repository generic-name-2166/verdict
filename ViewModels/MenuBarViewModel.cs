/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.Threading.Tasks;

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
}
