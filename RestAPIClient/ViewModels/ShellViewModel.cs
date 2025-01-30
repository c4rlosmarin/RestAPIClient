using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
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
        var collectionsMenuItem = new NavigationMenuItem() { Content = "Collections", FontIcon = collectionsIcon, SubMenus = new ObservableCollection<NavigationMenuItem>(), Margin = new(0, 0, 0, 0) };
        collectionsMenuItem.IsRequest = false;

        var azureTemplatesMenuItem = new NavigationMenuItem() { Content = "Azure Templates", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/Azure.png")) }, IsRequest = false };
        azureTemplatesMenuItem.IsRequest = false;

        var resourcePath = "RestAPIClient.Templates.Shell.NavigationMenuItems.StorageServices.json";
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using StreamReader reader = new StreamReader(stream);
        var jsonString = reader.ReadToEnd();

        NavigationMenuItem deserializedNavigationMenuItems;
        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            JsonElement root = doc.RootElement;
            deserializedNavigationMenuItems = DeserializeNavigationMenuItems(root);
        }

        azureTemplatesMenuItem.SubMenus = deserializedNavigationMenuItems.subMenus;

        collectionsMenuItem.SubMenus.Add(azureTemplatesMenuItem);
        NavigationMenuItems.Add(collectionsMenuItem);

        NavigationFooterItems = new ObservableCollection<NavigationMenuItem>();
        var aboutIcon = new FontIcon { Glyph = "\uE946", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        var aboutFooterItem = new NavigationMenuItem() { Content = "About", FontIcon = aboutIcon, Margin = new(0, 0, 0, 0), IsRequest = false };
        NavigationFooterItems.Add(aboutFooterItem);
    }

    public NavigationMenuItem DeserializeNavigationMenuItems(JsonElement element)
    {
        var menuItem = new NavigationMenuItem();

        foreach (JsonProperty jsonProperty in element.EnumerateObject())
        {
            switch (jsonProperty.Name)
            {
                case "RequestId":
                    menuItem.RequestId = jsonProperty.Value.ToString();
                    break;
                case "Name":
                    menuItem.Name = jsonProperty.Value.ToString();
                    break;
                case "Content":
                    menuItem.Content = jsonProperty.Value.ToString();
                    break;
                case "Method":
                    menuItem.Method = jsonProperty.Value.ToString();
                    break;
                case "ImageIcon":
                    menuItem.ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri(jsonProperty.Value.ToString())) };
                    break;
                case "Margin":
                    menuItem.Margin = JsonSerializer.Deserialize<Microsoft.UI.Xaml.Thickness>(jsonProperty.Value.GetRawText());
                    break;
                case "IsRequest":
                    menuItem.IsRequest = JsonSerializer.Deserialize<bool>(jsonProperty.Value.GetRawText());
                    break;
                default:
                    if (menuItem.SubMenus is null)
                        menuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();

                    if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var subElement in jsonProperty.Value.EnumerateArray())
                            menuItem.SubMenus.Add(DeserializeNavigationMenuItems(subElement));
                    }
                    else if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
                        menuItem.SubMenus.Add(DeserializeNavigationMenuItems(jsonProperty.Value));
                    break;
            }
        }
        return menuItem;
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
