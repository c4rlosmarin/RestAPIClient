using System.Collections.ObjectModel;
using CommunityToolkit.WinUI.UI.Automation.Peers;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using mywinui3app.ViewModels;
using Windows.System;

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

    private double ParametersDatagridHeight = 275;

    #endregion

    #region << Constructor >>

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        this.InitializeComponent();
    }


    #endregion

    #region << Methods >>

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
                switch (myMethodComboBox.SelectedValue)
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

    private async Task<string> SendRequestAsync()
    {
        return await ViewModel.SendRequestAsync();

        //var paragraph = new Paragraph();
        //var run = new Run();
        //run.Text = Response.Body;
        //paragraph.Inlines.Add(run);
        //this.txtJson.Blocks.Clear();
        //this.txtJson.Blocks.Add(paragraph);
    }

    #endregion

    #region << Events >>

    private void selectbarRequest_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        switch (selectbarRequest.SelectedItem.Text)
        {
            case "Parameters":
                dtgridFormData.ItemsSource = ViewModel.Parameters;
                dtgridFormData.Columns[1].Header = "Parameter";
                break;
            case "Headers":
                dtgridFormData.ItemsSource = ViewModel.Headers;
                dtgridFormData.Columns[1].Header = "Header";
                break;
            default:
                dtgridFormData.ItemsSource = ViewModel.Body;
                dtgridFormData.Columns[1].Header = "Key";
                break;
        }
    }

    private void dtgridFormData_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.KeyDown += dtgridFormData_KeyDown;
    }

    private void dtgridFormData_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        var row = sender as DataGridRow;
        if (row != null)
        {
            if (e.Key == VirtualKey.Tab)
            {
                if (isShiftPressed)
                {
                    if (row.GetIndex() >= 0)
                    {
                        if (dtgridFormData.CurrentColumn.DisplayIndex > 1)
                        {
                            dtgridFormData.CurrentColumn = dtgridFormData.Columns[dtgridFormData.CurrentColumn.DisplayIndex - 1];
                            dtgridFormData.BeginEdit();
                        }
                        else if (row.GetIndex() > 0)
                        {
                            dtgridFormData.SelectedIndex = row.GetIndex() - 1;
                            dtgridFormData.CurrentColumn = dtgridFormData.Columns[3];
                            dtgridFormData.BeginEdit();
                        }
                    }
                }
                else
                {
                    var itemCount = 0;
                    switch (selectbarRequest.SelectedItem.Text)
                    {
                        case "Parameters":
                            itemCount = ViewModel.Parameters.Count;
                            break;

                        case "Headers":
                            itemCount = ViewModel.Headers.Count;
                            break;
                        
                        default:
                            itemCount = ViewModel.Body.Count;
                            break;
                    }

                    if (row.GetIndex() <= itemCount - 1)
                    {
                        if (dtgridFormData.CurrentColumn.DisplayIndex < dtgridFormData.Columns.Count - 2)
                        {
                            dtgridFormData.CurrentColumn = dtgridFormData.Columns[dtgridFormData.CurrentColumn.DisplayIndex + 1];
                            dtgridFormData.BeginEdit();
                        }
                        else if (itemCount - 1 > row.GetIndex())
                        {
                            dtgridFormData.SelectedIndex = row.GetIndex() + 1;
                            dtgridFormData.CurrentColumn = dtgridFormData.Columns[1];
                            dtgridFormData.BeginEdit();
                        }
                        else
                            SimulateCellClick(row, dtgridFormData.Columns[1]);
                    }
                }
                e.Handled = true;
            }
        }
    }

    private void SimulateCellClick(DataGridRow? row, DataGridColumn? column)
    {
        var firstColumn = dtgridFormData.Columns[(column.DisplayIndex)];
        var firstCellContent = firstColumn.GetCellContent(row);
        if (firstCellContent != null)
        {
            var cell = firstCellContent.Parent as DataGridCell;
            if (cell != null)
            {
                var peer = new DataGridCellAutomationPeer(cell);
                var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProvider?.Invoke();
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
        //    ViewModel.Name = this.txtName.Text;
        //    ViewModel.Method = ((MethodsItemViewModel)myMethodComboBox.SelectedValue).Name;
        //    ViewModel.URL = new URL() { RawURL = this.txtUrl.Text };
        //    var response = SendRequestAsync();
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var item = (FormData)button.DataContext;

        dtgridFormData.CommitEdit(DataGridEditingUnit.Row, true);

        SelectorBarItem selectedItem = selectbarRequest.SelectedItem;
        int currentSelectedIndex = selectbarRequest.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                if (ViewModel.Parameters.Count > 1)
                {
                    ViewModel.Parameters.Remove(item);
                    var tempParameters = new ObservableCollection<FormData>();

                    foreach (var item2 in ViewModel.Parameters)
                        tempParameters.Add(item2);

                    dtgridFormData.ItemsSource = null;
                    ViewModel.Parameters = null;
                    ViewModel.Parameters = tempParameters;
                    dtgridFormData.ItemsSource = ViewModel.Parameters;
                    dtgridFormData.UpdateLayout();
                }
                break;
            case 1:
                if (ViewModel.Headers.Count > 1)
                {
                    ViewModel.Headers.Remove(item);
                    var tempHeaders = new ObservableCollection<FormData>();

                    foreach (var item2 in ViewModel.Headers)
                        tempHeaders.Add(item2);

                    dtgridFormData.ItemsSource = null;
                    ViewModel.Headers = null;
                    ViewModel.Headers = tempHeaders;
                    dtgridFormData.ItemsSource = ViewModel.Headers;
                    dtgridFormData.UpdateLayout();
                }
                break;
            default:
                if (ViewModel.Body.Count > 1)
                {
                    ViewModel.Body.Remove(item);
                    var tempBodyItems = new ObservableCollection<FormData>();

                    foreach (var item2 in ViewModel.Body)
                        tempBodyItems.Add(item2);

                    dtgridFormData.ItemsSource = null;
                    ViewModel.Body = null;
                    ViewModel.Body = tempBodyItems;
                    dtgridFormData.ItemsSource = ViewModel.Body;
                    dtgridFormData.UpdateLayout();
                }
                break;
        }
    }

    private void dtgridFormData_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var newRequestDataGridHeight = e.NewSize.Height;
        double newJsonPanelHeight;
        double newHeadersPanelHeight;

        if (newRequestDataGridHeight > ParametersDatagridHeight)
        {
            newJsonPanelHeight = gridResponseJson.Height - (newRequestDataGridHeight - ParametersDatagridHeight);
            if (newJsonPanelHeight >= 0)
                gridResponseJson.Height -= (newRequestDataGridHeight - ParametersDatagridHeight);

            newHeadersPanelHeight = gridResponseHeaders.Height - (newRequestDataGridHeight - ParametersDatagridHeight);
            if (newHeadersPanelHeight >= 0)
                gridResponseHeaders.Height -= (newRequestDataGridHeight - ParametersDatagridHeight);
        }
        else if (newRequestDataGridHeight < ParametersDatagridHeight)
        {
            gridResponseJson.Height += (ParametersDatagridHeight - newRequestDataGridHeight);
            gridResponseHeaders.Height += (ParametersDatagridHeight - newRequestDataGridHeight);
        }
        ParametersDatagridHeight = newRequestDataGridHeight;
    }

    private void selectbarResponse_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
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

    #endregion

}