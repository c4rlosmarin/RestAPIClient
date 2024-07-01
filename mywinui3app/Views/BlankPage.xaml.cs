using Microsoft.UI.Xaml.Controls;

using mywinui3app.ViewModels;

namespace mywinui3app.Views;

public sealed partial class BlankPage : Page
{
    public BlankViewModel ViewModel
    {
        get;
    }

    public BlankPage()
    {
        ViewModel = App.GetService<BlankViewModel>();
        InitializeComponent();
    }
}
