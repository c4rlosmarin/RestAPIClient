using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace RestAPIClient.ViewModels;

public partial class RequestItem : ObservableRecipient
{
    public string RequestId
    {
        get; set;
    }

    [ObservableProperty]
    public string name;

    [ObservableProperty]
    public string content;

    public bool IsExistingRequest
    {
        get; set;
    }

    public string Tag
    {
        get; set;
    }

    public string method
    {
        get; set;
    }

    public MethodsItemViewModel Method
    {
        get; set;
    }

    public Thickness Margin
    {
        get; set;
    }

    public override string ToString()
    {
        return this.Name;
    }
}