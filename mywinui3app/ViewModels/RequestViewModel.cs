using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media;

namespace mywinui3app.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string requestId;
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string tabIconVisibility;
    [ObservableProperty]
    public string tabMethodForegroundColor;
    [ObservableProperty]
    public MethodsItemViewModel selectedMethod;
    [ObservableProperty]
    public ObservableCollection<MethodsItemViewModel> methods;
    [ObservableProperty]
    public string isMethodComboEnabled;
    public bool isURLEditing;
    [ObservableProperty]
    public URL uRL;
    public bool isParametersEditing;
    [ObservableProperty]
    internal ObservableCollection<ParameterItem> parameters;
    [ObservableProperty]
    public string parametersCount;
    [ObservableProperty]
    internal ObservableCollection<HeaderItem> headers;
    [ObservableProperty]
    public string headersCount;
    [ObservableProperty]
    public string selectedBodyType = "None";
    [ObservableProperty]
    public ObservableCollection<string> bodyTypes;
    [ObservableProperty]
    public string isBodyComboEnabled;
    [ObservableProperty]
    internal ObservableCollection<BodyItem> body;
    [ObservableProperty]
    public string rawBody;
    [ObservableProperty]
    public string bodyItemsCount;
    [ObservableProperty]
    public ResponseViewModel response;
    public bool IsExistingRequest;


    private WeakReferenceMessenger _messenger;

    private Stopwatch Stopwatch = new();

    public RequestViewModel()
    {

    }

    public void Initialize(string requestId, RequestItem? request = null)
    {
        this.AddMethods();
        this.AddBodyTypes();

        if (IsExistingRequest)
            InitializeExistingRequest(request);
        else
        {
            InitializeRequest(requestId);
            _messenger.Register<RequestViewModel, RequestMessage>(this, (r, m) =>
            {
                var message = m as RequestMessage;
                switch (message.Name)
                {
                    case Command.RefreshURL:
                        RefreshURL();
                        break;
                    case Command.DeleteParameterItem:
                        Parameters.Remove(m.ParameterItem);
                        SetParameterCount();
                        RefreshURL();
                        break;
                    case Command.GetDateTimeInUTC:
                        message.HeaderItem.Value = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
                        break;

                    case Command.DeleteHeaderItem:
                        DeleteHeaderItem(message.HeaderItem);
                        break;
                    case Command.DeleteBodyItem:
                        DeleteBodyItem(message.BodyItem);
                        break;
                    case Command.RefreshParameters:
                        RefreshParameters();
                        break;
                }
            });
        }
    }

    public void AddBodyTypes()
    {
        BodyTypes = new ObservableCollection<string>() { "None", "Form", "Json", "Xml", "Text" };
    }

    public void InitializeMessenger(string tabKey, MessengerService messengerService)
    {
        _messenger = messengerService.GetMessenger(tabKey);
    }

    private void InitializeRequest(string requestId)
    {
        RequestId = requestId;
        IsExistingRequest = false;
        Name = "Untitled request";
        TabIconVisibility = "Visible";
        var foregroundColorHelper = new MethodForegroundColor();
        TabMethodForegroundColor = foregroundColorHelper.GET;
        IsMethodComboEnabled = "true";
        URL = new URL(_messenger) { RawURL = "" };
        Parameters = new ObservableCollection<ParameterItem>();
        Headers = new ObservableCollection<HeaderItem>();
        Body = new ObservableCollection<BodyItem>();
        Response = new ResponseViewModel();
        IsBodyComboEnabled = "true";

        this.AddNewParameter();
        this.AddNewHeader();
        this.AddNewBodyItem();
    }

    public void InitializeExistingRequest(RequestItem request)
    {
        RequestId = request.RequestId;
        Name = request.Name;
        TabIconVisibility = request.TabIconVisibility;
        TabMethodForegroundColor = request.TabMethodForegroundColor;
        IsMethodComboEnabled = request.IsMethodComboEnabled;
        URL = new URL(_messenger) { RawURL = request.URL.RawURL };

        foreach (MethodsItemViewModel item in Methods)
        {
            if (item.Name == SelectedMethod.Name)
                SelectedMethod = item;
        }

        Parameters = new ObservableCollection<ParameterItem>();
        foreach (var item in request.Parameters)
            Parameters.Add(new ParameterItem(_messenger) { IsEnabled = item.IsEnabled, Key = item.Key, Value = item.Value, Description = item.Description, IsKeyReadyOnly = item.IsKeyReadyOnly, IsDescriptionReadyOnly = item.IsDescriptionReadyOnly, DeleteButtonVisibility = item.DeleteButtonVisibility });

        foreach (ParameterItem item in Parameters)
            item.PropertyChanged += Parameter_PropertyChanged;

        if (Parameters.Count > 0)
        {
            isParametersEditing = true;
            RefreshURL();
            isParametersEditing = false;
        }
        SetParameterCount();

        Headers = new ObservableCollection<HeaderItem>();
        foreach (var item in request.Headers)
            Headers.Add(new HeaderItem(_messenger) { IsEnabled = item.IsEnabled, Key = item.Key, Value = item.Value, Description = item.Description, IsKeyReadyOnly = item.IsKeyReadyOnly, IsDescriptionReadyOnly = item.IsDescriptionReadyOnly, DeleteButtonVisibility = item.DeleteButtonVisibility });

        foreach (HeaderItem item in Headers)
            item.PropertyChanged += Header_PropertyChanged;
        SetHeaderCount();

        Body = new ObservableCollection<BodyItem>();
        foreach (var item in request.Body)
            Body.Add(new BodyItem(_messenger) { IsEnabled = item.IsEnabled, Key = item.Key, Value = item.Value, Description = item.Description, IsKeyReadyOnly = item.IsKeyReadyOnly, IsDescriptionReadyOnly = item.IsDescriptionReadyOnly, DeleteButtonVisibility = item.DeleteButtonVisibility });

        foreach (BodyItem item in Body)
            item.PropertyChanged += BodyItem_PropertyChanged;
        SetBodyItemCount();

        Response = new ResponseViewModel();
        Response.Visibility = request.ResponseVisibility;
        IsBodyComboEnabled = request.IsBodyComboEnabled;
    }

    public void AddMethods()
    {
        Methods = new ObservableCollection<MethodsItemViewModel>();
        var getMethod = new MethodsItemViewModel() { Name = "GET", Foreground = "Green" };
        Methods.Add(getMethod);
        Methods.Add(new MethodsItemViewModel() { Name = "POST", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "PUT", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "PATCH", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "DELETE", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "OPTIONS", Foreground = "Blue" });
        Methods.Add(new MethodsItemViewModel() { Name = "HEAD", Foreground = "Blue" });

        SelectedMethod = getMethod;
    }

    public void AddNewParameter(bool isEnabled = false, string key = "", string value = "", string description = "", string deleteButtonVisibility = "Collapsed", string isKeyReadonly = "false", string isDescriptionReadyOnly = "false")
    {
        var Parameter = new ParameterItem(_messenger) { IsEnabled = isEnabled, Key = key, Value = value, Description = description, DeleteButtonVisibility = deleteButtonVisibility, IsKeyReadyOnly = isKeyReadonly, IsDescriptionReadyOnly = isDescriptionReadyOnly };
        Parameter.PropertyChanged += Parameter_PropertyChanged;
        Parameters.Add(Parameter);
        SetParameterCount();
    }

    private void SetParameterCount()
    {
        int count = 0;
        if (Parameters is not null)
        {
            foreach (var item in Parameters)
            {
                if (item.IsEnabled)
                    count += 1;
            }
        }

        if (count == 0)
            ParametersCount = " ";
        else
            ParametersCount = "(" + (count) + ")";
    }

    private void Parameter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as ParameterItem;
        int index = Parameters.IndexOf(item);

        item.PropertyChanged -= Parameter_PropertyChanged;

        if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;
        if (e.PropertyName == "IsEnabled")
            SetParameterCount();
        if (index == Parameters.Count - 1 && item.IsEnabled && !IsExistingRequest)
            AddNewParameter(false);

        if (!IsExistingRequest)
            item.DeleteButtonVisibility = "Visible";

        RefreshURL();

        item.PropertyChanged += Parameter_PropertyChanged;
    }


    public void DeleteParameterItem(ParameterItem item)
    {
        Parameters.Remove(item);
        SetParameterCount();
        RefreshURL();
    }

    public void DeleteHeaderItem(HeaderItem item)
    {
        Headers.Remove(item);
        SetHeaderCount();
    }

    public void DeleteBodyItem(BodyItem item)
    {
        Body.Remove(item);
        SetBodyItemCount();
    }

    public void AddNewHeader(string isKeyReadonly = "false", string isDescriptionReadyOnly = "false")
    {
        var Header = new HeaderItem(_messenger) { IsEnabled = false, Key = "", Value = "", Description = "", UTCVisibility = "Collapsed", DateTextboxVisibility = "Visible", DatePickerButtonVisibility = "Collapsed", HideDatePickerButtonVisibility = "Collapsed", DatePickerVisibility = "Collapsed", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = isKeyReadonly, IsDescriptionReadyOnly = isDescriptionReadyOnly };
        Header.PropertyChanged += Header_PropertyChanged;
        Headers.Add(Header);
        SetHeaderCount();
    }

    private void Header_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as HeaderItem;
        int index = Headers.IndexOf(item);

        item.PropertyChanged -= Header_PropertyChanged;

        if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;
        if (e.PropertyName == "IsEnabled")
            SetHeaderCount();
        if (index == Headers.Count - 1 && item.IsEnabled && !IsExistingRequest)
            AddNewHeader();


        if (e.PropertyName == "Key")
        {
            if (item.Key == "Date" || item.Key == "x-ms-date")
            {
                item.UTCVisibility = "Visible";
                item.DateTextboxVisibility = "Visible";
                item.DatePickerButtonVisibility = "Collapsed";
                item.DatePickerVisibility = "Collapsed";
            }
            else if (item.Key == "x-ms-version")
            {
                item.UTCVisibility = "Collapsed";
                item.DateTextboxVisibility = "Visible";
                item.DatePickerButtonVisibility = "Visible";
                item.DatePickerVisibility = "Collapsed";
            }
            else
            {
                item.UTCVisibility = "Collapsed";
                item.DateTextboxVisibility = "Visible";
                item.DatePickerButtonVisibility = "Collapsed";
                item.DatePickerVisibility = "Collapsed";
            }
        }

        if (!IsExistingRequest)
            item.DeleteButtonVisibility = "Visible";

        item.PropertyChanged += Header_PropertyChanged;
    }

    private void SetHeaderCount()
    {
        int count = 0;
        if (Headers is not null)
        {
            foreach (var item in Headers)
            {
                if (item.IsEnabled)
                    count += 1;
            }
        }

        if (count == 0)
            HeadersCount = " ";
        else
            HeadersCount = "(" + (count) + ")";
    }

    public void AddNewBodyItem(string isKeyReadonly = "false", string isDescriptionReadyOnly = "false")
    {
        var BodyItem = new BodyItem(_messenger) { IsEnabled = false, Key = "", Value = "", Description = "", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = isKeyReadonly, IsDescriptionReadyOnly = isDescriptionReadyOnly };
        BodyItem.PropertyChanged += BodyItem_PropertyChanged;
        Body.Add(BodyItem);
        SetBodyItemCount();
    }

    private void SetBodyItemCount()
    {
        int count = 0;

        if (Body is not null)
        {
            foreach (var item in Body)
            {
                if (item.IsEnabled)
                    count += 1;
            }
        }

        if (count == 0)
            BodyItemsCount = " ";
        else
            BodyItemsCount = "(" + (count) + ")";
    }

    private void BodyItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var item = sender as BodyItem;
        int index = Body.IndexOf(item);

        item.PropertyChanged -= BodyItem_PropertyChanged;

        if (!item.IsEnabled && e.PropertyName != "IsEnabled")
            item.IsEnabled = true;
        if (e.PropertyName == "IsEnabled")
            SetBodyItemCount();
        if (index == Body.Count - 1 && item.IsEnabled && !IsExistingRequest)
            AddNewBodyItem();

        if (!IsExistingRequest)
            item.DeleteButtonVisibility = "Visible";
        item.PropertyChanged += BodyItem_PropertyChanged;
    }

    public void SetMsDate(HeaderItem item, DateTimeOffset date)
    {
        item.Value = date.ToString("yyyy-MM-dd");
    }

     [RelayCommand]
    public async Task<string> SendRequestAsync()
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();

        var request = new HttpRequestMessage(new HttpMethod(SelectedMethod.Name), URL.RawURL);
        AddRequestHeaders(client);
        AddRequestBody(request);
        Response.HeadersCount = "";
        Stopwatch.Reset();
        Stopwatch.Start();
        try
        {
            response = await client.SendAsync(request);
            GetResponseStatusCode(response);
            await GetResponseMetadata(response);
            await GetResponseBody(response);
            GetResponseHeaders(response);

        }
        catch (Exception ex)
        {
            Response.StatusStyleKey = "MyStatusCodeErrorStyle";
            Response.StatusCode = ex.Message;
        }
        Stopwatch.Stop();

        Response.BannerVisibility = "Collapsed";
        Response.Visibility = "Visible";

        return Response.Body;
    }

    public string BeautifyXml(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        StringBuilder sb = new StringBuilder();
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace
        };

        using (XmlWriter writer = XmlWriter.Create(sb, settings))
        {
            doc.Save(writer);
        }

        return sb.ToString();
    }

    private void GetResponseHeaders(HttpResponseMessage response)
    {
        foreach (var item in response.Content.Headers)
        {
            if ((item.Key == "Content-Type") || (item.Key == "Content-Length" && item.Value != null) || (item.Key == "Expires" && item.Value != null))
            {
                foreach (var subitem in item.Value)
                    Response.Headers.Add(new ResponseHeaderItem() { Key = item.Key, Value = subitem.ToString() });
            }
        }

        foreach (var item in response.Headers)
        {
            foreach (var subitem in item.Value)
                Response.Headers.Add(new ResponseHeaderItem() { Key = item.Key, Value = subitem.ToString() });
        }

        foreach (var item in response.Headers)

            if (Response.Headers.Count == 0)
                Response.HeadersCount = " ";
            else
                Response.HeadersCount = "(" + (Response.Headers.Count) + ")";
    }

    private async Task GetResponseMetadata(HttpResponseMessage response)
    {
        var responseHeadersSize = await GetResponseHeadersSizeInKB(response);
        long? responseBodySize = response.Content.Headers.ContentLength;

        if (responseBodySize is not null)
            Response.Size = Math.Round((decimal)((responseHeadersSize + responseBodySize) / 1024.0), 2) + " KB";
        else
            Response.Size = Math.Round((decimal)(responseHeadersSize / 1024.0), 2) + " KB";

        Response.Time = Stopwatch.ElapsedMilliseconds + " ms";
        Response.Headers = new ObservableCollection<ResponseHeaderItem>();
    }

    private async Task GetResponseBody(HttpResponseMessage response)
    {
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrEmpty(responseBody))
        {
            switch (response.Content.Headers.ContentType.MediaType)
            {
                case "application/xml":
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseBody);
                    Response.Body = BeautifyXml(xmlDocument.OuterXml);
                    break;
                case "application/json":
                    JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                    var stream = new MemoryStream();
                    var writer = new Utf8JsonWriter(stream, new JsonWriterOptions() { Indented = true });
                    jsonDocument.WriteTo(writer);
                    writer.Flush();

                    Response.Body = Encoding.UTF8.GetString(stream.ToArray());
                    break;
                default:
                    Response.Body = "";
                    break;
            }
        }
        else
            Response.Body = "";
    }

    private void GetResponseStatusCode(HttpResponseMessage response)
    {
        if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299)
            Response.StatusStyleKey = "MyStatusCodeSuccessfulStyle";
        else if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 399)
            Response.StatusStyleKey = "MyStatusCodeWarningStyle";
        else if ((int)response.StatusCode >= 400)
            Response.StatusStyleKey = "MyStatusCodeErrorStyle";

        Response.StatusCode = ((int)response.StatusCode).ToString() + " " + response.ReasonPhrase;
    }

    private void AddRequestBody(HttpRequestMessage request)
    {
        switch (selectedBodyType)
        {
            case "Form":
                var form = new MultipartFormDataContent();
                foreach (BodyItem item in Body)
                {
                    if (item.IsEnabled)
                        form.Add(new StringContent(item.Value), item.Key);
                }
                request.Content = form;
                break;

            case "Json":
                request.Content = new StringContent(RawBody, Encoding.UTF8, "application/json");
                break;
            case "Xml":
                request.Content = new StringContent(RawBody, Encoding.UTF8, "application/xml");
                break;
            case "Text":
                request.Content = new StringContent(RawBody, Encoding.UTF8, "text/plain");
                break;
        }
    }

    private void AddRequestHeaders(HttpClient client)
    {
        foreach (HeaderItem item in Headers)
        {
            if (item.IsEnabled)
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
        }
    }

    public async Task<long> GetResponseHeadersSizeInKB(HttpResponseMessage response)
    {
        long totalSize = 0;
        foreach (var header in response.Headers)
        {
            totalSize += Encoding.UTF8.GetByteCount(header.Key);

            foreach (var value in header.Value)
                totalSize += Encoding.UTF8.GetByteCount(value);
        }

        return totalSize;
    }

    private void RefreshParameters()
    {
        if (isURLEditing)
        {
            int questionMarkIndex = URL.RawURL.IndexOf('?');
            List<List<string>> disabledParameters = new List<List<string>>();
            foreach (var parameter in Parameters)
            {
                if (parameter.IsEnabled == false)
                {
                    disabledParameters.Add(new List<string> { parameter.Key, parameter.Value, parameter.Description, parameter.DeleteButtonVisibility });
                }
            }

            Parameters.Clear();

            var rawParameters = URL.RawURL.Substring(questionMarkIndex + 1, URL.RawURL.Length - questionMarkIndex - 1);
            var parameterSplit = rawParameters.Split("&");

            if (parameterSplit.Length > 0)
            {
                for (int i = 0; i < parameterSplit.Length; i++)
                {
                    var equalsMarkIndex = parameterSplit[i].IndexOf("=");

                    if (parameterSplit[i] == "")
                        AddNewParameter(isEnabled: true, deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                    else if (equalsMarkIndex == -1)
                        AddNewParameter(isEnabled: true, key: parameterSplit[i], deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                    else
                        AddNewParameter(isEnabled: true, key: parameterSplit[i].Substring(0, equalsMarkIndex), value: parameterSplit[i].Substring(equalsMarkIndex + 1, parameterSplit[i].Length - equalsMarkIndex - 1), deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                }
            }

            foreach (var parameter in disabledParameters)
            {
                AddNewParameter(isEnabled: false, key: parameter[0], value: parameter[1], description: parameter[2], deleteButtonVisibility: parameter[3], isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
            }
            //disabledParameters = null;
        }
    }

    private void RefreshURL()
    {
        if (isParametersEditing)
        {
            int questionMarkIndex = URL.RawURL.IndexOf('?');
            var rawURL = "";
            if (questionMarkIndex == -1)
                rawURL = URL.RawURL;
            else
                rawURL = URL.RawURL.Substring(0, questionMarkIndex);

            var rawParameters = "";
            bool isFirstParameter = true;

            foreach (var item in Parameters)
            {
                if (item.IsEnabled)
                {
                    if (isFirstParameter)
                    {
                        if (!string.IsNullOrEmpty(item.Key) && string.IsNullOrEmpty(item.Value))
                            rawParameters += "?" + item.Key;
                        else if (string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                            rawParameters += "?" + "=" + item.Value;
                        else if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                            rawParameters += "?" + item.Key + "=" + item.Value;
                        else if (string.IsNullOrEmpty(item.Key) && string.IsNullOrEmpty(item.Value))
                            rawParameters += "?";

                        isFirstParameter = false;
                    }
                    else
                    {
                        if (Parameters.IndexOf(item) <= Parameters.Count - 1)
                        {
                            if (!string.IsNullOrEmpty(item.Key) && string.IsNullOrEmpty(item.Value))
                                rawParameters += "&" + item.Key;
                            else if (string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                                rawParameters += "&" + "=" + item.Value;
                            else if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                                rawParameters += "&" + item.Key + "=" + item.Value;
                            else if (string.IsNullOrEmpty(item.Key) && string.IsNullOrEmpty(item.Value))
                                rawParameters += "&";
                        }
                    }
                }
            }

            if (rawParameters == "?=")
                rawParameters = "";

            URL.RawURL = rawURL + rawParameters;
        }
    }

}
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

public partial class ParameterItem : ObservableRecipient
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
    public ParameterItem()
    {
    }

    public ParameterItem(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    partial void OnKeyChanged(string value)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.RefreshURL);
            _messenger.Send(message);
        }
    }

    partial void OnValueChanged(string value)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.RefreshURL);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void DeleteParameterItem(ParameterItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.DeleteParameterItem, parameterItem: item);
            _messenger.Send(message);
        }
    }
}

public partial class HeaderItem : ObservableRecipient
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
    [ObservableProperty]
    public string uTCVisibility = "Collapsed";
    [ObservableProperty]
    public string datePickerButtonVisibility = "Collapsed";
    [ObservableProperty]
    public string hideDatePickerButtonVisibility = "Collapsed";
    [ObservableProperty]
    public string dateTextboxVisibility = "Visible";
    [ObservableProperty]
    public string datePickerVisibility = "Collapsed";

    private readonly WeakReferenceMessenger _messenger;

    public HeaderItem()
    {
    }

    public HeaderItem(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    public void GetDateTimeInUTC(HeaderItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.GetDateTimeInUTC, headerItem: item);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void DeleteHeaderItem(HeaderItem item)
    {
        if (_messenger is not null)
        {
            var message = new RequestMessage(Command.DeleteHeaderItem, headerItem: item);
            _messenger.Send(message);
        }
    }

    [RelayCommand]
    public void ShowDatePickerItem(HeaderItem item)
    {
        item.DateTextboxVisibility = "Collapsed";
        item.DatePickerVisibility = "Visible";
        item.DatePickerButtonVisibility = "Collapsed";
        item.HideDatePickerButtonVisibility = "Visible";
    }

    [RelayCommand]
    public void HideDatePickerItem(HeaderItem item)
    {
        item.DateTextboxVisibility = "Visible";
        item.DatePickerVisibility = "Collapsed";
        item.DatePickerButtonVisibility = "Visible";
        item.HideDatePickerButtonVisibility = "Collapsed";
    }
}

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

public partial class TabItem : ObservableRecipient
{
    [ObservableProperty]
    public string title;
    [ObservableProperty]
    public string editingIconVisibility;
    [ObservableProperty]
    public SolidColorBrush foreground;
    [ObservableProperty]
    public string method;

}

public partial class TabsViewModel : ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<TabItem> tabs;
    [ObservableProperty]
    public TabItem selectedTabItem;

    public TabsViewModel()
    {
        tabs = new ObservableCollection<TabItem>();
    }
}

public class RequestMessage
{
    public Command Name
    {
        get;
    }
    public ParameterItem? ParameterItem
    {
        get;
    }
    public HeaderItem? HeaderItem
    {
        get;
    }
    public BodyItem? BodyItem
    {
        get;
    }

    public RequestMessage(Command commandName, ParameterItem? parameterItem = null, HeaderItem? headerItem = null, BodyItem? bodyItem = null)
    {
        Name = commandName;
        ParameterItem = parameterItem;
        HeaderItem = headerItem;
        BodyItem = bodyItem;
    }
}

public enum Command
{
    RefreshURL,
    RefreshParameters,
    DeleteParameterItem,
    GetDateTimeInUTC,
    ShowDatePickerItem,
    HideDatePickerItem,
    DeleteHeaderItem,
    DeleteBodyItem
}