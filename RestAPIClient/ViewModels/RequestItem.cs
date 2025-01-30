using CommunityToolkit.Mvvm.ComponentModel;

namespace RestAPIClient.ViewModels;

public partial class RequestItem : ObservableRecipient
{
    public string RequestId { get; set; }

    [ObservableProperty]
    public string? name;

    [ObservableProperty]
    public string? content;

    public string Method { get; set; }

    public AzureService AzureService { get; set; }

}