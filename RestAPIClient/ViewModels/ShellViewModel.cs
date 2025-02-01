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
    ObservableCollection<NavigationViewItem> navigationMenuItems;

    [ObservableProperty]
    ObservableCollection<NavigationViewItem> navigationFooterItems;

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
        NavigationMenuItems = new ObservableCollection<NavigationViewItem>();

        var collectionsMenuItem = new NavigationViewItem() { Content = "Collections", Icon = new FontIcon { Glyph = "\uE8A4", FontFamily = new FontFamily("Segoe MDL2 Assets")}, Margin = new(0, 0, 0, 0)};
        NavigationMenuItems.Add(collectionsMenuItem);

        var environmentsMenuItem = new NavigationViewItem() { Content = "Environments", Icon = new FontIcon { Glyph = "\uF259", FontFamily = new FontFamily("Segoe MDL2 Assets")}, Margin = new(0, 0, 0, 0)};
        NavigationMenuItems.Add(environmentsMenuItem);

        var azureTemplatesMenuItem = new NavigationViewItem() { Content = "Azure Templates", Icon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/Azure.png"))}};

        var resourcePath = "RestAPIClient.Templates.Shell.NavigationMenuItems.StorageServices.json";
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using StreamReader reader = new StreamReader(stream);
        var jsonString = reader.ReadToEnd();

        NavigationViewItem deserializedNavigationMenuItems;
        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            JsonElement root = doc.RootElement;
            deserializedNavigationMenuItems = DeserializeNavigationMenuItems(root);
        }

        azureTemplatesMenuItem.MenuItemsSource = deserializedNavigationMenuItems.MenuItemsSource;
        NavigationMenuItems.Add(azureTemplatesMenuItem);

        NavigationFooterItems = new ObservableCollection<NavigationViewItem>();
        var aboutIcon = new FontIcon { Glyph = "\uE946", FontFamily = new FontFamily("Segoe MDL2 Assets")};
        var aboutFooterItem = new NavigationViewItem() { Content = "About", Icon = aboutIcon, Margin = new(0, 0, 0, 0)};
        NavigationFooterItems.Add(aboutFooterItem);
    }

    public NavigationViewItem DeserializeNavigationMenuItems(JsonElement element)
    {
        var navigationViewItem = new NavigationViewItem();
        var navigationViewItemMetadata = new NavigationViewItemMetadata();

        foreach (JsonProperty jsonProperty in element.EnumerateObject())
        {
            switch (jsonProperty.Name)
            {
                case "RequestId":
                    navigationViewItemMetadata.RequestId = jsonProperty.Value.ToString();
                    break;
                case "Name":
                    navigationViewItem.Name = jsonProperty.Value.ToString();
                    break;
                case "Content":
                    var textBlock = new TextBlock();
                    textBlock.TextTrimming = Microsoft.UI.Xaml.TextTrimming.CharacterEllipsis;
                    textBlock.Text = jsonProperty.Value.ToString();
                    navigationViewItem.Content = textBlock;

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = textBlock.Text;
                    ToolTipService.SetToolTip(textBlock, toolTip);
                    break;
                case "Method":
                    navigationViewItemMetadata.Method = jsonProperty.Value.ToString();
                    break;
                case "ImageIcon":
                    navigationViewItem.Icon = new ImageIcon { Source = new BitmapImage(new Uri(jsonProperty.Value.ToString())) };
                    break;
                case "Margin":
                    navigationViewItem.Margin = JsonSerializer.Deserialize<Microsoft.UI.Xaml.Thickness>(jsonProperty.Value.GetRawText());
                    break;
                default:
                    if (navigationViewItem.MenuItemsSource is null)
                        navigationViewItem.MenuItemsSource = new ObservableCollection<NavigationViewItem>();

                    if (jsonProperty.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var subElement in jsonProperty.Value.EnumerateArray())
                        {
                            ((ObservableCollection<NavigationViewItem>)navigationViewItem.MenuItemsSource).Add(DeserializeNavigationMenuItems(subElement));
                        }
                    }
                    else if (jsonProperty.Value.ValueKind == JsonValueKind.Object)
                        ((ObservableCollection<NavigationViewItem>)navigationViewItem.MenuItemsSource).Add(DeserializeNavigationMenuItems(jsonProperty.Value));
                    break;
            }

            navigationViewItem.Margin = new(-20, 0, 0, 0);
        }
        navigationViewItem.Tag = navigationViewItemMetadata;
        return navigationViewItem;
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
