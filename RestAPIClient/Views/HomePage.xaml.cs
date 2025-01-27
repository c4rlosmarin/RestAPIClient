using System.Collections.ObjectModel;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using RestAPIClient.Helpers;
using RestAPIClient.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RestAPIClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        public TabsViewModel TabsViewModel
        {
            get;
        }

        public HomePage()
        {
            TabsViewModel = App.GetService<TabsViewModel>();
            this.InitializeComponent();
        }

        #region << Methods >>

        public void CreateRequestTab(RequestItem? request)
        {
            //TODO: Implementar el estilo y texto del tabViewItem de forma dinámica
            Frame frame = new Frame();
            TabItem newTabItem;            

            if (request == null)
            {
                newTabItem = new TabItem() { Title = "Untitled request", EditingIconVisibility = "Visible", Method = "GET", Foreground = ColorHelper.CreateSolidColorBrushFromHex(ForegroundColorHelper.GET) };
                TabsViewModel.Tabs.Add(newTabItem);
                frame.Navigate(typeof(RequestPage));
            }
            else
            {
                foreach (TabItem item in tabView.TabItems)
                {
                    if (item.Id == request.RequestId)
                    {
                        tabView.SelectionChanged -= tabView_SelectionChanged;
                        TabsViewModel.SelectedTabItem = item;
                        tabView.SelectionChanged += tabView_SelectionChanged;
                        return;
                    }
                }

                newTabItem = new TabItem() { Id = request.RequestId, Title = request.Name, EditingIconVisibility = "Collapsed", Method = request.method, Foreground = ColorHelper.CreateSolidColorBrushFromHex(ForegroundColorHelper.GetColorByMethod(request.method)) };
                TabsViewModel.Tabs.Add(newTabItem);
                frame.Navigate(typeof(RequestPage), request);
            }

            tabView.SelectionChanged -= tabView_SelectionChanged;

            TabsViewModel.SelectedTabItem = newTabItem;

            TabViewItem newItem = tabView.ContainerFromItem(tabView.SelectedItem) as TabViewItem;
            newItem.Content = frame;

            var originalSelectedItem = tabView.SelectedItem;
            tabView.SelectedItem = null;
            tabView.SelectedItem = originalSelectedItem;

            tabView.SelectionChanged += tabView_SelectionChanged;
        }

        private void RefreshSelectedMenuItem()
        {
            var parentPage = ((Grid)((Frame)this.Parent).Parent).Parent as NavigationView;
            if (parentPage != null)
            {
                var navigationView = parentPage.FindName("NavigationViewControl") as NavigationView;
                if (tabView.SelectedItem != null)
                {
                    var node = FindNavigationViewItemByName((ObservableCollection<NavigationMenuItem>)navigationView.MenuItemsSource, ((TabItem)tabView.SelectedItem).Id, ((TabItem)tabView.SelectedItem).Title);
                    navigationView.SelectedItem = node;
                }
                else
                    navigationView.SelectedItem = null;
            }
        }

        private NavigationMenuItem FindNavigationViewItemByName(ObservableCollection<NavigationMenuItem> navigationViewItemsSource, string requestId, string name)
        {
            foreach (var item in navigationViewItemsSource)
            {
                var result = FindNavigationViewItemByNameRecursive(item.SubMenus, requestId, name);

                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private NavigationMenuItem FindNavigationViewItemByNameRecursive(ObservableCollection<NavigationMenuItem> navigationViewItems, string requestId, string name)
        {
            if (navigationViewItems == null) return null;

            foreach (var item in navigationViewItems)
            {
                if (item != null && item.Name == name && item.RequestId == requestId)
                    return item;
            }

            foreach (var item in navigationViewItems)
            {
                var result = FindNavigationViewItemByNameRecursive(item.SubMenus, requestId, name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        #endregion

        #region << Events >>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is RequestItem request)
            {
                CreateRequestTab(request);
            }
        }

        private void tabView_AddTabButtonClick(TabView sender, object args)
        {
            tabView.SelectionChanged -= tabView_SelectionChanged;

            CreateRequestTab(null);

            tabView.SelectionChanged += tabView_SelectionChanged;
        }

        private void tabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            TabsViewModel.Tabs.Remove((TabItem)args.Item);
        }

        private void tabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSelectedMenuItem();
        }

        #endregion
    }
}
