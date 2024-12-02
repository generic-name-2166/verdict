using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;
using Verdict.Services;

namespace Verdict.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        _items =
        [
            new TextItemViewModel(new TextItem("Item 1")),
            new TextItemViewModel(new TextItem("Item 2")),
        ];
        _verdicts = new bool?[_items.Length];
    }

    private readonly TextItemViewModel[] _items;
    private bool?[] _verdicts;

    // Initializes to a default value of 0
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentItem))]
    private uint _currentItemIndex;

    public TextItemViewModel CurrentItem => _items[CurrentItemIndex];

    private void RegisterNextItem()
    {
        if (CurrentItemIndex < _items.Length - 1)
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

    public async Task SaveVerdicts()
    {
        await JsonService.Write(_verdicts);
    }
}
