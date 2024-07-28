using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Composition.Interactions;
using Windows.AI.MachineLearning;

namespace mywinui3app.ViewModels;

public partial class RequestViewModel : ObservableRecipient, IRecipient<string>
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
    public ObservableCollection<FormData> parameters;
    [ObservableProperty]
    public ObservableCollection<FormData> headers;
    [ObservableProperty]
    public ObservableCollection<FormData> body;
    [ObservableProperty]
    public ResponseViewModel response;

    private bool isInitialized = false;

    public RequestViewModel()
    {
        Name = "Untitled request";

        Parameters = new ObservableCollection<FormData>();
        Headers = new ObservableCollection<FormData>();
        Body = new ObservableCollection<FormData>();
        Methods = new ObservableCollection<MethodsItemViewModel>();
        URL = new URL() { RawURL = "" };

        StrongReferenceMessenger.Default.Register<string>(this);

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

    [RelayCommand]
    public void DeleteParameter(FormData item)
    {
        Parameters.Remove(item);
    }

    [RelayCommand]
    public void AddNewParameter()
    {
        var Parameter = new FormData() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        Parameter.PropertyChanged += Parameter_PropertyChanged;
        Parameters.Add(Parameter);
    }

    private void Parameter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        var item = sender as FormData;
        int index = Parameters.IndexOf(item);

        if (index == Parameters.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewParameter();

        item.DeleteButtonVisibility = "Visible";
        item.IsEnabled = true;
    }

    [RelayCommand]
    public void AddNewHeader()
    {
        var Header = new FormData() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        Header.PropertyChanged += Header_PropertyChanged;
        Headers.Add(Header);
    }

    private void Header_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as FormData;
        int index = Headers.IndexOf(item);

        if (index == Headers.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewHeader();

        item.DeleteButtonVisibility = "Visible";
        item.IsEnabled = true;
    }

    [RelayCommand]
    public void AddNewBodyItem()
    {
        var BodyItem = new FormData() { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed" };
        BodyItem.PropertyChanged += BodyItem_PropertyChanged;
        body.Add(BodyItem);
    }

    private void BodyItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as FormData;
        int index = Body.IndexOf(item);

        if (index == Body.Count - 1 && e.PropertyName != "IsEnabled")
            AddNewBodyItem();

        item.DeleteButtonVisibility = "Visible";
        item.IsEnabled = true;
    }

    private void HeadersItemChanged(object sender, PropertyChangedEventArgs e)
    {
        var item = sender as FormData;
        int index = Headers.IndexOf(item);
    }

    private void BodyItemChanged(object sender, PropertyChangedEventArgs e)
    {
        var item = sender as FormData;
        int index = Body.IndexOf(item);
    }

    public async Task<string> SendRequestAsync()
    {
        using HttpClient client = new HttpClient();
        using MultipartFormDataContent form = new MultipartFormDataContent();
        HttpResponseMessage response;

        int i;

        for (i = 0; i <= Parameters.Count - 1; i++)
        {
            if (Parameters[i].IsEnabled)
            {
                if (i == 0)
                    URL.RawURL += "?";
                else
                    URL.RawURL += "&";

                URL.RawURL += Parameters[i].Key + "=" + Parameters[i].Value;
            }
        }

        var request = new HttpRequestMessage(new HttpMethod("GET"), URL.RawURL);

        foreach (FormData item in Headers)
        {
            if (item.IsEnabled)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }

        foreach (FormData item in Body)
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

        Response = new ResponseViewModel();
        Response.Body = Encoding.UTF8.GetString(stream.ToArray()); ;

        Response.Headers = new ObservableCollection<ResponseData>();

        foreach (var item in response.Headers)
        {
            foreach (var subitem in item.Value)
            {
                Response.Headers.Add(new ResponseData() { Key = item.Key, Value = subitem.ToString() });
            }
        }

        return Response.Body;
    }

    public void Receive(string message)
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
                    if (Parameters.IndexOf(item) < Parameters.Count - 1)
                        rawURL += "&" + item.Key + "=" + item.Value;
                }
            }

            URL.RawURL = rawURL;            
        }
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
        StrongReferenceMessenger.Default.Send("KeyChanged");
    }

    partial void OnValueChanged(string value)
    {
        StrongReferenceMessenger.Default.Send("ValueChanged");
    }
}