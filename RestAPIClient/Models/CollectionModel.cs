using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RestAPIClient.Models;
public partial class CollectionModel : ObservableRecipient
{
    [ObservableProperty]
    public string collectionId;
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public ImageIcon icon;
    [ObservableProperty]
    public Thickness margin;

    public override string ToString()
    {
        return this.Description;
    }
}
