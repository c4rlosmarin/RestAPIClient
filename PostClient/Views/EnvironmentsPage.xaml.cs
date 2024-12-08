using Microsoft.UI.Xaml.Controls;

using PostClient.ViewModels;

namespace PostClient.Views;

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
