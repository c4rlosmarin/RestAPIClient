using Microsoft.UI.Xaml.Controls;

using Postclient.ViewModels;

namespace Postclient.Views;

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
