using Microsoft.UI.Xaml.Controls;

using mywinui3app.ViewModels;

namespace mywinui3app.Views;

public sealed partial class CollectionsPage : Page
{
    public CollectionsViewModel ViewModel
    {
        get;
    }

    public CollectionsPage()
    {
        ViewModel = App.GetService<CollectionsViewModel>();
        InitializeComponent();
    }
}
