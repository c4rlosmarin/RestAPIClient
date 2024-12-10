using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Postclient.ViewModels;

public partial class HeaderItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
    [ObservableProperty]
    public string isEnabledActive = "true";
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string isKeyReadyOnly;
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string isDescriptionReadyOnly;
    [ObservableProperty]
    public string deleteButtonVisibility;
    [ObservableProperty]
    public string uTCVisibility = "Collapsed";
    [ObservableProperty]
    public string datePickerButtonVisibility = "Collapsed";
    [ObservableProperty]
    public string hideDatePickerButtonVisibility = "Collapsed";
    [ObservableProperty]
    public string dateTextboxVisibility = "Visible";
    [ObservableProperty]
    public string datePickerVisibility = "Collapsed";

    private readonly WeakReferenceMessenger _messenger;

    public HeaderItem()
    {
    }

    public HeaderItem(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    public void GetDateTimeInUTC(HeaderItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.GetDateTimeInUTC, headerItem: item);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void DeleteHeaderItem(HeaderItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.DeleteHeaderItem, headerItem: item);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void ShowDatePickerItem(HeaderItem item)
    {
        item.DateTextboxVisibility = "Collapsed";
        item.DatePickerVisibility = "Visible";
        item.DatePickerButtonVisibility = "Collapsed";
        item.HideDatePickerButtonVisibility = "Visible";
    }

    [RelayCommand]
    public void HideDatePickerItem(HeaderItem item)
    {
        item.DateTextboxVisibility = "Visible";
        item.DatePickerVisibility = "Collapsed";
        item.DatePickerButtonVisibility = "Visible";
        item.HideDatePickerButtonVisibility = "Collapsed";
    }
}
