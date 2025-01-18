using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

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

    private void GetRequestFromTemplate(string RestAPIName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "RestAPIClient.Templates." + RestAPIName + ".json";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream is not null)
        {
            using StreamReader reader = new StreamReader(stream);
            var jsonString = reader.ReadToEnd();

            //TODO: Create a new "entity" to avoid using RequestViewModel
            RequestViewModel request = JsonSerializer.Deserialize<RequestViewModel>(jsonString);

            if (request is not null)
            {
                Name = request.Name;
                URL = request.URL;
                SelectedMethod = request.SelectedMethod;
                IsMethodComboEnabled = request.IsMethodComboEnabled;
                TabIconVisibility = request.TabIconVisibility;
                Parameters = request.Parameters;
                Headers = request.Headers;
                Body = request.Body;
                IsBodyComboEnabled = request.IsBodyComboEnabled;
                Response = request.Response;
                Response.Visibility = request.Response.Visibility;
            }
        }
    }

    public void InitializeExistingRequest(RequestItem request)
    {
        DeleteColumnVisibility = "Collapsed";
        switch (request.RequestId)
        {
            case "975b53b7-f48f-4682-8434-893f5a324278":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.ListContainers");
                break;
            case "e192945c-7d70-49c6-8e50-521a2f7f01c2":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.SetBlobServiceProperties");                
                break;

            case "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.GetBlobServiceProperties");
                break;

            case "e4e5c062-82a1-4282-a571-19d5acdec6d3":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.PreflightBlobRequest");
                break;

            case "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.GetBlobServiceStats");
                break;

            case "30e6c366-35d0-47e5-9a5c-def2ee2cfe31":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.GetAccountInformation");
                break;

            case "53d9941c-dcc4-48b8-a717-24064108ecda":
                GetRequestFromTemplate("BlobService.OperationsOnTheAccount.GetUserDelegationKey");
                break;

            case "042368e5-65ec-4a73-8462-86b65be8c353":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.CreateContainer");
                break;

            case "18fd3171-d359-4e1e-bd48-44ea44ee842c":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.GetContainerProperties");
                break;

            case "2117dd4c-e41f-4c15-b5c3-2b266ca3c225":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.GetContainerMetadata");
                break;

            case "78f4583c-ced5-4b21-98a6-e82690abd782":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.SetContainerMetadata");
                break;

            case "4944ad75-ad15-4ae2-bfd7-79d8051c427c":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.GetContainerACL");
                break;

            case "9f93eb42-cc6b-414e-8156-c39828f8d2d3":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.SetContainerACL");
                break;

            case "7661e957-a0ba-4188-a53d-4decd8672dac":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.DeleteContainer");
                break;

            case "7a2948eb-9751-4f53-b7cc-272284108344":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.LeaseContainer");
                break;

            case "76dbc6cf-f1ba-49af-ac15-b0447114d933":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.RestoreContainer");
                break;

            case "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.ListBlobs");
                break;

            case "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92":
                GetRequestFromTemplate("BlobService.OperationsOnContainers.FindBlobsByTagsInContainer");
                break;

            case "1fd54291-9b89-4348-8f08-85ecce5114ae":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.PutBlob");
                break;

            case "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.PutBlobFromURL");
                break;

            case "18e8d3cf-77b7-471c-b868-782f49afdaf5":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.GetBlob");
                break;

            case "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.GetBlobProperties");
                break;

            case "186bbfdd-abcb-4003-9efe-6504986bcff5":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobProperties");
                break;

            case "d3a570a7-c5bc-4a20-ac53-29d8631e8445":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.GetBlobMetadata");
                break;

            case "1a971155-adfa-47eb-b739-5aa04551e13c":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobMetadata");
                break;

            case "d992088f-86b7-4268-a697-a1e7ec719fb4":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.GetBlobTags");
                break;

            case "b2dde2c7-86ec-4667-a5db-8c75118f175a":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobTags");
                break;

            case "a1bcea4c-3c36-40ba-b133-b841d6a5bf06":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.FindBlobsByTags");
                break;

            case "061c4c88-981e-482f-bbd8-356f3594484c":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.LeaseBlob");
                break;

            case "bf386510-9394-4720-8769-b712a6ea08fb":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SnapshotBlob");
                break;

            case "8f5e0e97-744c-4218-bdd3-8f45ea545e95":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.CopyBlob");
                break;

            case "9487af8f-941f-40c0-8671-98e8a461d0ad":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.CopyBlobFromURL");
                break;

            case "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.AbortCopyBlob");
                break;

            case "faee6c9b-b52c-4ccd-844d-1425f2b81663":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.DeleteBlob");
                break;

            case "799b6e8b-3aab-4521-9507-2fec5b7e894a":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.UndeleteBlob");
                break;

            case "16744da0-054b-4fea-baee-2a8c0e4ee36f":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobTier");
                break;

            case "27400b89-4193-4804-9c97-8fc98bd4ec73":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.BlobBatch");
                break;

            case "965b3e05-3adc-4413-ba39-cf82ab6e26db":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobImmutabilityPolicy");
                break;

            case "7e7c8aba-d49d-4de2-bb62-d1507cf00030":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.DeleteBlobImmutabilityPolicy");
                break;

            case "2548c991-90c9-4159-98a8-c6d076787b01":
                GetRequestFromTemplate("BlobService.OperationsOnBlobs.SetBlobLegalHold");
                break;

            case "6ccb4aba-4209-4192-aa87-a0c24f9e26bf":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnFileSystem.Create");
                break;

            case "57dc4b51-a0d4-4917-8ba9-c7acde42cb71":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnFileSystem.Delete");
                break;

            case "e8f351eb-7408-4913-bf19-8002ea9da72d":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnFileSystem.GetProperties");
                break;

            case "232cc021-77e1-4b8e-a7cd-88bd8d1434c7":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnFileSystem.List");
                break;

            case "e71bf0b4-b2eb-4325-a8f0-d51d402124c7":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnFileSystem.SetProperties");
                break;

            case "b418189f-0b7e-4e5d-a85f-5305eea05bd7":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.Create");
                break;

            case "3c43a681-da19-4303-8388-70b82771cdea":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.Delete");
                break;

            case "978153ac-94cd-4c35-8b1f-b8044109a7f9":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.GetProperties");
                break;

            case "351df3a9-3e1f-425e-af99-31d94888fad8":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.Lease");
                break;

            case "b9bbf718-1be0-4d6e-b6ca-941af99a1383":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.List");
                break;

            case "fd373385-ac1d-49a0-bf38-e79ab98036a5":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.Read");
                break;

            case "9f787102-8ff4-419b-af44-de620d842ebc":
                GetRequestFromTemplate("DataLakeStorageGen2.OperationsOnPath.Update");
                break;

                //Name = "Filesystem - Set Properties";
                //URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?resource=filesystem" };
                //SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PATCH");
                //IsMethodComboEnabled = "false";
                //TabIconVisibility = "Collapsed";
                //Parameters = new ObservableCollection<ParameterItem>() {
                //        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                //Headers = new ObservableCollection<HeaderItem>() {
                //        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "x-ms-properties", Description = "Optional. User-defined properties to be stored with the filesystem, in the format of a comma-separated list of name and value pairs \"n1=v1, n2=v2, ...\", where each value is a base64 encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set. If the filesystem exists, any properties not included in the list will be removed. All properties are removed if the header is omitted. To merge new and existing properties, first get all existing properties and the current E-Tag, then make a conditional request with the E-Tag and include values for all properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                //IsBodyComboEnabled = "false";
                //SelectedBodyType = "None";

                //Response = new ResponseViewModel();
                //Response.Visibility = "Collapsed";

        }

        //string jsonString = JsonSerializer.Serialize(this);
        //Debug.WriteLine(jsonString);

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