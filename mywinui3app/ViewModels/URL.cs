using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace mywinui3app.ViewModels;

public partial class URL : ObservableRecipient
{
    [ObservableProperty]
    public string rawURL;
    [ObservableProperty]
    public string protocol;
    [ObservableProperty]
    public ICollection<string> host;
    [ObservableProperty]
    public ICollection<string> path;
    [ObservableProperty]
    public IDictionary<string, string> variables;
    private readonly WeakReferenceMessenger _messenger;
    public URL()
    {
    }

    public URL(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    partial void OnRawURLChanged(string value)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.RefreshParameters);
            _messenger.Send(message);
        }
    }
}
