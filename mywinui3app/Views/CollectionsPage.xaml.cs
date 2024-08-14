using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using mywinui3app.Helpers;
using mywinui3app.ViewModels;

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

    private void CreateRequestTab(RequestItem? request)
    {
        var foregroundColorHelper = new MethodForegroundColor();
        //TODO: Implementar el estilo y texto del tabViewItem de forma dinámica
        Frame frame = new Frame();

        if (request == null)
        {
            TabsViewModel.Tabs.Add(new TabItem() { Title = "Untitled request", EditingIconVisibility = "Visible", Method = "GET", Foreground = ColorHelper.CreateSolidColorBrushFromHex(foregroundColorHelper.GET) });
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

            TabsViewModel.Tabs.Add(new TabItem() { Title = request.Name, EditingIconVisibility = "Collapsed", Method = request.SelectedMethod.Name, Foreground = ColorHelper.CreateSolidColorBrushFromHex(foregroundColorHelper.GetColorByMethod(request.SelectedMethod.Name)) });
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

    private void RefreshSelectedCollection()
    {
        if (tabView.SelectedItem != null)
        {
            var node = FindTreeViewItemByName(treeCollections, ((TabItem)tabView.SelectedItem).Title);

            if (node is not null)
                treeCollections.SelectedItem = node.DataContext;
            else
                treeCollections.SelectedItem = null;
        }
        else if (treeCollections.SelectedNode != null)
        {
            if (treeCollections.SelectedNode.Parent != null)
                treeCollections.SelectedNode = treeCollections.SelectedNode.Parent;
        }
    }

    private TreeViewItem FindTreeViewItemByName(TreeView treeView, string name)
    {
        foreach (var rootNode in treeView.RootNodes)
        {
            var result = FindTreeViewItemByNameRecursive((TreeViewItem)treeView.ContainerFromNode(rootNode), name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private TreeViewItem FindTreeViewItemByNameRecursive(TreeViewItem treeViewItem, string name)
    {
        if (treeViewItem == null) return null;

        var viewModel = treeViewItem.DataContext;

        switch (viewModel)
        {
            case CollectionItem:
                if (viewModel != null && ((CollectionItem)viewModel).Name == name)
                    return treeViewItem;
                break;
            case ViewModels.GroupItem:
                if (viewModel != null && ((ViewModels.GroupItem)viewModel).Name == name)
                    return treeViewItem;
                break;
            case RequestItem:
                if (viewModel != null && ((ViewModels.RequestItem)viewModel).Name == name)
                    return treeViewItem;
                break;
        }

        var node = treeCollections.NodeFromContainer(treeViewItem);
        foreach (var childNode in node.Children)
        {
            var childTreeViewItem = treeCollections.ContainerFromNode(childNode) as TreeViewItem;
            var result = FindTreeViewItemByNameRecursive(childTreeViewItem, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
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
        TabsViewModel.Tabs.Remove((TabItem)args.Item);
    }
    private void tabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshSelectedCollection();
    }

    private void treeCollections_SelectionChanged(TreeView sender, TreeViewSelectionChangedEventArgs args)
    {
        if (treeCollections.SelectedNode is not null)
        {
            var selectedTreeviewItem = treeCollections.SelectedNode.Content;
            if (selectedTreeviewItem is not null)
            {
                if (selectedTreeviewItem is RequestItem)
                {
                    var request = (RequestItem)selectedTreeviewItem;
                    CreateRequestTab(request);
                }
            }
        }
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
            RequestItem => RequestTemplate,
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

    public DataTemplate? GroupTemplate
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
            ViewModels.GroupItem => GroupTemplate,
            RequestItem => RequestTemplate,
            _ => null,
        };
    }
}

#endregion
