namespace Postclient.ViewModels;

public class ViewModelLocator
{
    private readonly MessengerService _messengerService = new();

    public RequestViewModel CreateRequestModel(string tabKey)
    {
        var viewModel = new RequestViewModel();
        viewModel.InitializeMessenger(tabKey, _messengerService);
        return viewModel;
    }

    public ParameterItem CreateChildViewModel(string tabKey)
    {
        var messenger = _messengerService.GetMessenger(tabKey);
        return new ParameterItem(messenger);
    }
}
