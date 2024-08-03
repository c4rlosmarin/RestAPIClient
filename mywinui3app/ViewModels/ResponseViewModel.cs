using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class ResponseViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string body;
    [ObservableProperty]
    public ObservableCollection<ResponseData> headers;
    [ObservableProperty]
    public string headersCount;

}
public partial class ResponseData: ObservableRecipient
{
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
}
