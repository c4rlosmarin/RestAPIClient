using Microsoft.UI.Xaml.Controls;

using mywinui3app.ViewModels;

namespace mywinui3app.Views;

public sealed partial class EnvironmentsPage : Page
{
    public EnvironmentsViewModel ViewModel
    {
        get;
    }

    public EnvironmentsPage()
    {
        ViewModel = App.GetService<EnvironmentsViewModel>();
        InitializeComponent();
    }
}
