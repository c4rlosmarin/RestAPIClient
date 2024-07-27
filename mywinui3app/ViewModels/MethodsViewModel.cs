using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class MethodsItemViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string foreground;
}