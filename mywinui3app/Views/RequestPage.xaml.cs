using CommunityToolkit.WinUI.UI.Controls;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace mywinui3app.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RequestPage : Page
{

    #region << Variables >>

    int previousSelectedIndex = 0;
    public ObservableCollection<DatagridRow>? Parameters
    {
        get; private set;
    }
    public ObservableCollection<DatagridRow>? Headers
    {
        get; private set;
    }
    public ObservableCollection<DatagridRow>? Body
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
    }

    #endregion

    #region << Methods >>

    public void InitializeParameters()
    {
        Parameters = new ObservableCollection<DatagridRow> { new DatagridRow() { IsSelected = true, Name = "", Value = "", Description = "" } };

        Headers = new ObservableCollection<DatagridRow>
            {
                new DatagridRow() { IsSelected = true, Name = "Header1", Value = "Value1", Description = "Description1" },
                new DatagridRow() { IsSelected = true, Name = "Header2", Value = "Value2", Description = "Description2" }
            };

        Body = new ObservableCollection<DatagridRow>
            {
                new DatagridRow() { IsSelected = true, Name = "Body1", Value = "Value1", Description = "Description1" },
                new DatagridRow() { IsSelected = true, Name = "Body2", Value = "Value2", Description = "Description2" }
            };
    }

    private void AddNewDatagridRow()
    {
        SelectorBarItem selectedItem = SelectorBar.SelectedItem;
        int currentSelectedIndex = SelectorBar.Items.IndexOf(selectedItem);

        switch (currentSelectedIndex)
        {
            case 0:
                Parameters.Add(new DatagridRow() { IsSelected = true, Name = "", Value = "", Description = "" });
                break;
            case 1:
                Headers.Add(new DatagridRow() { IsSelected = true, Name = "", Value = "", Description = "" });
                break;
            default:
                Body.Add(new DatagridRow() { IsSelected = true, Name = "", Value = "", Description = "" });
                break;
        }

    }

    private static void RemoveTabUnsavedRequestIcon(object sender)
    {
        TabView? myTabView = null;
        DependencyObject parent = VisualTreeHelper.GetParent((Button)sender);
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
                myTabViewItem.IconSource = null;
        }
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
                myDataGrid.ItemsSource = Parameters;
                myDataGrid.Columns[1].Header = "Parameter";
                break;
            case 1:
                myDataGrid.ItemsSource = Headers;
                myDataGrid.Columns[1].Header = "Header";
                break;
            default:
                myDataGrid.ItemsSource = Body;
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
        if (myDataGrid.SelectedIndex == Parameters.Count - 1 && myDataGrid.CurrentColumn.DisplayIndex != 0)
        {
            this.AddNewDatagridRow();
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        RemoveTabUnsavedRequestIcon(sender);

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

    #endregion

    private void myDataGrid_LoadingRow(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEventArgs e)
    {

    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        var selectedItem = myDataGrid.SelectedIndex;


    }
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

#endregion
