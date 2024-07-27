using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class ResponseViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string body;

    [ObservableProperty]
    public ObservableCollection<ResponseData> headers;

}
public class ResponseData
{
    public string Key
    {
        get; set;
    }
    public string Value
    {
        get; set;
    }
}
