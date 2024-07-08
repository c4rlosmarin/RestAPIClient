using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.Models;
public partial class RequestModel: ObservableObject
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
}

public partial class URL: ObservableObject
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

public partial class FormData: ObservableObject
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

public partial class Method: ObservableObject
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string foreground;
}