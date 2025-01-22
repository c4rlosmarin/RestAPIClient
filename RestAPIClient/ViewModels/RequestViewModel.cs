using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RestAPIClient.Helpers;
using RestAPIClient.Models;

namespace RestAPIClient.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string requestId;
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string tabIconVisibility;
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
    public string deleteColumnVisibility;
    [ObservableProperty]
    public ResponseViewModel response;
    public bool IsExistingRequest;
    private WeakReferenceMessenger _messenger;

    private Stopwatch Stopwatch = new();

    public RequestViewModel()
    {

    }

    public void Initialize(string? requestId = null, RequestItem? request = null)
    {
        this.AddMethods();
        this.AddBodyTypes();

        if (IsExistingRequest)
        {
            _messenger.Register<RequestViewModel, RequestMessage>(this, (r, m) =>
            {
                var message = m as RequestMessage;
                switch (message.Name)
                {
                    case Command.RefreshURL:
                        RefreshURL();
                        break;
                    case Command.GetDateTimeInUTC:
                        message.HeaderItem.Value = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'");
                        break;
                    case Command.RefreshParameters:
                        RefreshParameters();
                        break;
                }
            });

            InitializeExistingRequest(request);
        }
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
        DeleteColumnVisibility = "Visible";
        Name = "Untitled request";
        TabIconVisibility = "Visible";
        SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
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
        DeleteColumnVisibility = "Collapsed";
        var templateHelper = new TemplateHelper();
        var requestModel = templateHelper.GetExistingRequestFromTemplate(request.RequestId);

        if (requestModel is not null)
        {
            Name = requestModel.Name;
            URL = requestModel.URL;
            SelectedMethod = requestModel.SelectedMethod;
            IsMethodComboEnabled = requestModel.IsMethodComboEnabled;
            TabIconVisibility = requestModel.TabIconVisibility;
            Parameters = requestModel.Parameters;
            Headers = requestModel.Headers;
            Body = requestModel.Body;
            IsBodyComboEnabled = requestModel.IsBodyComboEnabled;
            SelectedBodyType = requestModel.SelectedBodyType;
            RawBody = requestModel.RawBody;
            Response = requestModel.Response;
        }

        foreach (MethodsItemViewModel item in Methods)
        {
            if (item.Name == SelectedMethod.Name)
                SelectedMethod = item;
        }

        if (Parameters is not null)
        {
            foreach (ParameterItem item in Parameters)
                item.PropertyChanged += Parameter_PropertyChanged;

            if (Parameters.Count > 0)
            {
                isParametersEditing = true;
                RefreshURL();
                isParametersEditing = false;
            }
        }
        SetParameterCount();

        if (Headers is not null)
        {
            foreach (HeaderItem item in Headers)
                item.PropertyChanged += Header_PropertyChanged;
        }
        SetHeaderCount();

        if (Body is not null)
        {
            foreach (BodyItem item in Body)
                item.PropertyChanged += BodyItem_PropertyChanged;
        }
        SetBodyItemCount();
    }

    public void AddMethods()
    {
        Methods = new ObservableCollection<MethodsItemViewModel>();
        var getMethod = new MethodsItemViewModel("GET");
        Methods.Add(getMethod);
        Methods.Add(new MethodsItemViewModel("POST"));
        Methods.Add(new MethodsItemViewModel("PUT"));
        Methods.Add(new MethodsItemViewModel("PATCH"));
        Methods.Add(new MethodsItemViewModel("DELETE"));
        Methods.Add(new MethodsItemViewModel("OPTIONS"));
        Methods.Add(new MethodsItemViewModel("HEAD"));

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

        //var requestModel = new RequestModel();
        //requestModel.Name = Name;
        //requestModel.URL = URL;
        //requestModel.SelectedMethod = SelectedMethod;
        //requestModel.IsMethodComboEnabled = "false";
        //requestModel.TabIconVisibility = "Collapsed";
        //requestModel.Parameters = Parameters;
        //requestModel.Headers = Headers;
        //requestModel.Body = Body;
        //requestModel.IsBodyComboEnabled = "false";
        //requestModel.SelectedBodyType = SelectedBodyType;
        //requestModel.RawBody = RawBody;

        //string jsonString = JsonSerializer.Serialize(requestModel);
        //Debug.WriteLine(jsonString);

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
                    disabledParameters.Add(new List<string> { parameter.Key, parameter.Value, parameter.Description, parameter.DeleteButtonVisibility });
            }

            Parameters.Clear();

            if (questionMarkIndex != -1)
            {
                var rawParameters = URL.RawURL.Substring(questionMarkIndex + 1, URL.RawURL.Length - questionMarkIndex - 1);
                var parameterSplit = rawParameters.Split("&");

                if (parameterSplit.Length > 0)
                {
                    foreach (var item in parameterSplit)
                    {
                        var equalsSplit = item.Split('=');
                        var equalsMarkIndex = item.IndexOf("=");

                        if (equalsSplit.Length == 1)
                        {
                            if (equalsMarkIndex == -1)
                                AddNewParameter(isEnabled: true, key: equalsSplit[0], deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                            else
                                AddNewParameter(isEnabled: true, key: equalsSplit[0], value: equalsSplit[1], deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                        }
                        else
                        {
                            if (equalsMarkIndex != -1)
                                AddNewParameter(isEnabled: true, key: equalsSplit[0], value: equalsSplit[1], deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
                        }
                    }
                }

                foreach (var parameter in disabledParameters)
                    AddNewParameter(isEnabled: false, key: parameter[0], value: parameter[1], description: parameter[2], deleteButtonVisibility: parameter[3], isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
            }
            else
                AddNewParameter(isEnabled: false, deleteButtonVisibility: IsExistingRequest ? "Collapsed" : "Visible", isKeyReadonly: IsExistingRequest ? "True" : "False", isDescriptionReadyOnly: IsExistingRequest ? "True" : "False");
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