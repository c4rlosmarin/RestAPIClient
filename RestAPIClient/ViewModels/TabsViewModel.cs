using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RestAPIClient.ViewModels;

public partial class TabsViewModel : ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<TabItem> tabs;
    [ObservableProperty]
    public TabItem selectedTabItem;

    public TabsViewModel()
    {
        tabs = new ObservableCollection<TabItem>();
    }
}
