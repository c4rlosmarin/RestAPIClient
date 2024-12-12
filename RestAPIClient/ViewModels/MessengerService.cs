using CommunityToolkit.Mvvm.Messaging;

namespace RestAPIClient.ViewModels;
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
