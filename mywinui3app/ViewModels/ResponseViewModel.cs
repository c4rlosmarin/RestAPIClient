using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class ResponseViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string statusCode;
    [ObservableProperty]
    public string statusStyleKey = "MyTextBlockAccentStyle";
    [ObservableProperty]
    public string time;
    [ObservableProperty]
    public string size;
    [ObservableProperty]
    public string body;
    [ObservableProperty]
    public ObservableCollection<ResponseHeaderItem> headers;
    [ObservableProperty]
    public string headersCount;
    [ObservableProperty]
    public string visibility = "Collapsed";
    [ObservableProperty]
    public string bannerVisibility = "Visible";

}
public partial class ResponseHeaderItem: ObservableRecipient
{
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
}
