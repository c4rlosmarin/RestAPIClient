using Microsoft.UI.Xaml.Controls;
using RestAPIClient.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RestAPIClient.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AboutPage : Page
{
    public AboutViewModel ViewModel
    {
        get;
    }
    public AboutPage()
    {
        ViewModel = App.GetService<AboutViewModel>();
        this.InitializeComponent();
    }
}
