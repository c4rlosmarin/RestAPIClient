using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string requestId;
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string method;
    [ObservableProperty]
    public URL uRL;
    [ObservableProperty]
    public ObservableCollection<FormData> parameters;
    [ObservableProperty]
    public ObservableCollection<FormData> headers;
    [ObservableProperty]
    public ObservableCollection<FormData> body;


    public RequestViewModel()
    {
        Name = "Untitled request";
    }

    partial void OnNameChanged(string value)
    {
            var algo = 123; 
    }
}

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
}

public partial class FormData : ObservableRecipient
{
    [ObservableProperty]
    public bool isSelected;
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string deleteButtonVisibility;
}

public partial class MethodViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string foreground;
}
