using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using mywinui3app.ViewModels;

namespace mywinui3app.Views;

public sealed partial class CollectionsPage : Page
{

    #region << Properties >>

    public CollectionsViewModel ViewModel
    {
        get;
    }

    #endregion

    #region << Constructor >>

    public CollectionsPage()
    {
        ViewModel = App.GetService<CollectionsViewModel>();
        InitializeComponent();
    }

    #endregion

    #region << Methods >>

    private void CreateRequestTab(RequestItem? request)
    {
        //TODO: Implementar el estilo y texto del tabViewItem de forma dinámica

        TabViewItem newItem = new TabViewItem();
        Frame frame = new Frame();

        if (request == null)
        {
            newItem.Header = "Untitled request";
            frame.Navigate(typeof(RequestPage));
            newItem.HeaderTemplate = newTabViewItemHeaderTemplate;
        }
        else
        {
            foreach (TabViewItem item in tabView.TabItems)
            {
                if (item.Header == request.Name)
                {
                    item.IsSelected = true;
                    return;
                }
            }

            newItem.Header = request.Name;
            frame.Navigate(typeof(RequestPage), request);
        }

        newItem.Content = frame;
        newItem.IsSelected = true;
        
        tabView.TabItems.Add(newItem);
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
        sender.TabItems.Remove(args.Tab);
    }

    #endregion

    private void treeCollections_SelectionChanged(TreeView sender, TreeViewSelectionChangedEventArgs args)
    {
        var selectedNode = (sender as TreeView).SelectedNode.Content;
        if (selectedNode is RequestItem)
        {
            var request = (RequestItem)selectedNode;
            CreateRequestTab(request);

        }
    }
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

#endregion
