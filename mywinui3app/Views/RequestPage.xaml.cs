using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Automation.Peers;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using mywinui3app.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace mywinui3app.Views;
public sealed partial class RequestPage : Page
{

    #region << Variables >>

    public RequestViewModel? ViewModel
    {
        get;
    }

    private double datagridHeight = 275;
    DataGrid currentRequestDataGrid;
    DataPackage dataPackage = new DataPackage();


    #endregion

    #region << Constructor >>

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        this.InitializeComponent();
        currentRequestDataGrid = dtgridParameters;
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
                switch (comboMethods.SelectedValue)
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

    private void SimulateRequestCellClick(DataGridRow? row, DataGridColumn? column)
    {
        var firstColumn = currentRequestDataGrid.Columns[(column.DisplayIndex)];
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

    private void SimulateResponseCellClick(DataGridRow? row, DataGridColumn? column)
    {
        var firstColumn = dtgridResponseHeaders.Columns[(column.DisplayIndex)];
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

    #endregion

    #region << Events >>

    private void tabRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (tabRequest.SelectedIndex)
        {
            case 0:
                dtgridParameters.Visibility = Visibility.Visible;
                dtgridHeaders.Visibility = Visibility.Collapsed;
                dtgridBodyItems.Visibility = Visibility.Collapsed;
                dtgridContentSizer.TargetControl = dtgridParameters;

                break;
            case 1:
                dtgridParameters.Visibility = Visibility.Collapsed;
                dtgridHeaders.Visibility = Visibility.Visible;
                dtgridBodyItems.Visibility = Visibility.Collapsed;
                dtgridContentSizer.TargetControl = dtgridHeaders;
                break;
            default:
                dtgridParameters.Visibility = Visibility.Collapsed;
                dtgridHeaders.Visibility = Visibility.Collapsed;
                dtgridBodyItems.Visibility = Visibility.Visible;
                dtgridContentSizer.TargetControl = dtgridBodyItems;
                break;
        }
    }

    private void comboMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        this.SetTabViewHeaderTemplate(sender, true);
    }

    private void dtgridRequest_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.KeyDown -= dtgridRequest_KeyDown;
        e.Row.KeyDown += dtgridRequest_KeyDown;
    }

    private void dtgridResponseHeaders_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.KeyDown -= dtgridResponse_KeyDown;
        e.Row.KeyDown += dtgridResponse_KeyDown;       
    }

    private void dtgridRequest_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var row = sender as DataGridRow;
        if (row != null)
        {
            if (e.Key == VirtualKey.Tab)
            {
                switch (tabRequest.SelectedIndex)
                {
                    case 0:
                        currentRequestDataGrid = dtgridParameters;
                        break;
                    case 1:
                        currentRequestDataGrid = dtgridHeaders;
                        break;
                    default:
                        currentRequestDataGrid = dtgridBodyItems;
                        break;
                }

                var selectedRowIndex = 0;
                var selectedColumnIndex = 0;

                var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

                if (isShiftPressed)
                {
                    if (row.GetIndex() >= 0)
                    {
                        if (currentRequestDataGrid.CurrentColumn.DisplayIndex > 1)
                        {
                            selectedColumnIndex = currentRequestDataGrid.CurrentColumn.DisplayIndex - 1;
                            selectedRowIndex = row.GetIndex();
                            currentRequestDataGrid.CurrentColumn = currentRequestDataGrid.Columns[selectedColumnIndex];
                            currentRequestDataGrid.ScrollIntoView(currentRequestDataGrid.SelectedItem, currentRequestDataGrid.Columns[selectedColumnIndex]);
                            currentRequestDataGrid.BeginEdit();
                        }
                        else if (row.GetIndex() > 0)
                        {
                            selectedColumnIndex = 3;
                            selectedRowIndex = row.GetIndex() - 1;
                            currentRequestDataGrid.SelectedIndex = selectedRowIndex;
                            currentRequestDataGrid.CurrentColumn = currentRequestDataGrid.Columns[3];
                            currentRequestDataGrid.ScrollIntoView(currentRequestDataGrid.SelectedItem, currentRequestDataGrid.Columns[selectedColumnIndex]);
                            currentRequestDataGrid.BeginEdit();
                        }
                    }
                }
                else
                {
                    var itemCount = 0;
                    switch (tabRequest.SelectedIndex)
                    {
                        case 0:
                            itemCount = ViewModel.Parameters.Count;
                            break;

                        case 1:
                            itemCount = ViewModel.Headers.Count;
                            break;

                        default:
                            itemCount = ViewModel.Body.Count;
                            break;
                    }

                    if (row.GetIndex() <= itemCount - 1)
                    {
                        if (currentRequestDataGrid.CurrentColumn.DisplayIndex < currentRequestDataGrid.Columns.Count - 2)
                        {
                            selectedColumnIndex = currentRequestDataGrid.CurrentColumn.DisplayIndex + 1;
                            selectedRowIndex = row.GetIndex();
                            currentRequestDataGrid.CurrentColumn = currentRequestDataGrid.Columns[selectedColumnIndex];
                            currentRequestDataGrid.ScrollIntoView(currentRequestDataGrid.SelectedItem, currentRequestDataGrid.Columns[selectedColumnIndex]);
                            currentRequestDataGrid.BeginEdit();
                        }
                        else if (itemCount - 1 > row.GetIndex())
                        {
                            selectedColumnIndex = 1;
                            selectedRowIndex = row.GetIndex() + 1;
                            currentRequestDataGrid.SelectedIndex = selectedRowIndex;
                            currentRequestDataGrid.CurrentColumn = currentRequestDataGrid.Columns[1];
                            currentRequestDataGrid.ScrollIntoView(currentRequestDataGrid.SelectedItem, currentRequestDataGrid.Columns[selectedColumnIndex]);
                            currentRequestDataGrid.BeginEdit();
                        }
                        else
                        {
                            selectedColumnIndex = 1;
                            selectedRowIndex = row.GetIndex();
                            currentRequestDataGrid.ScrollIntoView(currentRequestDataGrid.SelectedItem, currentRequestDataGrid.Columns[selectedColumnIndex]);
                            SimulateRequestCellClick(row, currentRequestDataGrid.Columns[1]);
                        }

                    }
                }
                e.Handled = true;
            }
        }
    }

    private void dtgridResponse_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var row = sender as DataGridRow;
        if (row != null)
        {
            if (e.Key == VirtualKey.Tab)
            {
                var selectedRowIndex = 0;
                var selectedColumnIndex = 0;

                var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

                if (isShiftPressed)
                {
                    if (row.GetIndex() >= 0)
                    {
                        if (dtgridResponseHeaders.CurrentColumn.DisplayIndex > 0)
                        {
                            selectedColumnIndex = dtgridResponseHeaders.CurrentColumn.DisplayIndex - 1;
                            selectedRowIndex = row.GetIndex();
                            dtgridResponseHeaders.CurrentColumn = dtgridResponseHeaders.Columns[selectedColumnIndex];
                            dtgridResponseHeaders.ScrollIntoView(dtgridResponseHeaders.SelectedItem, dtgridResponseHeaders.Columns[selectedColumnIndex]);
                        }
                        else if (row.GetIndex() > 0)
                        {
                            selectedColumnIndex = 1;
                            selectedRowIndex = row.GetIndex() - 1;
                            dtgridResponseHeaders.SelectedIndex = selectedRowIndex;
                            dtgridResponseHeaders.CurrentColumn = dtgridResponseHeaders.Columns[selectedColumnIndex];
                            dtgridResponseHeaders.ScrollIntoView(dtgridResponseHeaders.SelectedItem, dtgridResponseHeaders.Columns[selectedColumnIndex]);
                            dtgridResponseHeaders.BeginEdit();
                        }
                    }
                }
                else
                {
                    var itemCount = ViewModel.Response.Headers.Count;

                    if (row.GetIndex() <= itemCount - 1)
                    {
                        if (dtgridResponseHeaders.CurrentColumn.DisplayIndex < dtgridResponseHeaders.Columns.Count - 1)
                        {
                            selectedColumnIndex = dtgridResponseHeaders.CurrentColumn.DisplayIndex + 1;
                            selectedRowIndex = row.GetIndex();
                            dtgridResponseHeaders.CurrentColumn = dtgridResponseHeaders.Columns[selectedColumnIndex];
                            dtgridResponseHeaders.ScrollIntoView(dtgridResponseHeaders.SelectedItem, dtgridResponseHeaders.Columns[selectedColumnIndex]);
                            dtgridResponseHeaders.BeginEdit();
                        }
                        else if (itemCount - 1 > row.GetIndex())
                        {
                            selectedColumnIndex = 0;
                            selectedRowIndex = row.GetIndex() + 1;
                            dtgridResponseHeaders.SelectedIndex = selectedRowIndex;
                            dtgridResponseHeaders.ScrollIntoView(dtgridResponseHeaders.SelectedItem, dtgridResponseHeaders.Columns[selectedColumnIndex]);
                            dtgridResponseHeaders.CurrentColumn = dtgridResponseHeaders.Columns[0];
                            dtgridResponseHeaders.BeginEdit();
                        }
                        else
                        {
                            selectedColumnIndex = 0;
                            selectedRowIndex = row.GetIndex();
                            dtgridResponseHeaders.CurrentColumn = dtgridResponseHeaders.Columns[selectedColumnIndex];
                            dtgridResponseHeaders.ScrollIntoView(dtgridResponseHeaders.SelectedItem, dtgridResponseHeaders.Columns[selectedColumnIndex]);
                            dtgridResponseHeaders.BeginEdit();
                        }
                    }
                }
                e.Handled = true;
            }
        }
    }

    private void dtgridRequest_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var newRequestDataGridHeight = e.NewSize.Height;
        double newJsonPanelHeight;
        double newHeadersPanelHeight;

        if (newRequestDataGridHeight > datagridHeight)
        {
            newJsonPanelHeight = gridResponseJson.Height - (newRequestDataGridHeight - datagridHeight);
            if (newJsonPanelHeight >= 0)
                gridResponseJson.Height -= (newRequestDataGridHeight - datagridHeight);

            newHeadersPanelHeight = gridResponseHeaders.Height - (newRequestDataGridHeight - datagridHeight);
            if (newHeadersPanelHeight >= 0)
                gridResponseHeaders.Height -= (newRequestDataGridHeight - datagridHeight);
        }
        else if (newRequestDataGridHeight < datagridHeight)
        {
            gridResponseJson.Height += (datagridHeight - newRequestDataGridHeight);
            gridResponseHeaders.Height += (datagridHeight - newRequestDataGridHeight);
        }
        datagridHeight = newRequestDataGridHeight;
        dtgridParameters.Height = datagridHeight;
        dtgridHeaders.Height = datagridHeight;
        dtgridBodyItems.Height = datagridHeight;
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

    private void TextBoxDatagridCell_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var isControlPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);

        if (isControlPressed)
        {
            var textbox = sender as TextBox;

            if (e.Key == VirtualKey.C)
            {
                CopyTextToClipboard(textbox);
                e.Handled = true;
            }
        }
    }

    private void CopyTextToClipboard(TextBox? textbox)
    {
        dataPackage.RequestedOperation = DataPackageOperation.Copy;
        if (textbox.SelectedText != "")
            dataPackage.SetText(textbox.SelectedText);
        else
            dataPackage.SetText("");

        Clipboard.SetContent(dataPackage);
    }

    private void txtUrl_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.isURLEditing = true;
    }

    private void txtUrl_LostFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.isURLEditing = false;
    }

    private void dtgridParameters_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.isParametersEditing = true;
    }

    private void dtgridParameters_LostFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.isParametersEditing = false;
    }

    private void tabResponse_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (tabResponse.SelectedIndex)
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

    private void TabViewItem_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is TabViewItem tabViewItem)
        {
            ToolTipService.SetToolTip(tabViewItem, null);
        }
    }

    private void DatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
    {
        var picker = sender as DatePicker;
        if (!string.IsNullOrEmpty(picker.Date.ToString()))
        {
            var item = dtgridHeaders.SelectedItem as HeaderItem;
            ViewModel.SetMsDate(item, (DateTimeOffset)picker.Date);
            item.DatePickerVisibility = "Collapsed";
            item.DateTextboxVisibility = "Visible";
            item.DatePickerButtonVisibility = "Visible";
            item.HideDatePickerButtonVisibility = "Collapsed";
        }
    }

    #endregion

}