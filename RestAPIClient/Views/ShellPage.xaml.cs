using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using RestAPIClient.Contracts.Services;
using RestAPIClient.Helpers;
using RestAPIClient.ViewModels;
using Windows.System;

namespace RestAPIClient.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    List<NavigationViewItem> originalNavigationViewItems;

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);


        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        //App.MainWindow.Activated += MainWindow_Activated;
        //AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        originalNavigationViewItems = new List<NavigationViewItem>();
        foreach (var item in nviCollections.MenuItems)
        {
            if (item is NavigationViewItem navItem)
                originalNavigationViewItems.Add(navItem);
        }
    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    //private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    //{
    //    App.AppTitlebar = AppTitleBarText as UIElement;
    //}

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var query = sender.Text.ToLower();
            if (!string.IsNullOrEmpty(query))
            {
                var filteredItems = FilterItems(originalNavigationViewItems, query);
                nviCollections.MenuItems.Clear();
                nviCollections.MenuItemsSource = null;
                nviCollections.MenuItemsSource = filteredItems;
            }
            else
            {
                var filteredItems = FilterItems(originalNavigationViewItems, "");
                nviCollections.MenuItems.Clear();
                nviCollections.MenuItemsSource = null;
                nviCollections.MenuItemsSource = filteredItems;
            }
        }
    }

    private List<NavigationViewItem> FilterItems(IEnumerable<NavigationViewItem> items, string query)
    {
        var filteredItems = new List<NavigationViewItem>();
        foreach (var item in items)
        {
            if (item.Content.ToString().ToLower().Contains(query))
                filteredItems.Add(CloneNavigationViewItem(item));
            else if (item.MenuItems.Count > 0)
            {
                var filteredSubItems = FilterItems(item.MenuItems.OfType<NavigationViewItem>(), query);
                if (filteredSubItems.Count > 0)
                {
                    var newItem = CloneNavigationViewItem(item);
                    newItem.MenuItems.Clear();
                    foreach (var subItem in filteredSubItems)
                        newItem.MenuItems.Add(subItem);
                    filteredItems.Add(newItem);
                }
            }
        }

        return filteredItems;
    }

    private NavigationViewItem CloneNavigationViewItem(NavigationViewItem item)
    {
        var newItem = new NavigationViewItem
        {
            Content = item.Content,
            Icon = item.Icon,
            IsExpanded = true
        };

        foreach (var subItem in item.MenuItems.OfType<NavigationViewItem>())
            newItem.MenuItems.Add(CloneNavigationViewItem(subItem));

        return newItem;
    }

}
