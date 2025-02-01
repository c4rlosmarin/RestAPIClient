using Microsoft.UI.Xaml.Controls;

namespace RestAPIClient.ViewModels;
public partial class NavigationViewItemMetadata
{
    public string RequestId
    {
        get; set;
    }

    public string Method
    {
        get; set;
    }


    public AzureService AzureService
    {
        get; set;
    }

}

public enum AzureService
{
    StorageServices
}