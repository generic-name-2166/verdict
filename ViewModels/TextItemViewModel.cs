using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;

namespace Verdict.ViewModels;

public partial class TextItemViewModel(TextItem textItem) : ViewModelBase
{
    [ObservableProperty]
    private string _text = textItem.Text;
}
