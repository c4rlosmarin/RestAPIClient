using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Postclient.ViewModels;

public partial class BodyItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
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
    private readonly WeakReferenceMessenger _messenger;

    public BodyItem()
    {
    }

    public BodyItem(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }


    [RelayCommand]
    public void DeleteBodyItem(BodyItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.DeleteBodyItem, bodyItem: item);
            _messenger.Send(message);
        }
    }
}
