using CommunityToolkit.Mvvm.ComponentModel;

namespace RestAPIClient.ViewModels;

public partial class RequestItem : ObservableRecipient
{
    public string RequestId { get; set; }

    [ObservableProperty]
    public string? name;

    [ObservableProperty]
    public string? content;

    public AzureRESTApi AzureRESTApi { get; set; }

    public string method { get; set; }

    public MethodsItemViewModel Method { get; set; }

}