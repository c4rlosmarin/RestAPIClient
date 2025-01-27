using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using RestAPIClient.Contracts.Services;
using RestAPIClient.Helpers;
using RestAPIClient.ViewModels;
using Windows.System;

namespace RestAPIClient.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel { get; }

    #region << Constructor >>

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
    }

    #endregion

    #region << Events >>

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
                FilterItemsRecursive((IEnumerable<NavigationMenuItem>)NavigationViewControl.MenuItemsSource, query);
            else
                FilterItemsRecursive((IEnumerable<NavigationMenuItem>)NavigationViewControl.MenuItemsSource, "");
        }
    }

    private bool FilterItemsRecursive(IEnumerable<NavigationMenuItem> items, string query)
    {
        string content;
        bool itemFound = false;
        foreach (var item in items)
        {
            bool foundSubItem = false;
            if (item.SubMenus is not null && item.SubMenus.Count > 0)
            {
                itemFound = FilterItemsRecursive(item.SubMenus, query);
                if (itemFound)
                {
                    item.Visibility = "Visible";
                    if (!string.IsNullOrEmpty(query))
                        item.IsExpanded = true;
                    else
                        item.IsExpanded = false;
                }
                else
                {
                    item.Visibility = "Collapsed";
                    item.IsExpanded = false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(query) || item.Content.ToLower().Contains(query))
                {
                    item.Visibility = "Visible";
                    itemFound = true;
                }
                else
                    item.Visibility = "Collapsed";
            }
        }
        return itemFound;
    }

    private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        NavigationViewControl.SelectionChanged -= NavigationViewControl_SelectionChanged;

        if (args.SelectedItem is NavigationMenuItem selectedItem)
        {
            if (selectedItem.IsRequest)
            {
                if (NavigationFrame.Content is HomePage homePage)
                {
                    if (selectedItem.IsRequest)
                        homePage.CreateRequestTab(new RequestItem() { RequestId = selectedItem.RequestId, Name = selectedItem.Content, method = selectedItem.Method.Name });
                }
                else
                    NavigationFrame.Navigate(typeof(HomePage), (new RequestItem() { RequestId = selectedItem.RequestId, Name = selectedItem.Content, method = selectedItem.Method.Name }));
            }
            else
            {
                switch (selectedItem.Content)
                {
                    case "About":
                        NavigationFrame.Navigate(typeof(AboutPage));

                        break;
                }
            }
        }
        NavigationViewControl.SelectionChanged += NavigationViewControl_SelectionChanged;
    }

    #endregion
}

#region << Internal Classes >>

public class IconTemplateSelector : DataTemplateSelector
{
    public DataTemplate ImageIconTemplate
    {
        get; set;
    }
    public DataTemplate FontIconTemplate
    {
        get; set;
    }

    public DataTemplate NoIconTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        var viewModel = item as NavigationMenuItem;
        if (viewModel != null)
        {
            if (viewModel.ImageIcon is not null)
                return ImageIconTemplate;
            else if (viewModel.FontIcon is not null)
                return FontIconTemplate;
            else
                return NoIconTemplate;
        }
        return base.SelectTemplateCore(item, container);
    }
}

#endregion