using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using mywinui3app.ViewModels;

namespace mywinui3app.Views;

public sealed partial class CollectionsPage : Page
{
    #region << Variables >>

    ObservableCollection<Collection> Collections = new ObservableCollection<Collection>();

    #endregion

    public CollectionsViewModel ViewModel
    {
        get;
    }

    public CollectionsPage()
    {
        ViewModel = App.GetService<CollectionsViewModel>();
        InitializeComponent();
        this.InitializeCollections();
    }

    #region << Methods >>

    private void InitializeCollections()
    {
        var collection = new Collection();
        collection.Title = "Azure Entra ID";

        var request = new Request() { Title = "Get Azure AD Token" };
        collection.Requests.Add(request);

        request = new Request() { Title = "Get Azure AD Token for Blob Storage REST API Copy" };
        collection.Requests.Add(request);

        request = new Request() { Title = "Get Azure AD Token for ADLSGen2 Storage REST API" };
        collection.Requests.Add(request);

        request = new Request() { Title = "Get Azure AD Token for Azure Service Bus" };
        collection.Requests.Add(request);

        Collections.Add(collection);

        collection = new Collection() { Title = "Azure Storage | Blob Service" };
        Collections.Add(collection);

        collection = new Collection() { Title = "Azure Storage | Data Lake Storage Gen2 as das dasdasdasdas" };
        Collections.Add(collection);

        collection = new Collection() { Title = "Azure Storage | File Service" };
        Collections.Add(collection);

        collection = new Collection() { Title = "Azure Storage | Queue Service" };
        Collections.Add(collection);
    }

    private TabViewItem CreateNewTab()
    {
        TabViewItem newItem = new TabViewItem();
        newItem.HeaderTemplate = TabViewItemHeaderTemplate;
        newItem.Header = "Untitled request";

        newItem.IconSource = new FontIconSource() { FontFamily = new FontFamily("Segoe Fluent Icons"), Glyph = "\ue915" };
        SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 57, 170, 246));
        newItem.IconSource.Foreground = myBrush;


        Frame frame = new Frame();
        frame.Navigate(typeof(RequestPage));
        newItem.Content = frame;
        newItem.IsSelected = true;
        return newItem;
    }

    #endregion

    #region << Events >>

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var naViewItemInvoked = (NavigationViewItem)args.InvokedItemContainer;

        if (args.InvokedItemContainer is not null)
        {
            var navItemTag = args.InvokedItemContainer.Tag?.ToString();
            if (!string.IsNullOrEmpty(navItemTag))
            {
                NavView_Navigate(navItemTag, new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo());
            }
        }
    }

    private void NavView_Navigate(string navItemTag, Microsoft.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
    {
        //Type page;

        //MenuOption item = menuOptions.First(p => p.ClassName.Equals(navItemTag));
        //page = Type.GetType(item.ClassName);

        //// Get the page type before navigation so you can prevent duplicate
        //// entries in the backstack.
        //var preNavPageType = ContentFrame.CurrentSourcePageType;

        //// Only navigate if the selected page isn't currently loaded.
        //if ((page is not null) && !Type.Equals(preNavPageType, page))
        //{
        //    ContentFrame.Navigate(page, null, transitionInfo);
        //}
    }

    private void tabView_AddTabButtonClick(TabView sender, object args)
    {
        sender.TabItems.Add(CreateNewTab());
    }

    private void tabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        sender.TabItems.Remove(args.Tab);
    }

    #endregion

}

#region << Internal Classes >>

internal class Collection
{
    public string Title
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public DateTime? CreationTime
    {
        get; set;
    }
    public DateTime? LastModifiedTime
    {
        get; set;
    }
    public ObservableCollection<Request> Requests = new ObservableCollection<Request>();
    public readonly bool IsCollection = true;
}

internal class Request
{
    public string Title
    {
        get; set;
    }
    public readonly bool IsCollection = false;
}

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
            Collection => CollectionTemplate,
            Request => RequestTemplate,
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
            Collection => CollectionTemplate,
            Request => RequestTemplate,
            _ => null,
        };
    }
}

    #endregion
