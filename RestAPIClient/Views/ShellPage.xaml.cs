using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using RestAPIClient.Contracts.Services;
using RestAPIClient.Helpers;
using RestAPIClient.ViewModels;
using Windows.System;

namespace RestAPIClient.Views;

public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

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

        //var aboutIcon = new FontIcon { Glyph = "\uE946", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        //var aboutFooterItem = new NavigationViewItem() { Content = "About", Icon = aboutIcon };
        //aboutFooterItem.Icon = aboutIcon;

        //NavigationViewControl.FooterMenuItems.Add(aboutFooterItem);
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
                FilterItemsRecursive((ObservableCollection<NavigationViewItem>)NavigationViewControl.MenuItemsSource, query);
            else
                FilterItemsRecursive((ObservableCollection<NavigationViewItem>)NavigationViewControl.MenuItemsSource, "");
        }
    }

    private bool FilterItemsRecursive(ObservableCollection<NavigationViewItem> navigationViewItem, string query)
    {
        bool itemFound = false;
        bool foundSubItem;
        foreach (var item in navigationViewItem)
        {
            foundSubItem = false;
            bool flag = false;

            if (item.MenuItemsSource is not null && ((ObservableCollection<NavigationViewItem>)item.MenuItemsSource).Count > 0)
            {
                foundSubItem = FilterItemsRecursive((ObservableCollection<NavigationViewItem>)item.MenuItemsSource, query);
                if (foundSubItem)
                {
                    item.Visibility = Visibility.Visible;
                    if (!string.IsNullOrEmpty(query))
                    {
                        item.IsExpanded = true;
                        itemFound = true;
                    }
                    else
                        item.IsExpanded = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(query))
                        item.Visibility = Visibility.Collapsed;
                    else
                        item.Visibility = Visibility.Visible;
                    item.IsExpanded = false;
                }
            }
            else
            {
                string content = "";
                if (item.Content is TextBlock)
                    content = ((TextBlock)item.Content).Text;
                else if (item.Content is TextBox)
                    content = ((TextBox)item.Content).Text;

                if (string.IsNullOrEmpty(query) || content.ToLower().Contains(query))
                {
                    item.Visibility = Visibility.Visible;
                    flag = true;
                }
                else
                    item.Visibility = Visibility.Collapsed;
            }
            itemFound = itemFound || flag;
        }
        return itemFound;
    }

    private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        NavigationViewControl.SelectionChanged -= NavigationViewControl_SelectionChanged;

        if (args.SelectedItem is NavigationViewItem selectedItem)
        {
            if (selectedItem.Tag is not null && selectedItem.Tag.ToString() != "Settings")
            {
                var navigationviewItemMetadata = (NavigationViewItemMetadata)selectedItem.Tag;
                if (navigationviewItemMetadata.RequestId is not null)
                {
                    var content = (TextBlock)selectedItem.Content;
                    if (NavigationFrame.Content is HomePage homePage)
                    {
                        homePage.CreateRequestTab(new RequestItem() { RequestId = navigationviewItemMetadata.RequestId, Name = content.Text, Method = navigationviewItemMetadata.Method });
                    }
                    else
                        NavigationFrame.Navigate(typeof(HomePage), (new RequestItem() { RequestId = navigationviewItemMetadata.RequestId, Name = content.Text, Method = navigationviewItemMetadata.Method, AzureService = navigationviewItemMetadata.AzureService }));
                }
            }
            else
            {
                switch (selectedItem.Content)
                {
                    case "Environments":
                        NavigationFrame.Navigate(typeof(EnvironmentsPage));
                        break;
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
