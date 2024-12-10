using CommunityToolkit.Mvvm.ComponentModel;

namespace PostClient.ViewModels;

public partial class RequestItem : ObservableRecipient
{
    public string RequestId
    {
        get; set;
    }

    [ObservableProperty]
    public string name;

    public bool IsExistingRequest
    {
        get; set;
    }
    
    public MethodsItemViewModel Method
    {
        get; set;
    }
}