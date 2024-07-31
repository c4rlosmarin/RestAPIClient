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

    private double ParametersDatagridHeight = 275;
    DataGrid currentDataGrid;

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
    }

    #endregion

    #region << Events >>

    private void selectbarRequest_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        switch (selectbarRequest.SelectedItem.Text)
        {
            case "Parameters":
                dtgridParameters.Visibility = Visibility.Visible;
                dtgridHeaders.Visibility = Visibility.Collapsed;
                dtgridBodyItems.Visibility = Visibility.Collapsed;
                break;
            case "Headers":
                dtgridParameters.Visibility = Visibility.Collapsed;
                dtgridHeaders.Visibility = Visibility.Visible;
                dtgridBodyItems.Visibility = Visibility.Collapsed;
                break;
            default:
                dtgridParameters.Visibility = Visibility.Collapsed;
                dtgridHeaders.Visibility = Visibility.Collapsed;
                dtgridBodyItems.Visibility = Visibility.Visible;
                break;
        }
    }

    private void dtgridFormData_LoadingRow(object sender, DataGridRowEventArgs e)
    {
        e.Row.KeyDown -= dtgridFormData_KeyDown;
        e.Row.KeyDown += dtgridFormData_KeyDown;
    }

    private void dtgridFormData_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var isShiftPressed = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
        var selectedRowIndex = 0;
        var selectedColumnIndex = 0;

        var row = sender as DataGridRow;
        if (row != null)
        {
            if (e.Key == VirtualKey.Tab)
            {
                var currentSelectedIndex = GetCurrentlySelectedRequestTab();

                switch (currentSelectedIndex)
                {
                    case 0:
                        currentDataGrid = dtgridParameters;
                        break;
                    case 1:
                        currentDataGrid = dtgridHeaders;
                        break;
                    default:
                        currentDataGrid = dtgridBodyItems;
                        break;
                }

                if (isShiftPressed)
                {
                    if (row.GetIndex() >= 0)
                    {
                        if (currentDataGrid.CurrentColumn.DisplayIndex > 1)
                        {
                            selectedColumnIndex = currentDataGrid.CurrentColumn.DisplayIndex - 1;
                            selectedRowIndex = row.GetIndex();
                            currentDataGrid.CurrentColumn = currentDataGrid.Columns[selectedColumnIndex];
                            currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem, currentDataGrid.Columns[selectedColumnIndex]);
                            currentDataGrid.BeginEdit();
                        }
                        else if (row.GetIndex() > 0)
                        {
                            selectedColumnIndex = 3;
                            selectedRowIndex = row.GetIndex() - 1;
                            currentDataGrid.SelectedIndex = selectedRowIndex;
                            currentDataGrid.CurrentColumn = currentDataGrid.Columns[3];
                            currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem, currentDataGrid.Columns[selectedColumnIndex]);
                            currentDataGrid.BeginEdit();
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
                        if (currentDataGrid.CurrentColumn.DisplayIndex < currentDataGrid.Columns.Count - 2)
                        {
                            selectedColumnIndex = currentDataGrid.CurrentColumn.DisplayIndex + 1;
                            selectedRowIndex = row.GetIndex();
                            currentDataGrid.CurrentColumn = currentDataGrid.Columns[selectedColumnIndex];
                            currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem, currentDataGrid.Columns[selectedColumnIndex]);
                            currentDataGrid.BeginEdit();
                        }
                        else if (itemCount - 1 > row.GetIndex())
                        {
                            selectedColumnIndex = 1;
                            selectedRowIndex = row.GetIndex() + 1;
                            currentDataGrid.SelectedIndex = selectedRowIndex;
                            currentDataGrid.CurrentColumn = currentDataGrid.Columns[1];
                            currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem, currentDataGrid.Columns[selectedColumnIndex]);
                            currentDataGrid.BeginEdit();
                        }
                        else
                        {
                            selectedColumnIndex = 1;
                            selectedRowIndex = row.GetIndex();
                            currentDataGrid.ScrollIntoView(currentDataGrid.SelectedItem, currentDataGrid.Columns[selectedColumnIndex]);
                            SimulateCellClick(row, currentDataGrid.Columns[1]);
                        }

                    }
                }
                e.Handled = true;
            }
        }        
    }

    private int GetCurrentlySelectedRequestTab()
    {
        SelectorBarItem selectedItem = selectbarRequest.SelectedItem;
        int currentSelectedIndex = selectbarRequest.Items.IndexOf(selectedItem);
        return currentSelectedIndex;
    }

    private void SimulateCellClick(DataGridRow? row, DataGridColumn? column)
    {
        var firstColumn = currentDataGrid.Columns[(column.DisplayIndex)];
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
        var response = SendRequestAsync();
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        dtgridParameters.CommitEdit(DataGridEditingUnit.Row, true);

        var currentSelectedIndex = GetCurrentlySelectedRequestTab();

        switch (currentSelectedIndex)
        {
            case 0:
                if (ViewModel.Parameters.Count > 1)
                {
                    var item = (ParameterItem)button.DataContext;
                    ViewModel.Parameters.Remove(item);
                    ViewModel.Receive("ParameterDeleted");
                }
                break;
            case 1:
                if (ViewModel.Headers.Count > 1)
                {
                    var item = (HeaderItem)button.DataContext;
                    ViewModel.Headers.Remove(item);
                }
                break;
            default:
                if (ViewModel.Body.Count > 1)
                {
                    var item = (BodyItem)button.DataContext;
                    ViewModel.Body.Remove(item);
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