using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;

namespace Postclient.ViewModels;

public partial class TabItem : ObservableRecipient
{
    public string Id;
    [ObservableProperty]
    public string title;
    [ObservableProperty]
    public string editingIconVisibility;
    [ObservableProperty]
    public SolidColorBrush foreground;
    [ObservableProperty]
    public string method;

}
