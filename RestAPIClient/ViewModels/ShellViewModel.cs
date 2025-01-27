using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using RestAPIClient.Contracts.Services;
using RestAPIClient.Views;

namespace RestAPIClient.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    ObservableCollection<NavigationMenuItem> navigationMenuItems;

    [ObservableProperty]
    ObservableCollection<NavigationMenuItem> navigationFooterItems;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    private void InitializeServices()
    {
        NavigationMenuItems = new ObservableCollection<NavigationMenuItem>();

        var collectionsIcon = new FontIcon { Glyph = "\uE8A4", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        var collectionsMenuItem = new NavigationMenuItem() { Content = "Collections", FontIcon = collectionsIcon, SubMenus = new ObservableCollection<NavigationMenuItem>() };
        var azureTemplatesMenuItem = new NavigationMenuItem() { Content = "Azure Templates", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/Azure.png")) }, Margin=new(-20,0,0,0)};
        collectionsMenuItem.SubMenus.Add(azureTemplatesMenuItem);

        var navigationMenuItem = new NavigationMenuItem() { Content = "Storage Services", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Accounts.png")) }, Margin = new(-20, 0, 0, 0) };

        azureTemplatesMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        azureTemplatesMenuItem.SubMenus.Add(navigationMenuItem);

        navigationMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        var subMenuItem = new NavigationMenuItem() { Content = "Blob Service REST API", Margin = new(-20, 0, 0, 0) };
        navigationMenuItem.SubMenus.Add(subMenuItem);

        subMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        subMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "975b53b7-f48f-4682-8434-893f5a324278",Name = "List Containers",Content = "List Containers", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "e192945c-7d70-49c6-8e50-521a2f7f01c2",Name = "Set Blob Service Properties", Content = "Set Blob Service Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94",Name = "Get Blob Service Properties", Content = "Get Blob Service Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "e4e5c062-82a1-4282-a571-19d5acdec6d3",Name = "Preflight Blob Request", Content = "Preflight Blob Request", Method = new MethodsItemViewModel("OPTIONS"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17",Name = "Get Blob Service Stats", Content = "Get Blob Service Stats", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "30e6c366-35d0-47e5-9a5c-def2ee2cfe31",Name = "Get Account Information", Content = "Get Account Information", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "53d9941c-dcc4-48b8-a717-24064108ecda",Name = "Get User Delegation Key", Content = "Get User Delegation Key", Method = new MethodsItemViewModel("POST"), Margin = new(-20, 0, 0, 0), IsRequest=true }
        ];

        NavigationMenuItems.Add(collectionsMenuItem);

        NavigationFooterItems = new ObservableCollection<NavigationMenuItem>();
        var aboutIcon = new FontIcon { Glyph = "\uE946", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        var aboutFooterItem = new NavigationMenuItem() { Content = "About", FontIcon = aboutIcon };
        NavigationFooterItems.Add(aboutFooterItem);
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;

        InitializeServices();
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
