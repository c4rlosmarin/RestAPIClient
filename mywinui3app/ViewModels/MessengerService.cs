using CommunityToolkit.Mvvm.Messaging;

namespace mywinui3app.ViewModels;
public class MessengerService
{
    private readonly Dictionary<string, WeakReferenceMessenger> _messengers = new();

    public WeakReferenceMessenger GetMessenger(string key)
    {
        if (!_messengers.ContainsKey(key))
        {
            _messengers[key] = new WeakReferenceMessenger();
        }
        return _messengers[key];
    }
}
