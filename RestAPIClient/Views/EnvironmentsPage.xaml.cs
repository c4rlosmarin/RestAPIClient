using Microsoft.UI.Xaml.Controls;

using RestAPIClient.ViewModels;

namespace RestAPIClient.Views;

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
