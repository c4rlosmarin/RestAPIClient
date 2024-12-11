using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Postclient.ViewModels;

public partial class ParameterItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
    [ObservableProperty]
    public string isEnabledActive = "true";
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string isKeyReadyOnly = "false";
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string isValueReadyOnly = "false";
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string isDescriptionReadyOnly;
    [ObservableProperty]
    public string deleteButtonVisibility;
    private readonly WeakReferenceMessenger _messenger;
    public ParameterItem()
    {
    }

    public ParameterItem(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    partial void OnKeyChanged(string value)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.RefreshURL);
            _messenger.Send(message);
        }
    }


    partial void OnValueChanged(string value)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.RefreshURL);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void DeleteParameterItem(ParameterItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.DeleteParameterItem, parameterItem: item);
            _messenger.Send(message);
        }
    }
}
