using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;

namespace Verdict.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<TextItemViewModel> _items =
    [
        new TextItemViewModel(new TextItem("Item")),
    ];
}
