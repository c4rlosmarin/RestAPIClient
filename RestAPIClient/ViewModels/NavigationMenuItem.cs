using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace RestAPIClient.ViewModels;
public partial class NavigationMenuItem : ObservableRecipient
{
    [ObservableProperty]
    public string name;
	[ObservableProperty]
	public string content;
	[ObservableProperty]
    public string description;
    [ObservableProperty]
    public ImageIcon imageIcon;
    [ObservableProperty]
    public FontIcon fontIcon;
    [ObservableProperty]
    public bool isExpanded;
    [ObservableProperty]
    public string visibility;
    [ObservableProperty]
    public Thickness margin = new(0,0,0,0);
	[ObservableProperty]
	public ObservableCollection<NavigationMenuItem> subMenus;

	public string RequestId
	{
		get; set;
	}

	public bool IsRequest
	{
		get; set;
	}

	public MethodsItemViewModel Method
	{
		get; set;
	}

}
