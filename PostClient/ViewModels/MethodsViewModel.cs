using CommunityToolkit.Mvvm.ComponentModel;

namespace PostClient.ViewModels;

public partial class MethodsItemViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string foreground;

    public MethodsItemViewModel(string name)
    {
        Name = name;

        switch (name)
        {
            case "GET":
                Foreground = ForegroundColorHelper.GET;
                break;
            case "POST":
                Foreground = ForegroundColorHelper.POST;
                break;
            case "PUT":
                Foreground = ForegroundColorHelper.PUT;
                break;
            case "PATCH":
                Foreground = ForegroundColorHelper.PATCH;
                break;
            case "DELETE":
                Foreground = ForegroundColorHelper.DELETE;
                break;
            case "OPTIONS":
                Foreground = ForegroundColorHelper.OPTIONS;
                break;
            default:
                Foreground = ForegroundColorHelper.HEAD;
                break;
        }
    }
}