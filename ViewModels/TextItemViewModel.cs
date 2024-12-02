/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;

namespace Verdict.ViewModels;

public partial class TextItemViewModel(TextItem textItem) : ViewModelBase
{
    [ObservableProperty]
    private string _text = textItem.Text;
}
