using Avalonia.Controls;
using Verdict.ViewModels;

namespace Verdict.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
