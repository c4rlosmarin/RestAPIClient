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
                Name = "Filesystem - Create";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?resource=filesystem" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Optional. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-properties", Description = "User-defined properties to be stored with the filesystem, in the format of a comma-separated list of name and value pairs \"n1=v1, n2=v2, ...\", where each value is a base64 encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-default-encryption-scope", Description = "The encryption scope set as default on the filesystem.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "57dc4b51-a0d4-4917-8ba9-c7acde42cb71":
                Name = "Filesystem - Delete";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?resource=filesystem" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "DELETE");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Optional. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "e8f351eb-7408-4913-bf19-8002ea9da72d":
                Name = "Filesystem - Get Properties";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?resource=filesystem" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "HEAD");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "232cc021-77e1-4b8e-a7cd-88bd8d1434c7":
                Name = "Filesystem - List";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/?resource=account" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="account", Description="Required. The value must be \"account\" for all account operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "prefix", Description="Filters results to filesystems within the specified prefix.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "continuation", Description="Required. The value must be \"account\" for all account operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "maxResults", Description="An optional value that specifies the maximum number of items to return. If omitted or greater than 5,000, the response will include up to 5,000 items.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "e71bf0b4-b2eb-4325-a8f0-d51d402124c7":
                Name = "Filesystem - Set Properties";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?resource=filesystem" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PATCH");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-properties", Description = "Optional. User-defined properties to be stored with the filesystem, in the format of a comma-separated list of name and value pairs \"n1=v1, n2=v2, ...\", where each value is a base64 encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set. If the filesystem exists, any properties not included in the list will be removed. All properties are removed if the header is omitted. To merge new and existing properties, first get all existing properties and the current E-Tag, then make a conditional request with the E-Tag and include values for all properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "b418189f-0b7e-4e5d-a85f-5305eea05bd7":
                Name = "Path - Create";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "resource", Value="", Description="Required only for Create File and Create Directory. The value must be \"file\" or \"directory\".", IsEnabledActive="false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "continuation", Description="Optional. When renaming a directory, the number of paths that are renamed with each invocation is limited. If the number of paths to be renamed exceeds this limit, a continuation token is returned in this response header. When a continuation token is returned in the response, it must be specified in a subsequent invocation of the rename operation to continue renaming the directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "mode", Description="Optional. Valid only when namespace is enabled. This parameter determines the behavior of the rename operation. The value must be \"legacy\" or \"posix\", and the default value will be \"posix\".", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Cache-Control", Description = "Optional. The service stores this value and includes it in the \"Cache-Control\" response header for \"Read File\" operations for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Encoding", Description = "Optional. Specifies which content encodings have been applied to the file. This value is returned to the client when the \"Read File\" operation is performed.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Language", Description = "Optional. Specifies the natural language used by the intended audience for the file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Disposition", Description = "Optional. The service stores this value and includes it in the \"Content-Disposition\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-cache-control", Description = "Optional. The service stores this value and includes it in the \"Cache-Control\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-type", Description = "Optional. The service stores this value and includes it in the \"Content-Type\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-encoding", Description = "Optional. The service stores this value and includes it in the \"Content-Encoding\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-language", Description = "Optional. The service stores this value and includes it in the \"Content-Language\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-disposition", Description = "Optional. The service stores this value and includes it in the \"Content-Disposition\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-rename-source", Description = "An optional file or directory to be renamed. The value must have the following format: \"/{filesystem}/{path}\", or \"/{filesystem}/{path}?sastoken\" when using a SAS token. If \"x-ms-properties\" is specified, the properties will overwrite the existing properties; otherwise, the existing properties will be preserved. This value must be a URL percent-encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. A lease ID for the path specified in the URI. The path to be overwritten must have an active lease and the lease ID must match.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-lease-id", Description = "Optional for rename operations. A lease ID for the source path. The source path must have an active lease and the lease ID must match.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-properties", Description = "Optional. User-defined properties to be stored with the file or directory, in the format of a comma-separated list of name and value pairs \"n1=v1, n2=v2, ...\", where each value is a base64 encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-permissions", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. Sets POSIX access permissions for the file owner, the file owning group, and others. Each class may be granted read (4), write (2), or execute (1) permission. Both symbolic (rwxrw-rw-) and 4-digit octal notation (e.g. 0766) are supported. The sticky bit is also supported and in symbolic notation, its represented either by the letter t or T in the final character-place depending on whether the execution bit for the others category is set or unset respectively (e.g. rwxrw-rw- with sticky bit is represented as rwxrw-rwT. A rwxrw-rwx with sticky bit is represented as rwxrw-rwt), absence of t or T indicates sticky bit not set. In 4-digit octal notation, its represented by 1st digit (e.g. 1766 represents rwxrw-rw- with sticky bit and 0766 represents rwxrw-rw- without sticky bit). Invalid in conjunction with x-ms-acl.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-umask", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. When creating a file or directory and the parent folder does not have a default ACL, the umask restricts the permissions of the file or directory to be created. The resulting permission is given by p & ^u, where p is the permission and u is the umask. For example, if p is 0777 and u is 0057, then the resulting permission is 0720. The default permission is 0777 for a directory and 0666 for a file. The default umask is 0027. The umask must be specified in 4-digit octal notation (e.g. 0766).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-owner", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. Sets the owner of the file or directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-group", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. Sets the owning group of the file or directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-acl", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. Sets POSIX access control rights on files and directories. Each access control entry (ACE) consists of a scope, a type, a user or group identifier, and permissions in the format \"[scope:][type]:[id]:[permissions]\". The scope must be \"default\" to indicate the ACE belongs to the default ACL for a directory; otherwise scope is implicit and the ACE belongs to the access ACL. There are four ACE types: \"user\" grants rights to the owner or a named user, \"group\" grants rights to the owning group or a named group, \"mask\" restricts rights granted to named users and the members of groups, and \"other\" grants rights to all users not found in any of the other entries. The user or group identifier is omitted for entries of type \"mask\" and \"other\". The user or group identifier is also omitted for the owner and owning group. The permission field is a 3-character sequence where the first character is 'r' to grant read access, the second character is 'w' to grant write access, and the third character is 'x' to grant execute permission. If access is not granted, the '-' character is used to denote that the permission is denied. For example, the following ACL grants read, write, and execute rights to the file owner and john.doe@contoso, the read right to the owning group, and nothing to everyone else: \"user::rwx,user:john.doe@contoso:rwx,group::r--,other::---,mask=rwx\". Invalid in conjunction with x-ms-permissions.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Optional. Specify a proposed lease id if you want to acquire a lease during creation of a file or directory. A lease will be acquired with this lease ID if the creation is successful.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-option", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account and only supported on files. Specify one of the following expiry option if you want to set expiry time on a file while creation. \"RelativeToNow\" Set the expiry relative to the current time. User will pass the number of milliseconds elapsed from now. \"Absolute\" Absolute time in RFC 1123 Format. \"Neverexpire\" Set the file to never expire, expiry-time does not need to be specified with this option.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-time", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account and only supported on files. Specify the expiry time when to expire the file. Given as RFC 1123 HTTP Time String or number of milliseconds according to the expiry-option.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-match", Description = "Optional. An ETag value. Specify this header to perform the rename operation only if the source's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-none-match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the rename operation only if the source's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-modified-since", Description = "Optional. A date and time value. Specify this header to perform the rename operation only if the source has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-unmodified-since", Description = "Optional. A date and time value. Specify this header to perform the rename operation only if the source has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "Optional. The Base64-encoded AES-256 encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "Optional. The Base64-encoded SHA256 hash of the encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Optional. Specifies the algorithm to use for encryption. The value of this header must be AES256.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-context", Description = "Optional. Default is “Empty”. If the value is set it will set Blob / File system metadata. Max length- 1024. Valid only when Hierarchical Namespace is enabled for the account.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"} };
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "3c43a681-da19-4303-8388-70b82771cdea":
                Name = "Path - Delete";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "DELETE");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "recursive", Description="Required and valid only when the resource is a directory. If \"true\", all paths beneath the directory will be deleted. If \"false\" and the directory is non-empty, an error occurs.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "continuation", Description="Optional. When deleting a directory, the number of paths that are deleted with each invocation is limited. If the number of paths to be deleted exceeds this limit, a continuation token is returned in this response header. When a continuation token is returned in the response, it must be specified in a subsequent invocation of the delete operation to continue deleting the directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "paginated", Description="Optional and valid only if Hierarchical Namespace is enabled for the account and resource is a directory with \"recursive\" query parameter set to \"true\". For recursive directory deletion, the number of paths that could be deleted with each invocation is limited when the authorization mechanism used is ACL and the caller is a non-super user, as the default timeout is 30 seconds. When \"paginated\" query parameter is set to \"true\", the response header may contain \"x-ms-continuation\" if the above limit is hit. While sending the \"x-ms-continuation\" in the subsequnt request, \"paginated\" and \"recursive\" query parameter should be set to \"true\". When the response doesnt contain any \"x-ms-continuation\", recursive directory deletion is successful. The actual directory deletion happens only in the last invocation, the previous ones involve ACL checks in the server of the files and directories under the directory to be recursively deleted.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "The lease ID must be specified if there is an active lease.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "978153ac-94cd-4c35-8b1f-b8044109a7f9":
                Name = "Path - Get Properties";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "HEAD");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "action", Description="Optional. If the value is \"getStatus\" only the system defined properties for the path are returned. If the value is \"getAccessControl\" the access control list is returned in the response headers (Hierarchical Namespace must be enabled for the account), otherwise the properties are returned.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "upn", Description="Optional. Valid only when Hierarchical Namespace is enabled for the account. If \"true\", the user identity values returned in the x-ms-owner, x-ms-group, and x-ms-acl response headers will be transformed from Azure Active Directory Object IDs to User Principal Names. If \"false\", the values will be returned as Azure Active Directory Object IDs. The default value is false. Note that group and application Object IDs are not translated because they do not have unique friendly names.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "fsAction", Description="Required only for check access action. Valid only when Hierarchical Namespace is enabled for the account. File system operation read/write/execute in string form, matching regex pattern '[rwx-]{3}'", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If this header is specified, the operation will be performed only if both of the following conditions are met: i) the path's lease is currently active and ii) the lease ID specified in the request matches that of the path.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "Optional. The Base64-encoded AES-256 encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "Optional. The Base64-encoded SHA256 hash of the encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Optional. Specifies the algorithm to use for encryption. The value of this header must be AES256.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "351df3a9-3e1f-425e-af99-31d94888fad8":
                Name = "Path - Lease";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "POST");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-lease-action", Description = "Required. There are five lease actions: \"acquire\", \"break\", \"change\", \"renew\", and \"release\". Use \"acquire\" and specify the \"x-ms-proposed-lease-id\" and \"x-ms-lease-duration\" to acquire a new lease. Use \"break\" to break an existing lease. When a lease is broken, the lease break period is allowed to elapse, during which time no lease operation except break and release can be performed on the file. When a lease is successfully broken, the response indicates the interval in seconds until a new lease can be acquired. Use \"change\" and specify the current lease ID in \"x-ms-lease-id\" and the new lease ID in \"x-ms-proposed-lease-id\" to change the lease ID of an active lease. Use \"renew\" and specify the \"x-ms-lease-id\" to renew an existing lease. Use \"release\" and specify the \"x-ms-lease-id\" to release a lease.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-duration", Description = "The lease duration is required to acquire a lease, and specifies the duration of the lease in seconds. The lease duration must be between 15 and 60 seconds or -1 for infinite lease.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-break-period", Description = "The lease break period duration is optional to break a lease, and specifies the break period of the lease in seconds. The lease break duration must be between 0 and 60 seconds.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required when \"x-ms-lease-action\" is \"renew\", \"change\" or \"release\". For the renew and release actions, this must match the current lease ID.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Required when \"x-ms-lease-action\" is \"acquire\" or \"change\". A lease will be acquired with this lease ID if the operation is successful.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "b9bbf718-1be0-4d6e-b6ca-941af99a1383":
                Name = "Path - List";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}?recursive={recursive}&resource=filesystem" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "recursive", Value="{recursive}", Description="Required. If \"true\", all paths are listed; otherwise, only paths at the root of the filesystem are listed. If \"directory\" is specified, the list will only include paths that share the same root." , IsEnabledActive="false", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "resource", Value="filesystem", Description="Required. The value must be \"filesystem\" for all filesystem operations.", IsEnabledActive="false", IsValueReadyOnly="true" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "continuation", Description="The number of paths returned with each invocation is limited. If the number of paths to be returned exceeds this limit, a continuation token is returned in the response header x-ms-continuation. When a continuation token is returned in the response, it must be specified in a subsequent invocation of the list operation to continue listing the paths. Note that the continuation token returned in the response header x-ms-continuation must be URL encoded before being used in a subsequent invocation.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "directory", Description="Filters results to paths within the specified directory. An error occurs if the directory does not exist.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "maxResults", Description="An optional value that specifies the maximum number of items to return. If omitted or greater than 5,000, the response will include up to 5,000 items.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "upn", Description="Optional. Valid only when Hierarchical Namespace is enabled for the account. If \"true\", the user identity values returned in the owner and group fields of each list entry will be transformed from Azure Active Directory Object IDs to User Principal Names. If \"false\", the values will be returned as Azure Active Directory Object IDs. The default value is false. Note that group and application Object IDs are not translated because they do not have unique friendly names.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "fd373385-ac1d-49a0-bf38-e79ab98036a5":
                Name = "Path - Read";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Range", Description = "The HTTP Range request header specifies one or more byte ranges of the resource to be retrieved.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If this header is specified, the operation will be performed only if both of the following conditions are met: i) the path's lease is currently active and ii) the lease ID specified in the request matches that of the path.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-range-get-content-md5", Description = "Optional. When this header is set to \"true\" and specified together with the Range header, the service returns the MD5 hash for the range, as long as the range is less than or equal to 4MB in size. If this header is specified without the Range header, the service returns status code 400 (Bad Request). If this header is set to true when the range exceeds 4 MB in size, the service returns status code 400 (Bad Request).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "Optional. The Base64-encoded AES-256 encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "Optional. The Base64-encoded SHA256 hash of the encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Optional. Specifies the algorithm to use for encryption. The value of this header must be AES256.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "9f787102-8ff4-419b-af44-de620d842ebc":
                Name = "Path - Update";
                URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}?action={action}" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PATCH");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "action", Value="{action}", Description="Required. The action must be \"append\" to upload data to be appended to a file, \"flush\" to flush previously uploaded data to a file, \"setProperties\" to set the properties of a file or directory, or \"setAccessControl\" to set the owner, group, permissions, or access control list for a file or directory, or \"setAccessControlRecursive\" to set the access control list for a directory recursively. Note that Hierarchical Namespace must be enabled for the account in order to use access control. Also note that the Access Control List (ACL) includes permissions for the owner, owning group, and others, so the x-ms-permissions and x-ms-acl request headers are mutually exclusive.", IsEnabledActive="false", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "close", Description="Azure Storage Events allow applications to receive notifications when files change. When Azure Storage Events are enabled, a file changed event is raised. This event has a property indicating whether this is the final change to distinguish the difference between an intermediate flush to a file stream and the final close of a file stream. The close query parameter is valid only when the action is \"flush\" and change notifications are enabled. If the value of close is \"true\" and the flush operation completes successfully, the service raises a file change notification with a property indicating that this is the final update (the file stream has been closed). If \"false\" a change notification is raised indicating the file has changed. The default is false. This query parameter is set to true by the Hadoop ABFS driver to indicate that the file stream has been closed.\"" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "continuation", Description="Optional and valid only for \"setAccessControlRecursive\" operation. The number of paths processed with each invocation is limited. If the number of paths to be processed exceeds this limit, a continuation token is returned in the response header x-ms-continuation. When a continuation token is returned in the response, it must be percent-encoded and specified in a subsequent invocation of setAccessControlRecursive operation." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "flush", Description="Valid only for append calls. This parameter allows the caller to flush during an append call. Default value is \"false\" , if \"true\" the data will be flushed with the append call. Note that when using flush=true, the following headers are not supported - \"x-ms-cache-control\", \"x-ms-content-encoding\", \"x-ms-content-type\", \"x-ms-content-language\", \"x-ms-content-md5\", \"x-ms-content-disposition\". To set these headers during flush, please use action=flush" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "forceFlag", Description="Optional and valid only for \"setAccessControlRecursive\" operation. If this is \"false\" operation will terminate fast on encountering user errors (4XX). If \"true\" the api will ignore user errors and proceed with the operation on other sub-entities of the directory. Detailed status of user errors will be returned in the response for either scenario. Continuation token will only be returned when forceFlag is \"true\" in case of user errors. Default value for forceFlag is false." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "maxRecords", Description="Optional and valid only for \"setAccessControlRecursive\" operation. It specifies the maximum number of files or directories on which the acl change will be applied. If omitted or greater than 2,000, the request will process up to 2,000 items" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "mode", Description="Optional. Valid and required for \"setAccessControlRecursive\" operation. Mode \"set\" sets POSIX access control rights on files and directories, \"modify\" modifies one or more POSIX access control rights that pre-exist on files and directories, \"remove\" removes one or more POSIX access control rights that were present earlier on files and directories" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "position", Description="This parameter allows the caller to upload data in parallel and control the order in which it is appended to the file. It is required when uploading data to be appended to the file and when flushing previously uploaded data to the file. The value must be the position where the data is to be appended. Uploaded data is not immediately flushed, or written, to the file. To flush, the previously uploaded data must be contiguous, the position parameter must be specified and equal to the length of the file after all data has been written, and there must not be a request entity body included with the request." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "retainUncommittedData", Description="Valid only for flush operations. If \"true\", uncommitted data is retained after the flush operation completes; otherwise, the uncommitted data is deleted after the flush operation. The default is false. Data at offsets less than the specified position are written to the file when flush succeeds, but this optional parameter allows data after the flush position to be retained for a future flush operation." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="An optional operation timeout value in seconds. The period begins when the request is received by the service. If the timeout value elapses before the operation completes, the operation fails." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Length", Description = "Required for \"Append Data\" and \"Flush Data\". Must be 0 for \"Flush Data\". Must be the length of the request content in bytes for \"Append Data\".", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-MD5", Description = "Optional. An MD5 hash of the request content. This header is valid on \"Append\" and \"Flush\" operations. This hash is used to verify the integrity of the request content during transport. When this header is specified, the storage service compares the hash of the content that has arrived with this header value. If the two hashes do not match, the operation will fail with error code 400 (Bad Request). Note that this MD5 hash is not stored with the file. This header is associated with the request content, and not with the stored content of the file itself.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "The lease ID must be specified if there is an active lease. Invalid for \"setAccessControlRecursive\" operations.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-cache-control", Description = "Optional and only valid for flush and set properties operations. The service stores this value and includes it in the \"Cache-Control\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-type", Description = "Optional and only valid for flush and set properties operations. The service stores this value and includes it in the \"Content-Type\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-disposition", Description = "Optional and only valid for flush and set properties operations. The service stores this value and includes it in the \"Content-Disposition\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-encoding", Description = "Optional and only valid for flush and set properties operations. The service stores this value and includes it in the \"Content-Encoding\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-language", Description = "Optional and only valid for flush and set properties operations. The service stores this value and includes it in the \"Content-Language\" response header for \"Read File\" operations.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-md5", Description = "Optional and only valid for \"Flush and Set Properties\" operations. The service stores this value and includes it in the \"Content-Md5\" response header for \"Read and Get Properties\" operations. If this property is not specified on the request, then the property will be cleared for the file. Subsequent calls to \"Read and Get Properties\" will not return this property unless it is explicitly set on that file again.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-properties", Description = "Optional. User-defined properties to be stored with the file or directory, in the format of a comma-separated list of name and value pairs \"n1=v1, n2=v2, ...\", where each value is a base64 encoded string. Note that the string may only contain ASCII characters in the ISO-8859-1 character set. Valid only for the setProperties operation. If the file or directory exists, any properties not included in the list will be removed. All properties are removed if the header is omitted. To merge new and existing properties, first get all existing properties and the current E-Tag, then make a conditional request with the E-Tag and include values for all properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-owner", Description = "Optional and valid only for the setAccessControl operation. Sets the owner of the file or directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-group", Description = "Optional and valid only for the setAccessControl operation. Sets the owning group of the file or directory.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-permissions", Description = "Optional and only valid if Hierarchical Namespace is enabled for the account. Sets POSIX access permissions for the file owner, the file owning group, and others. Each class may be granted read (4), write (2), or execute (1) permission. Both symbolic (rwxrw-rw-) and 4-digit octal notation (e.g. 0766) are supported. The sticky bit is also supported and in symbolic notation, its represented either by the letter t or T in the final character-place depending on whether the execution bit for the others category is set or unset respectively (e.g. rwxrw-rw- with sticky bit is represented as rwxrw-rwT. A rwxrw-rwx with sticky bit is represented as rwxrw-rwt), absence of t or T indicates sticky bit not set. In 4-digit octal notation, its represented by 1st digit (e.g. 1766 represents rwxrw-rw- with sticky bit and 0766 represents rwxrw-rw- without sticky bit). Invalid in conjunction with x-ms-acl.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-acl", Description = "Optional and valid only for the setAccessControl and setAccessControlRecursive operations. Required for setAccessControlRecursive operation. Sets POSIX access control rights on files and directories. The value is a comma-separated list of access control entries that fully replaces the existing access control list (ACL) in case of setAccessControl and \"set\" mode of setAccessControlRecursive. \"modify\" mode of setAccessControlRecursive updates the pre-existing ACLS. Each access control entry (ACE) consists of a scope, a type, a user or group identifier, and permissions in the format \"[scope:][type]:[id]:[permissions]\". The scope must be \"default\" to indicate the ACE belongs to the default ACL for a directory; otherwise scope is implicit and the ACE belongs to the access ACL. There are four ACE types: \"user\" grants rights to the owner or a named user, \"group\" grants rights to the owning group or a named group, \"mask\" restricts rights granted to named users and the members of groups, and \"other\" grants rights to all users not found in any of the other entries. The user or group identifier is omitted for entries of type \"mask\" and \"other\". The user or group identifier is also omitted for the owner and owning group. The permission field is a 3-character sequence where the first character is 'r' to grant read access, the second character is 'w' to grant write access, and the third character is 'x' to grant execute permission. If access is not granted, the '-' character is used to denote that the permission is denied. For example, the following ACL grants read, write, and execute rights to the file owner and john.doe@contoso, the read right to the owning group, and nothing to everyone else: \"user::rwx,user:john.doe@contoso:rwx,group::r--,other::---,mask=rwx\". Invalid in conjunction with x-ms-permissions. \"remove\" mode of setAccessControlRecursive removes the pre-existing ACLs and should not contain permissions in the access control list specified : \"user:john.doe@contoso:, mask:\". \"set\" mode of setAccessControlRecursive sets the ACLs replacing the pre-existing ACLs of the scope specified and must contain all three - owning user, owning group and other info if access scope is getting set or if either of owning user, owning group or other is being set in default scope. \"set\" and \"modify\" modes of setAccessControlRecursive must contain permissions as part of access control list.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional for Flush Data, Set Access Control and Set Properties, but invalid for Append Data and Set Access Control Recursive. An ETag value. Specify this header to perform the operation only if the resource's ETag matches the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional for Flush Data, Set Access Control and Set Properties, but invalid for Append Data and Set Access Control Recursive. An ETag value or the special wildcard (\"*\") value. Specify this header to perform the operation only if the resource's ETag does not match the value specified. The ETag must be specified in quotes.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional for Flush Data and Set Properties, but invalid for Append Data and Set Access Control Recursive. A date and time value. Specify this header to perform the operation only if the resource has been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional for Flush Data and Set Properties, but invalid for Append Data and Set Access Control Recursive. A date and time value. Specify this header to perform the operation only if the resource has not been modified since the specified date and time.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "Optional. The Base64-encoded AES-256 encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "Optional. The Base64-encoded SHA256 hash of the encryption key.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Optional. Specifies the algorithm to use for encryption. The value of this header must be AES256.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-action", Description = "Starting with version 2020-08-04 in append and flush operations. Append supports 'acquire', 'auto-renew' and 'acquire-release' action. If 'acquire' it will acquire the lease. If 'auto-renew' it will renew the lease. If 'acquire-release' it will acquire & complete the operation & release the lease once operation is done. 'Release' action is only supported in flush operation. If 'true', will release the lease on the file using the lease id info from x-ms-lease-id header.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-duration", Description = "The lease duration is required to acquire a lease, and specifies the duration of the lease in seconds. The lease duration must be between 15 and 60 seconds or -1 for infinite lease.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Required when \"x-ms-lease-action\" is \"acquire\" or \"change\". A lease will be acquired with this lease ID if the operation is successful.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "true";
                SelectedBodyType = "Text";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

        }

        string jsonString = JsonSerializer.Serialize(this);
        Debug.WriteLine(jsonString);

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