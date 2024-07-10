using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using mywinui3app.ViewModels;
using Windows.System;
using Windows.UI.Input.Preview.Injection;

namespace mywinui3app.Views;
public sealed partial class RequestPage : Page
{

    #region << Variables >>

    public RequestViewModel? ViewModel
    {
        get;
    }

    public ResponseViewModel? Response
    {
        get; set;
    }

    public ObservableCollection<MethodViewModel>? Methods
    {
        get; private set;
    }

    private double CurrentRequestDatagridHeight = 275;

    #endregion

    #region << Constructor >>

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        this.InitializeComponent();
        this.InitializeRequest();
    }

    private void InitializeMethods()
    {
        Methods =
        [
            new MethodViewModel() { Name = "GET", Foreground = "Green" },
            new MethodViewModel() { Name = "POST", Foreground = "Blue" },
            new MethodViewModel() { Name = "PUT", Foreground = "Blue" },
            new MethodViewModel() { Name = "PATCH", Foreground = "Blue" },
            new MethodViewModel() { Name = "DELETE", Foreground = "Blue" },
            new MethodViewModel() { Name = "HEAD", Foreground = "Blue" },
            new MethodViewModel() { Name = "OPTIONS", Foreground = "Blue" },
        ];
    }

    #endregion

    #region << Methods >>

    public void InitializeRequest()
    {
        this.InitializeMethods();
        this.InitializeForms();
    }

    private void InitializeForms()
    {
        ViewModel.Parameters = new ObservableCollection<FormData>();
        var Parameter = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        ViewModel.Parameters.Add(Parameter);

        ViewModel.Headers = new ObservableCollection<FormData>();
        var Header = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        ViewModel.Headers.Add(Header);

        ViewModel.Body = new ObservableCollection<FormData>();
        var Body = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        ViewModel.Body.Add(Body);
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
                switch (((MethodViewModel)myMethodComboBox.SelectedValue).Name)
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
        SelectorBarItem selectedItem = barRequest.SelectedItem;
        int currentSelectedIndex = barRequest.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                if (myDataGrid.SelectedIndex == ViewModel.Parameters.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Parameter = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
                    ViewModel.Parameters.Add(Parameter);
                    ViewModel.Parameters[myDataGrid.SelectedIndex].IsSelected = true;
                    ViewModel.Parameters[myDataGrid.SelectedIndex].DeleteButtonVisibility = "Visible";
                }
                break;
            case 1:
                if (myDataGrid.SelectedIndex == ViewModel.Headers.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Header = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
                    ViewModel.Headers.Add(Header);
                    ViewModel.Headers[myDataGrid.SelectedIndex].IsSelected = true;
                    ViewModel.Headers[myDataGrid.SelectedIndex].DeleteButtonVisibility = "Visible";
                }
                break;
            default:
                if (myDataGrid.SelectedIndex == ViewModel.Body.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
                {
                    var Body = new FormData() { IsSelected = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
                    ViewModel.Body.Add(Body);
                    ViewModel.Body[myDataGrid.SelectedIndex].IsSelected = true;
                    ViewModel.Body[myDataGrid.SelectedIndex].DeleteButtonVisibility = "Visible";
                }
                break;
        }

    }

    public async Task<string> SendRequestAsync()
    {
        using HttpClient client = new HttpClient();
        using MultipartFormDataContent form = new MultipartFormDataContent();
        HttpResponseMessage response;

        int i;

        for (i = 0; i <= ViewModel.Parameters.Count - 1; i++)
        {
            if (ViewModel.Parameters[i].IsSelected)
            {
                if (i == 0)
                    ViewModel.URL.RawURL += "?";
                else
                    ViewModel.URL.RawURL += "&";

                ViewModel.URL.RawURL += ViewModel.Parameters[i].Key + "=" + ViewModel.Parameters[i].Value;
            }
        }

        var request = new HttpRequestMessage(new HttpMethod(((MethodViewModel)myMethodComboBox.SelectedItem).Name), ViewModel.URL.RawURL);

        foreach (FormData item in ViewModel.Headers)
        {
            if (item.IsSelected)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        foreach (FormData item in ViewModel.Body)
        {
            if (item.IsSelected)
                form.Add(new StringContent(item.Value), item.Key);
        }

        request.Content = form;
        response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        JsonDocument document = JsonDocument.Parse(responseBody);
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true });
        document.WriteTo(writer);
        writer.Flush();


        Response = new ResponseViewModel();
        Response.Body = Encoding.UTF8.GetString(stream.ToArray()); ;

        var paragraph = new Paragraph();
        var run = new Run();
        run.Text = Response.Body;
        paragraph.Inlines.Add(run);
        this.txtJson.Blocks.Clear();
        this.txtJson.Blocks.Add(paragraph);

        Response.Headers = new ObservableCollection<ResponseData>();

        foreach (var item in response.Headers)
        {
            foreach (var subitem in item.Value)
            {
                Response.Headers.Add(new ResponseData() { Key = item.Key, Value = subitem.ToString() });
            }
        }

        dtgridResponseHeaders.ItemsSource = Response.Headers;

        return Response.Body;
    }

    #endregion

    #region << Events >>

    private void barRequest_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                myDataGrid.ItemsSource = ViewModel.Parameters;
                myDataGrid.Columns[1].Header = "Parameter";
                break;
            case 1:
                myDataGrid.ItemsSource = ViewModel.Headers;
                myDataGrid.Columns[1].Header = "Header";
                break;
            default:
                myDataGrid.ItemsSource = ViewModel.Body;
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

    private void RefreshUrlText()
    {
        SelectorBarItem selectedItem = barRequest.SelectedItem;
        var currentSelectedIndex = barRequest.Items.IndexOf(selectedItem);

        if (currentSelectedIndex == 0)
        {
            if (myDataGrid.CurrentColumn.DisplayIndex == 1)
            {
                var urlSplit = txtUrl.Text.Split('?');
                var rawUrl = "";


                if (urlSplit.Length == 1)
                    rawUrl = urlSplit[0] + "?";
                else
                    rawUrl = urlSplit[0] + "&";

                foreach (var item in ViewModel.Parameters)
                {
                    rawUrl += "&" + item.Key + "=" + item.Value;
                }








                //string text = this.txtUrl.Text;

                //if (!string.IsNullOrEmpty(text))
                //{
                //    var lastCharArray = text.Substring(text.Length-1,1).ToCharArray();

                //    if (lastCharArray.Length == 1)
                //    {
                //        var algo = lastCharArray[0];
                //    }

                //}
            }
        }
    }

    private void EditingControl_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        this.AddNewDatagridRow();
        this.RefreshUrlText();
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
        ViewModel.Name = this.txtName.Text;
        ViewModel.Method = ((MethodViewModel)myMethodComboBox.SelectedValue).Name;
        ViewModel.URL = new URL() { RawURL = this.txtUrl.Text };
        var response = SendRequestAsync();
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var dataGridRow = (FormData)button.DataContext;

        myDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

        SelectorBarItem selectedItem = barRequest.SelectedItem;
        int currentSelectedIndex = barRequest.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                if (ViewModel.Parameters.Count > 1)
                    ViewModel.Parameters.Remove(dataGridRow);
                break;
            case 1:
                if (ViewModel.Headers.Count > 1)
                    ViewModel.Headers.Remove(dataGridRow);
                break;
            default:
                if (ViewModel.Body.Count > 1)
                    ViewModel.Body.Remove(dataGridRow);
                break;
        }
    }

    #endregion

    private void myDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var newRequestDataGridHeight = e.NewSize.Height;
        double newJsonPanelHeight;
        double newHeadersPanelHeight;

        if (newRequestDataGridHeight > CurrentRequestDatagridHeight)
        {
            newJsonPanelHeight = gridResponseJson.Height - (newRequestDataGridHeight - CurrentRequestDatagridHeight);
            if (newJsonPanelHeight >= 0)
                gridResponseJson.Height -= (newRequestDataGridHeight - CurrentRequestDatagridHeight);

            newHeadersPanelHeight = gridResponseHeaders.Height - (newRequestDataGridHeight - CurrentRequestDatagridHeight);
            if (newHeadersPanelHeight >= 0)
                gridResponseHeaders.Height -= (newRequestDataGridHeight - CurrentRequestDatagridHeight);
        }
        else if (newRequestDataGridHeight < CurrentRequestDatagridHeight)
        {
            gridResponseJson.Height += (CurrentRequestDatagridHeight - newRequestDataGridHeight);
            gridResponseHeaders.Height += (CurrentRequestDatagridHeight - newRequestDataGridHeight);
        }
        CurrentRequestDatagridHeight = newRequestDataGridHeight;
    }

    private void barResponse_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                gridResponseJson.Visibility = Visibility.Visible;
                gridResponseHeaders.Visibility = Visibility.Collapsed;
                break;
            case 1:
                gridResponseJson.Visibility = Visibility.Collapsed;
                gridResponseHeaders.Visibility = Visibility.Visible;
                break;
        }
    }

    private void txtUrl_TextChanged(object sender, TextChangedEventArgs e)
    {
        //string text = this.txtUrl.Text;

        //if (!string.IsNullOrEmpty(text))
        //{
        //    var lastCharArray = text.Substring(text.Length-1,1).ToCharArray();

        //    if (lastCharArray.Length == 1)
        //    {
        //        var algo = lastCharArray[0];
        //    }

        //}
    }
}