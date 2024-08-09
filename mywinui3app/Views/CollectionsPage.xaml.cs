using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using mywinui3app.ViewModels;
using mywinui3app.Helpers;

namespace mywinui3app.Views;

public sealed partial class CollectionsPage : Page
{

    #region << Properties >>

    public CollectionsViewModel ViewModel
    {
        get;
    }

    public TabsViewModel TabsViewModel
    {
        get;
    }

    #endregion

    #region << Constructor >>

    public CollectionsPage()
    {
        ViewModel = App.GetService<CollectionsViewModel>();
        TabsViewModel = App.GetService<TabsViewModel>();
        InitializeComponent();
    }

    #endregion

    #region << Methods >>

    private void CreateRequestTab(RequestViewModel? request)
    {
        //TODO: Implementar el estilo y texto del tabViewItem de forma dinámica
        Frame frame = new Frame();

        if (request == null)
        {
            TabsViewModel.Tabs.Add(new TabItem() { Title = "Untitled request", EditingIconVisibility = "Visible", Method = "GET", Foreground = ColorHelper.CreateSolidColorBrushFromHex(MethodForegroundColor.GET) });
            frame.Navigate(typeof(RequestPage));
        }
        else
        {
            foreach (TabItem item in tabView.TabItems)
            {
                if (item.Title == request.Name)
                {
                    TabViewItem existingItem = tabView.ContainerFromItem(item) as TabViewItem;
                    existingItem.IsSelected = true;
                    return;
                }
            }

            TabsViewModel.Tabs.Add(new TabItem() { Title = request.Name, EditingIconVisibility = "Collapsed", Method = request.SelectedMethod.Name, Foreground = ColorHelper.CreateSolidColorBrushFromHex(MethodForegroundColor.GetColorByMethod(request.SelectedMethod.Name)) });
            frame.Navigate(typeof(RequestPage), request);
        }

        tabView.SelectedIndex = TabsViewModel.Tabs.Count - 1;
        tabView.UpdateLayout();

        TabViewItem newItem = tabView.ContainerFromItem(tabView.SelectedItem) as TabViewItem;
        newItem.Content = frame;
        newItem.IsSelected = true;

        var originalSelectedItem = tabView.SelectedItem;
        tabView.SelectedItem = null;
        tabView.SelectedItem = originalSelectedItem;
    }

    #endregion

    #region << Events >>

    //private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    //{
    //    var naViewItemInvoked = (NavigationViewItem)args.InvokedItemContainer;

    //    if (args.InvokedItemContainer is not null)
    //    {
    //        var navItemTag = args.InvokedItemContainer.Tag?.ToString();
    //        if (!string.IsNullOrEmpty(navItemTag))
    //        {
    //            NavView_Navigate(navItemTag, new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
    //        }
    //    }
    //}

    //private void NavView_Navigate(string navItemTag, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
    //{
    //    Type page;

    //    MenuOption item = menuOptions.First(p => p.ClassName.Equals(navItemTag));
    //    page = Type.GetType(item.ClassName);

    //    // Get the page type before navigation so you can prevent duplicate
    //    // entries in the backstack.
    //    var preNavPageType = ContentFrame.CurrentSourcePageType;

    //    // Only navigate if the selected page isn't currently loaded.
    //    if ((page is not null) && !Type.Equals(preNavPageType, page))
    //    {
    //        ContentFrame.Navigate(page, null, transitionInfo);
    //    }
    //}

    private void tabView_AddTabButtonClick(TabView sender, object args)
    {
        CreateRequestTab(null);
    }

    private void tabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        TabsViewModel.Tabs.Remove(TabsViewModel.SelectedTabItem);
    }

    private void treeCollections_SelectionChanged(TreeView sender, TreeViewSelectionChangedEventArgs args)
    {
        var selectedNode = (sender as TreeView).SelectedNode.Content;
        if (selectedNode is RequestViewModel)
        {
            var request = (RequestViewModel)selectedNode;
            CreateRequestTab(request);

        }
    }

    private void tabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tabView = sender as TabView;

    }

    #endregion

}

#region << Internal Classes >>

internal class MenuItemDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CollectionTemplate
    {
        get; set;
    }
    public DataTemplate? RequestTemplate
    {
        get; set;
    }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        return item switch
        {
            CollectionItem => CollectionTemplate,
            RequestViewModel => RequestTemplate,
            _ => null,
        };
    }
}

internal class ExplorerItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CollectionTemplate
    {
        get; set;
    }
    public DataTemplate? RequestTemplate
    {
        get; set;
    }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        return item switch
        {
            CollectionItem => CollectionTemplate,
            RequestViewModel => RequestTemplate,
            _ => null,
        };
    }
}

#endregion
