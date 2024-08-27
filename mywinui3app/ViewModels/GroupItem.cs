using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class GroupItem : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public ObservableCollection<RequestItem> requests;
    [ObservableProperty]
    public ObservableCollection<string> requestsList;
}
