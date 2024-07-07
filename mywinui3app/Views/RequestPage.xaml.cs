using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using mywinui3app.Models;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace mywinui3app.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RequestPage : Page
{

    #region << Variables >>

    public RequestModel? Request
    {
        get; set;
    }


    public ObservableCollection<Method>? Methods
    {
        get; private set;
    }

    #endregion

    #region << Constructor >>

    public RequestPage()
    {
        this.InitializeComponent();
        //Parameters = new ObservableCollection<DatagridRow>();
        this.InitializeParameters();
        this.InitializeMethods();
    }

    private void InitializeMethods()
    {
        Methods =
        [
            new Method() { Name = "GET", Foreground = "Green" },
            new Method() { Name = "POST", Foreground = "Blue" },
            new Method() { Name = "PUT", Foreground = "Blue" },
            new Method() { Name = "PATCH", Foreground = "Blue" },
            new Method() { Name = "DELETE", Foreground = "Blue" },
            new Method() { Name = "HEAD", Foreground = "Blue" },
            new Method() { Name = "OPTIONS", Foreground = "Blue" },
        ];
    }

    #endregion

    #region << Methods >>

    public void InitializeParameters()
    {
        Request = new RequestModel();
        Request.Parameters = new ObservableCollection<FormData>();
        var Parameter = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
        Request.Parameters.Add(Parameter);

        Request.Headers = new ObservableCollection<FormData>();
        var Header = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
        Request.Headers.Add(Header);

        Request.Body = new ObservableCollection<FormData>();
        var Body = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
        Request.Body.Add(Body);
    }

    private void SetTabViewHeaderTemplate(object sender, bool IsEditing)
    {
        TabView? myTabView = null;

        DependencyObject parent = null;

        if (sender is Button)
            parent = VisualTreeHelper.GetParent((Button)sender);
        else if (sender is ComboBox)
            parent = VisualTreeHelper.GetParent((ComboBox)sender);

        while (parent != null)
        {
            if (parent is TabView tabView)
            {
                myTabView = tabView;
                break;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        if (myTabView != null)
        {
            var myTabViewItem = myTabView.SelectedItem as TabViewItem;

            if (myTabViewItem != null)
            {
                switch (((Method)myMethodComboBox.SelectedValue).Name)
                {
                    case "GET":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingGETTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = GETTabViewItemHeaderTemplate;
                        break;
                    case "POST":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingPOSTTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = POSTTabViewItemHeaderTemplate;
                        break;
                    case "PUT":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingPUTTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = PUTTabViewItemHeaderTemplate;
                        break;
                    case "PATCH":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingPATCHTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = PATCHTabViewItemHeaderTemplate;
                        break;
                    case "DELETE":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingDELETETabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = DELETETabViewItemHeaderTemplate;
                        break;
                    case "HEAD":
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingHEADTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = HEADTabViewItemHeaderTemplate;
                        break;
                    default:
                        if (IsEditing)
                            myTabViewItem.HeaderTemplate = EditingOPTIONSTabViewItemHeaderTemplate;
                        else
                            myTabViewItem.HeaderTemplate = OPTIONSTabViewItemHeaderTemplate;
                        break;

                }
            }
        }
    }

    private void AddNewDatagridRow()
    {
        SelectorBarItem selectedItem = SelectorBar.SelectedItem;
        int currentSelectedIndex = SelectorBar.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                if (myDataGrid.SelectedIndex == Request.Parameters.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Parameter = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
                    Request.Parameters.Add(Parameter);
                }
                break;
            case 1:
                if (myDataGrid.SelectedIndex == Request.Headers.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Header = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
                    Request.Headers.Add(Header);
                }
                break;
            default:
                if (myDataGrid.SelectedIndex == Request.Body.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Body = new FormData() { IsSelected = true, Key = "", Value = "", Description = "" };
                    Request.Body.Add(Body);
                }
                break;
        }

    }

    public async Task<string> SendPostRequestAsync()
    {
        using HttpClient client = new HttpClient();
        using MultipartFormDataContent form = new MultipartFormDataContent();

        form.Add(new StringContent("client_credentials"), "grant_type");
        form.Add(new StringContent("eaf45b7f-2560-44c3-bda7-ebd8270e70f6"), "client_id");
        form.Add(new StringContent("-bj8Q~XqBcdF6E7AzwCeN020gyfknq1wKr6knaQr"), "client_secret");
        form.Add(new StringContent("https://management.azure.com"), "resource");

        HttpResponseMessage response = await client.PostAsync("https://login.microsoftonline.com/16b3c013-d300-468d-ac64-7eda0820b6d3/oauth2/token", form);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    #endregion

    #region << Events >>

    private void SelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                myDataGrid.ItemsSource = Request.Parameters;
                myDataGrid.Columns[1].Header = "Parameter";
                break;
            case 1:
                myDataGrid.ItemsSource = Request.Headers;
                myDataGrid.Columns[1].Header = "Header";
                break;
            default:
                myDataGrid.ItemsSource = Request.Body;
                myDataGrid.Columns[1].Header = "Key";
                break;
        }
    }

    private void dataGrid_PreparingCellForEdit(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridPreparingCellForEditEventArgs e)
    {
        if (e.EditingElement is TextBox t)
        {
            t.Focus(FocusState.Keyboard);

            t.KeyDown += EditingControl_KeyDown;
        }
    }

    private void EditingControl_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        this.AddNewDatagridRow();
    }

    private void myDataGrid_CurrentCellChanged(object sender, EventArgs e)
    {
        if (myDataGrid.CurrentColumn != null)
        {
            if (myDataGrid.CurrentColumn.DisplayIndex == 0)
            {
                InputInjector inputInjector = InputInjector.TryCreate();
                var info = new InjectedInputKeyboardInfo();
                info.VirtualKey = (ushort)(VirtualKey.Tab);
                inputInjector.InjectKeyboardInput(new[] { info });
            }
        }
    }

    private void myMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this.SetTabViewHeaderTemplate(sender, true);
    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        SetTabViewHeaderTemplate(sender, false);

        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Save your work?";
        dialog.PrimaryButtonText = "Save";
        dialog.SecondaryButtonText = "Don't Save";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;

        var result = dialog.ShowAsync();
    }

    private void btnSend_Click(object sender, RoutedEventArgs e)
    {
        SendPostRequestAsync();
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var dataGridRow = (FormData)button.DataContext;

        myDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

        SelectorBarItem selectedItem = SelectorBar.SelectedItem;
        int currentSelectedIndex = SelectorBar.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                if (Request.Parameters.Count > 1)
                    Request.Parameters.Remove(dataGridRow);
                break;
            case 1:
                if (Request.Headers.Count > 1)
                    Request.Headers.Remove(dataGridRow);
                break;
            default:
                if (Request.Body.Count > 1)
                    Request.Body.Remove(dataGridRow);
                break;
        }
    }

    #endregion

}

#region << Internal Clases >>

public class DatagridRow
{
    public bool IsSelected
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Value
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }

}

public class Method
{
    public string Name
    {
        get; set;
    }
    public string Foreground
    {
        get; set;
    }
}


#endregion
