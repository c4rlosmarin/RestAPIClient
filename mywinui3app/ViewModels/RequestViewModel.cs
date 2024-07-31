using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace mywinui3app.ViewModels;

public partial class RequestViewModel : ObservableRecipient, IRecipient<string>, IRecipient<ParameterItem>, IRecipient<HeaderItem>, IRecipient<BodyItem>
{
    [ObservableProperty]
    public string requestId;
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string method;
    [ObservableProperty]
    public ObservableCollection<MethodsItemViewModel> methods;
    [ObservableProperty]
    public string rawURL;
    [ObservableProperty]
    public URL uRL;
    [ObservableProperty]
    public ObservableCollection<ParameterItem> parameters;
    [ObservableProperty]
    public ObservableCollection<HeaderItem> headers;
    [ObservableProperty]
    public ObservableCollection<BodyItem> body;
    [ObservableProperty]
    public ResponseViewModel response;

    private bool isInitialized = false;

    public RequestViewModel()
    {
        Name = "Untitled request";
        URL = new URL() { RawURL = "" };
        Parameters = new ObservableCollection<ParameterItem>();
        Headers = new ObservableCollection<HeaderItem>();
        Body = new ObservableCollection<BodyItem>();
        Methods = new ObservableCollection<MethodsItemViewModel>();
        Response = new ResponseViewModel();

        StrongReferenceMessenger.Default.Register<string>(this);
        StrongReferenceMessenger.Default.Register<ParameterItem>(this);
        StrongReferenceMessenger.Default.Register<HeaderItem>(this);
        StrongReferenceMessenger.Default.Register<BodyItem>(this);

        this.AddNewParameter();
        this.AddNewHeader();
        this.AddNewBodyItem();
        this.AddMethods();
        isInitialized = true;
    }


    public void AddMethods()
    {
        Methods.Add(new MethodsItemViewModel() { Name = "GET", Foreground = "Green" });
        Methods.Add(new MethodsItemViewModel() { Name = "POST", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "PUT", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "PATCH", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "DELETE", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "OPTIONS", Foreground = "Blue" });
    }

    public void AddNewParameter()
    {
        var Parameter = new ParameterItem() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        Parameter.PropertyChanged += Parameter_PropertyChanged;
        Parameters.Add(Parameter);
    }

    private void Parameter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        var item = sender as ParameterItem;
        int index = Parameters.IndexOf(item);

        if (index == Parameters.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewParameter();
        else if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;

        item.DeleteButtonVisibility = "Visible";
    }

    public void DeleteParameterItem(ParameterItem item)
    {
        Parameters.Remove(item);
        RefreshURL();
    }

    public void DeleteHeaderItem(HeaderItem item)
    {
        Headers.Remove(item);
    }

    public void DeleteBodyItem(BodyItem item)
    {
        Body.Remove(item);
    }

    public void AddNewHeader()
    {
        var Header = new HeaderItem() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        Header.PropertyChanged += Header_PropertyChanged;
        Headers.Add(Header);
    }

    private void Header_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as HeaderItem;
        int index = Headers.IndexOf(item);

        if (index == Headers.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewHeader();
        else if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;

        item.DeleteButtonVisibility = "Visible";
    }

    public void AddNewBodyItem()
    {
        var BodyItem = new BodyItem() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        BodyItem.PropertyChanged += BodyItem_PropertyChanged;
        body.Add(BodyItem);
    }

    private void BodyItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as BodyItem;
        int index = Body.IndexOf(item);

        if (index == Body.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewBodyItem();
        else if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;

        item.DeleteButtonVisibility = "Visible";
    }

    public async Task<string> SendRequestAsync()
    {
        using HttpClient client = new HttpClient();
        using MultipartFormDataContent form = new MultipartFormDataContent();
        HttpResponseMessage response;

        var request = new HttpRequestMessage(new HttpMethod("GET"), URL.RawURL);

        foreach (HeaderItem item in Headers)
        {
            if (item.IsEnabled)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        foreach (BodyItem item in Body)
        {
            if (item.IsEnabled)
                form.Add(new StringContent(item.Value), item.Key);
        }

        request.Content = form;
        response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        JsonDocument document = JsonDocument.Parse(responseBody);
        var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true });
        document.WriteTo(writer);
        writer.Flush();

        Response.Body = Encoding.UTF8.GetString(stream.ToArray());
        Response.Headers = new ObservableCollection<ResponseData>();

        foreach (var item in response.Headers)
        {
            foreach (var subitem in item.Value)
                Response.Headers.Add(new ResponseData() { Key = item.Key, Value = subitem.ToString() });
        }

        return Response.Body;
    }

    private void RefreshURL()
    {
        if (isInitialized)
        {
            var urlSplit = URL.RawURL.Split('?');
            var rawURL = urlSplit[0] + "?";
            bool isFirstParameter = true;

            foreach (var item in Parameters)
            {
                if (isFirstParameter)
                {
                    rawURL += item.Key + "=" + item.Value;
                    isFirstParameter = false;
                }
                else
                {
                    if (Parameters.IndexOf(item) <= Parameters.Count - 1 && (item.Key != "" || item.Value != ""))
                        rawURL += "&" + item.Key + "=" + item.Value;
                }
            }

            if (rawURL == "?=")
                rawURL = "";
            URL.RawURL = rawURL;
        }
    }



    public void Receive(string message)
    {
        RefreshURL();
    }

    public void Receive(ParameterItem item)
    {
        DeleteParameterItem(item);
    }

    public void Receive(HeaderItem item)
    {
        DeleteHeaderItem(item);
    }

    public void Receive(BodyItem item)
    {
        DeleteBodyItem(item);
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

public partial class ParameterItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string deleteButtonVisibility;    

    partial void OnKeyChanged(string value)
    {
        StrongReferenceMessenger.Default.Send("RefreshURL");
    }

    partial void OnValueChanged(string value)
    {
        StrongReferenceMessenger.Default.Send("RefreshURL");
    }

    [RelayCommand]
    public void DeleteParameterItem(ParameterItem item)
    {
        StrongReferenceMessenger.Default.Send(item);
    }
}

public partial class HeaderItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string deleteButtonVisibility;

    [RelayCommand]
    public void DeleteHeaderItem(HeaderItem item)
    {
        StrongReferenceMessenger.Default.Send(item);
    }
}

public partial class BodyItem : ObservableRecipient
{
    [ObservableProperty]
    public bool isEnabled;
    [ObservableProperty]
    public string key;
    [ObservableProperty]
    public string value;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public string deleteButtonVisibility;

    [RelayCommand]
    public void DeleteBodyItem(BodyItem item)
    {
        StrongReferenceMessenger.Default.Send(item);
    }
}