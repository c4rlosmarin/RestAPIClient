using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

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
        switch (request.RequestId)
        {
            case "975b53b7-f48f-4682-8434-893f5a324278":
                Name = "List Containers";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                new(_messenger) { IsEnabled = true, Key = "comp", Value="list", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "prefix", Description = "Optional. Filters the results to return only containers with a name that begins with the specified prefix." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the list of containers to be returned with the next listing operation. The operation returns the NextMarker value within the response body, if the listing operation didn't return all containers remaining to be listed with the current page. You can use the NextMarker value as the value for the marker parameter in a subsequent call to request the next page of list items. The marker value is opaque to the client." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of containers to return. If the request doesn't specify maxresults, or specifies a value greater than 5000, the server will return up to 5000 items.Note that if the listing operation crosses a partition boundary, then the service will return a continuation token for retrieving the remainder of the results. For this reason, it's possible that the service will return fewer results than specified by maxresults, or than the default of 5000.If the parameter is set to a value less than or equal to zero, the server returns status code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "include", Description="Optional. Specifies one or more datasets to include in the response:-metadata: Note that metadata requested with this parameter must be stored in accordance with the naming restrictions imposed by the 2009-09-19 version of Blob Storage. Beginning with this version, all metadata names must adhere to the naming conventions for C# identifiers.-deleted: Version 2019-12-12 and later. Specifies that soft-deleted containers should be included in the response.-system: Version 2020-10-02 and later. Specifies if system containers are to be included in the response. Including this option will list system containers, such as $logs and $changefeed. Note that the specific system containers returned will vary, based on which service features are enabled on the storage account.",DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>(){
                new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Body = new ObservableCollection<BodyItem>();
                IsBodyComboEnabled = "false";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "e192945c-7d70-49c6-8e50-521a2f7f01c2":
                Name = "Set Blob Service Properties";
                URL = new URL() { RawURL = "https://account-name.blob.core.windows.net/" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="service", IsEnabledActive="false", IsValueReadyOnly="true", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="properties", IsEnabledActive="false", IsValueReadyOnly="true", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "Xml";
                RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<StorageServiceProperties>  \r\n    <Logging>  \r\n        <Version>version-number</Version>  \r\n        <Delete>true|false</Delete>  \r\n        <Read>true|false</Read>  \r\n        <Write>true|false</Write>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </Logging>  \r\n    <HourMetrics>  \r\n        <Version>version-number</Version>  \r\n        <Enabled>true|false</Enabled>  \r\n        <IncludeAPIs>true|false</IncludeAPIs>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </HourMetrics>  \r\n    <MinuteMetrics>  \r\n        <Version>version-number</Version>  \r\n        <Enabled>true|false</Enabled>  \r\n        <IncludeAPIs>true|false</IncludeAPIs>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </MinuteMetrics>  \r\n    <Cors>  \r\n        <CorsRule>  \r\n            <AllowedOrigins>comma-separated-list-of-allowed-origins</AllowedOrigins>  \r\n            <AllowedMethods>comma-separated-list-of-HTTP-verbs</AllowedMethods>  \r\n            <MaxAgeInSeconds>max-caching-age-in-seconds</MaxAgeInSeconds>  \r\n            <ExposedHeaders>comma-separated-list-of-response-headers</ExposedHeaders>  \r\n            <AllowedHeaders>comma-separated-list-of-request-headers</AllowedHeaders>  \r\n        </CorsRule>  \r\n    </Cors>    \r\n    <DefaultServiceVersion>default-service-version-string</DefaultServiceVersion>\r\n    <DeleteRetentionPolicy>\r\n        <Enabled>true|false</Enabled>\r\n        <Days>number-of-days</Days>\r\n    </DeleteRetentionPolicy>\r\n    <StaticWebsite>\r\n        <Enabled>true|false</Enabled>\r\n        <IndexDocument>default-name-of-index-page-under-each-directory</IndexDocument>\r\n        <DefaultIndexDocumentPath>absolute-path-of-the-default-index-page</DefaultIndexDocumentPath>\r\n        <ErrorDocument404Path>absolute-path-of-the-custom-404-page</ErrorDocument404Path>\r\n    </StaticWebsite>\r\n</StorageServiceProperties>";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94":
                Name = "Get Blob Service Properties";
                URL = new URL() { RawURL = "https://<account-name>.blob.core.windows.net/?restype=service&comp=properties" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="service", IsEnabledActive="false", IsValueReadyOnly="true", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="properties", IsEnabledActive="false", IsValueReadyOnly="true", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "e4e5c062-82a1-4282-a571-19d5acdec6d3":
                Name = "Preflight Blob Request";
                URL = new URL() { RawURL = "http://<account-name>.blob.core.windows.net/<blob-resource>" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "OPTIONS");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Origin", Description = "Required. Specifies the origin from which the request will be issued. The origin is checked against the service's CORS rules to determine the success or failure of the preflight request.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Access-Control-Request-Method", Description = "Required. Specifies the method (or HTTP verb) for the request. The method is checked against the service's CORS rules to determine the failure or success of the preflight request.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Access-Control-Request-Headers", Description = "Optional. Specifies the request headers that will be sent. If it's not present, the service assumes that the request doesn't include headers.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17":
                Name = "Get Blob Service Stats";
                URL = new URL() { RawURL = "https://myaccount-secondary.blob.core.windows.net/?restype=service&comp=stats" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                    new(_messenger) { IsEnabled = true, Key = "restype", Value="service", IsEnabledActive="false", IsValueReadyOnly="true", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                    new(_messenger) { IsEnabled = true, Key = "comp", Value="properties", IsEnabledActive="false", IsValueReadyOnly="true", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                    new(_messenger) { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" } };
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "30e6c366-35d0-47e5-9a5c-def2ee2cfe31":
                Name = "Get Account Information";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/?restype=account&comp=properties" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="service", IsEnabledActive="false", IsValueReadyOnly="true", Description="Required. The restype parameter value must be account.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="properties", IsEnabledActive="false", IsValueReadyOnly="true", Description = "Required. The comp parameter value must be properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "53d9941c-dcc4-48b8-a717-24064108ecda":
                Name = "Get User Delegation Key";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/?restype=service&comp=userdelegationkey" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "POST");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="service", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="userdelegationkey", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "Xml";
                RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<KeyInfo>  \r\n    <Start>String, formatted ISO Date</Start>\r\n    <Expiry>String, formatted ISO Date </Expiry>\r\n</KeyInfo>";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "042368e5-65ec-4a73-8462-86b65be8c353":
                Name = "Create Container";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. A name-value pair to associate with the container as metadata. Note: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-public-access", Description = "Optional. Specifies whether data in the container can be accessed publicly and the level of access. Possible values include:\r\n\r\n- container: Specifies full public read access for container and blob data. Clients can enumerate blobs within the container via anonymous request, but they can't enumerate containers within the storage account.\r\n- blob: Specifies public read access for blobs. Blob data within this container can be read via anonymous request, but container data isn't available. Clients can't enumerate blobs within the container via anonymous request.\r\n\r\nIf this header isn't included in the request, container data is private to the account owner.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "18fd3171-d359-4e1e-bd48-44ea44ee842c":
                Name = "Get Container Properties";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container Properties succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID does not match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "2117dd4c-e41f-4c15-b5c3-2b266ca3c225":
                Name = "Get Container Metadata";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=metadata" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="metadata", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container Metadata succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID doesn't match, error code 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "78f4583c-ced5-4b21-98a6-e82690abd782":
                Name = "Set Container Metadata";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=metadata" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="metadata", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Set Container Metadata succeeds only if the container's lease is active and matches this ID. If there's no active lease or the ID doesn't match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name:value", Description = "Optional. A name-value pair to associate with the container as metadata.\r\n\r\nEach call to this operation replaces all existing metadata that's attached to the container. To remove all metadata from the container, call this operation with no metadata headers.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "4944ad75-ad15-4ae2-bfd7-79d8051c427c":
                Name = "Get Container ACL";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=acl" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="acl", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container ACL succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID does not match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "9f93eb42-cc6b-414e-8156-c39828f8d2d3":
                Name = "Set Container ACL";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=acl" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="acl", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-public-access", Description = "Optional. Specifies whether data in the container may be accessed publicly and the level of access. Possible values include:\r\n\r\n- container: Specifies full public read access for container and blob data. Clients can enumerate blobs within the container via anonymous request, but can't enumerate containers within the storage account.\r\n- blob: Specifies public read access for blobs. Blob data within this container can be read via anonymous request, but container data isn't available. Clients can't enumerate blobs within the container via anonymous request.\r\n\r\nIf this header isn't included in the request, container data is private to the account owner.\r\n\r\nNote that setting public access for a container in an Azure Premium Storage account isn't permitted.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Set Container ACL succeeds only if the container's lease is active and matches this ID. If there's no active lease or the ID doesn't match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "Xml";
                RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<SignedIdentifiers>  \r\n  <SignedIdentifier>   \r\n    <Id>unique-64-character-value</Id>  \r\n    <AccessPolicy>  \r\n      <Start>start-time</Start>  \r\n      <Expiry>expiry-time</Expiry>  \r\n      <Permission>abbreviated-permission-list</Permission>  \r\n    </AccessPolicy>  \r\n  </SignedIdentifier>  \r\n</SignedIdentifiers>";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "7661e957-a0ba-4188-a53d-4decd8672dac":
                Name = "Delete Container";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "DELETE");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-lease-id", Description = "Required for version 2012-02-12 and later if the container has an active lease. To call Delete Container on a container that has an active lease, specify the lease ID in this header. If this header isn't specified when there is an active lease, Delete Container returns a 409 (Conflict) error. If you specify the wrong lease ID, or a lease ID on a container that doesn't have an active lease, Delete Container returns a 412 (Precondition failed) error.", IsEnabledActive = "false", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "7a2948eb-9751-4f53-b7cc-272284108344":
                Name = "Lease Container";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?comp=lease&restype=container" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Optional. Specifies the version of the operation to use for this request", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required to renew, change, or release the lease.\r\n\r\nYou can specify the value of x-ms-lease-id in any valid GUID string format. See Guid Constructor (String) for a list of valid formats.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-action", Description = "acquire: Requests a new lease. If the container doesn't have an active lease, Blob Storage creates a lease on the container, and returns a new lease ID. If the container has an active lease, you can only request a new lease by using the active lease ID. You can, however, specify a new x-ms-lease duration, including negative one (-1) for a lease that never expires.\r\n\r\nrenew: Renews the lease. You can renew the lease if the lease ID specified on the request matches that associated with the container. Note that the lease can be renewed even if it has expired, as long as the container hasn't been leased again since the expiration of that lease. When you renew a lease, the lease duration clock resets.\r\n\r\nchange: Change the lease ID of an active lease. A change must include the current lease ID in x-ms-lease-id, and a new lease ID in x-ms-proposed-lease-id.\r\n\r\nrelease: Release the lease. You can release the lease if the lease ID specified on the request matches that associated with the container. Releasing the lease allows another client to immediately acquire the lease for the container, as soon as the release is complete.\r\n\r\nbreak: Break the lease, if the container has an active lease. After a lease is broken, it can't be renewed. Any authorized request can break the lease. The request isn't required to specify a matching lease ID. When a lease is broken, the lease break period is allowed to elapse. You can only perform break and release lease operations on the container during this time. When a lease is successfully broken, the response indicates the interval in seconds until a new lease can be acquired.\r\n\r\nA lease that has been broken can also be released. A client can immediately acquire a container lease that has been released.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-break-period", Description = "Optional. For a break operation, this header is the proposed duration that the lease should continue before it's broken, between 0 and 60 seconds. This break period is only used if it's shorter than the time remaining on the lease. If longer, the time remaining on the lease is used. A new lease won't be available before the break period has expired, but the lease can be held for longer than the break period. If this header doesn't appear with a break operation, a fixed-duration lease breaks after the remaining lease period elapses, and an infinite lease breaks immediately.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-duration", Description = "Required for acquire. Specifies the duration of the lease, in seconds, or negative one (-1) for a lease that never expires. A non-infinite lease can be between 15 and 60 seconds. A lease duration can't be changed by using renew or change.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional for acquire, and required for change. Proposed lease ID, in a GUID string format. Blob Storage returns 400 (Invalid request) if the proposed lease ID isn't in the correct format.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "76dbc6cf-f1ba-49af-ac15-b0447114d933":
                Name = "Restore Container";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/destinationcontainer?restype=container&comp=undelete" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", Description="Required. The restype parameter value must be container.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="undelete", IsEnabledActive="false", IsValueReadyOnly="true", Description="Required. The comp parameter value must be undelete.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For this operation, the version must be 2018-03-28 or later", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-deleted-container-name", Description = "Required. You use this header to uniquely identify the soft-deleted container that should be restored.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-deleted-container-version", Description = "Required. You use this header to uniquely identify the soft-deleted container that should be restored. You can obtain this value from specifying the deleted value in the include query parameter of the List Containers operation.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e":
                Name = "List Blobs";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=list" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="list", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "prefix", Description="Optional. Filters the results to return only blobs with names that begin with the specified prefix. In accounts that have a hierarchical namespace, an error will occur in cases where the name of a file appears in the middle of the prefix path. For example, you might attempt to find blobs that are named readmefile.txt by using the prefix path folder1/folder2/readme/readmefile.txt. An error will appear if any subfolder contains a file named readme." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "delimiter", Description = "Optional. When the request includes this parameter, the operation returns a BlobPrefix element in the response body. This element acts as a placeholder for all blobs with names that begin with the same substring, up to the appearance of the delimiter character. The delimiter can be a single character or a string.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the list to be returned with the next list operation. The operation returns a marker value within the response body if the list returned was not complete. You can then use the marker value in a subsequent call to request the next set of list items.\r\n\r\nThe marker value is opaque to the client.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of blobs to return, including all BlobPrefix elements. If the request doesn't specify maxresults, or specifies a value greater than 5,000, the server will return up to 5,000 items. If there are additional results to return, the service returns a continuation token in the NextMarker response element. In certain cases, the service might return fewer results than specified by maxresults, and also return a continuation token.\r\n\r\nSetting maxresults to a value less than or equal to zero results in error response code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "include", Description = "Optional. Specifies one or more datasets to include in the response:\r\n\r\n- snapshots: Specifies that snapshots should be included in the enumeration. Snapshots are listed from oldest to newest in the response.\r\n- metadata: Specifies that blob metadata be returned in the response.\r\n- uncommittedblobs: Specifies that blobs for which blocks have been uploaded, but which haven't been committed by using Put Block List, be included in the response.\r\n- copy: Version 2012-02-12 and later. Specifies that metadata related to any current or previous Copy Blob operation should be included in the response.\r\n-deleted: Version 2017-07-29 and later. Specifies that soft-deleted blobs should be included in the response.\r\n-tags: Version 2019-12-12 and later. Specifies that user-defined, blob index tags should be included in the response.\r\n-versions: Version 2019-12-12 and later. Specifies that versions of blobs should be included in the enumeration.\r\n-deletedwithversions: Version 2020-10-02 and later. Specifies that deleted blobs with any versions (active or deleted) should be included in the response. Items that you've permanently deleted appear in the response until they are processed by garbage collection. Use the tag \\<HasVersionsOnly\\>, and the value true.\r\n-immutabilitypolicy: Version 2020-06-12 and later. Specifies that the enumeration should include the immutability policy until date, and the immutability policy mode of the blobs.\r\n-legalhold: Version 2020-06-12 and later. Specifies that the enumeration should include the legal hold of blobs.\r\n-permissions: Version 2020-06-12 and later. Supported only for accounts with a hierarchical namespace enabled. If a request includes this parameter, then the owner, group, permissions, and access control list for the listed blobs or directories will be included in the enumeration.\r\n\r\nTo specify more than one of these options on the URI, you must separate each option with a URL-encoded comma (\"%82\").", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "showonly", Description = "Optional. Specifies one of these datasets to be returned in the response:\r\n\r\n-deleted: Optional. Version 2020-08-04 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains soft-deleted blobs. Note that POSIX ACL authorization fallback is not supported for listing soft deleted blobs. If include=deleted is also specified, the request fails with Bad Request (400).\r\n-files: Optional. Version 2020-12-06 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains files.\r\n-directories: Optional. Version 2020-12-06 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains directories.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For this operation, the version must be 2018-03-28 or later", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-upn", Description = "Optional. Valid only when a hierarchical namespace is enabled for the account, and include=permissions is provided in the request. If true, the user identity values returned in the <Owner>, <Group>, and <Acl> fields are transformed from Microsoft Entra object IDs to user principal names. If false, the values are returned as Microsoft Entra object IDs. The default value is false. Note that group and application object IDs aren't translated because they don't have unique friendly names.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92":
                Name = "Find Blobs by Tags in Container";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=blobs&where=expression" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "restype", Value="container", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="blobs", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "where", Value="expression", Description="Required. Filters the result set to include only blobs whose tags match the specified expression." , IsEnabledActive="false", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the result set to be returned with the next operation. The operation returns a marker value within the response body if the returned result set was not complete. The marker value can then be used in a subsequent call to request the next set of items.\r\n\r\nThe marker value is opaque to the client.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of blobs to return. If the request doesn't specify maxresults or specifies a value greater than 5,000, the server returns up to 5,000 items. If there are additional results to return, the service returns a continuation token in the NextMarker response element. In certain cases, the service might return fewer results than maxresults specifies but still return a continuation token.\r\n\r\nSetting maxresults to a value less than or equal to zero results in error response code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description = "Optional. Expressed in seconds.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests, but optional for anonymous requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "1fd54291-9b89-4348-8f08-85ecce5114ae":
                Name = "Put Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request.\r\n\r\nFor a page blob or an append blob, the value of this header must be set to zero, because Put Blob is used only to initialize the blob. To write content to an existing page blob, call Put Page. To write content to an append blob, call Append Block.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-blob-type", Description = "<BlockBlob ¦ PageBlob ¦ AppendBlob> Required. Specifies the type of blob to create: block blob, page blob, or append blob. Support for creating an append blob is available only in version 2015-02-21 and later.", IsEnabledActive = "false",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Type", Description = "Optional. The MIME content type of the blob. The default type is application/octet-stream.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Encoding", Description = "Optional. Specifies which content encodings have been applied to the blob. This value is returned to the client when the Get Blob operation is performed on the blob resource. When this value is returned, the client can use it to decode the blob content.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Language", Description = "Optional. Specifies the natural languages that are used by this resource.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-MD5", Description = "Optional. An MD5 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request).\r\n\r\nWhen the header is omitted in version 2012-02-12 or later, Blob Storage generates an MD5 hash.\r\n\r\nResults from Get Blob, Get Blob Properties, and List Blobs include the MD5 hash.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-crc64", Description = "Optional. A CRC64 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request). This header is supported in versions 02-02-2019 and later.\r\n\r\nIf both Content-MD5 and x-ms-content-crc64 headers are present, the request fails with a 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Cache-Control", Description = "Optional. Blob Storage stores this value but doesn't use or modify it.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-type", Description = "Optional. Set the blob’s content type.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-encoding", Description = "Optional. Set the blob’s content encoding.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-language", Description = "Optional. Set the blob's content language.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. Set the blob’s MD5 hash. For BlockBlob, this header takes precedence over Content-MD5 when verifying the integrity of the blob during transport. For PageBlob and AppendBlob, this header directly sets the MD5 hash of the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-cache-control", Description = "Optional. Sets the blob's cache control.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Name-value pairs associated with the blob as metadata.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope to use to encrypt the request contents. This header is supported in versions 2019-02-02 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-context", Description = "Optional. Default is “Empty”. If the value is set it will set blob system metadata. Max length-1024. Valid only when Hierarchical Namespace is enabled for the account. This header is supported in versions 2021-08-06 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets the given query-string encoded tags on the blob. See the Remarks for additional information. Supported in version 2019-12-12 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-disposition", Description = "Optional. Sets the blob’s Content-Disposition header. Available for versions 2013-08-15 and later.\r\n\r\nThe Content-Disposition response header field conveys additional information about how to process the response payload, and you can use it to attach additional metadata. For example, if the header is set to attachment, it indicates that the user-agent should not display the response. Instead, it should display a Save As dialog with a file name other than the specified blob name.\r\n\r\nThe response from the Get Blob and Get Blob Properties operations includes the content-disposition header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the analytics logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. The tier to be set on the blob. For page blobs on a Premium Storage account only with version 2017-04-17 and later. For a full list of page blob-supported tiers, see High-performance premium storage and managed disks for virtual machines (VMs). For block blobs, supported on blob storage or general purpose v2 accounts only with version 2018-11-09 and later. Valid values for block blob tiers are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. ",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-immutability-policy-until-date", Description = "Version 2020-06-12 and later. Specifies the retention-until date to be set on the blob. This is the date until which the blob can be protected from being modified or deleted. Follows RFC1123 format.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-immutability-policy-mode", Description = "Version 2020-06-12 and later. Specifies the immutability policy mode to be set on the blob. Valid values are unlocked and locked. With unlocked, users can change the policy by increasing or decreasing the retention-until date. With locked, these actions are prohibited.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-legal-hold", Description = "Version 2020-06-12 and later. Specifies the legal hold to be set on the blob. Valid values are true and false.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-option", Description = "Optional. Version 2023-08-03 and later. Specifies the expiration date option for the request. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-time", Description = "Optional. Version 2023-08-03 and later. Specifies the time when the blob is set to expire. The format for expiration date varies according to x-ms-expiry-option. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-length", Description = "Required for page blobs. This header specifies the maximum size for the page blob, up to 8 tebibytes (TiB). The page blob size must be aligned to a 512-byte boundary.\r\n\r\nIf this header is specified for a block blob or an append blob, Blob Storage returns status code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-sequence-number", Description = "Optional. Set for page blobs only. The sequence number is a user-controlled value that you can use to track requests. The value of the sequence number must be from 0 to 2^63 - 1. The default value is 0.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-access-tier", Description = "Version 2017-04-17 and later. For page blobs on a premium storage account only. Specifies the tier to be set on the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "This header can be used to troubleshoot requests and corresponding responses. The value of this header is equal to the value of the x-ms-client-request-id header if it's present in the request and the value contains no more than 1,024 visible ASCII characters. If the x-ms-client-request-id header isn't present in the request, it won't be present in the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Body = new ObservableCollection<BodyItem>();
                IsBodyComboEnabled = "true";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e":
                Name = "Put Blob From URL";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request.\r\n\r\nFor a page blob or an append blob, the value of this header must be set to zero, because Put Blob is used only to initialize the blob. To write content to an existing page blob, call Put Page. To write content to an append blob, call Append Block.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-copy-source", Description = "Required. Specifies the URL of the source blob. The value may be a URL of up to 2 kibibytes (KiB) in length that specifies a blob. The value should be URL-encoded as it would appear in a request URI. The source blob must either be public or be authorized via a shared access signature. If the source blob is public, no authorization is required to perform the operation. If the size of the source blob is greater than 5000 MiB, or if the source does not return a valid Content-Length value, the request fails with the status code 409 (Conflict). Here are some examples of source object URLs:\r\n\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?snapshot=<DateTime>\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?versionid=<DateTime>", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-blob-type", Description = "Required. Specifies the type of blob to create, which must be BlockBlob. If the blob type isn't BlockBlob, the operation fails with status code 400 (Bad Request).", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-copy-source-authorization", Description = "Optional. Specifies the authorization scheme and signature for the copy source. For more information, see Authorize requests to Azure Storage.\r\nOnly scheme bearer is supported for Microsoft Entra.\r\nThis header is supported in version 2020-10-02 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Type", Description = "Optional. The MIME content type of the blob. The default type is application/octet-stream.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Encoding", Description = "Optional. Specifies which content encodings have been applied to the blob. This value is returned to the client when the Get Blob operation is performed on the blob resource. When this value is returned, the client can use it to decode the blob content.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-Language", Description = "Optional. Specifies the natural languages that are used by this resource.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Cache-Control", Description = "Optional. Blob Storage stores this value but doesn't use or modify it.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. An MD5 hash of the blob content from the URI. This hash is used to verify the integrity of the blob during transport of the data from the URI. When this header is specified, the storage service compares the hash of the content that has arrived from the copy source with this header value. If this header is omitted, Blob Storage generates an MD5 hash.\r\n\r\nIf the two hashes don't match, the operation fails with error code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-crc64", Description = "Optional. A CRC64 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request). This header is supported in version 02-02-2019 and later.\r\n\r\nIf both Content-MD5 and x-ms-content-crc64 headers are present, the request fails with a 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-type", Description = "Optional. Set the blob’s content type.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-encoding", Description = "Optional. Set the blob’s content encoding.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-language", Description = "Optional. Set the blob's content language.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. Sets the blob's MD5 hash.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-cache-control", Description = "Optional. Sets the blob's cache control.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. The name-value pairs that are associated with the blob as metadata.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. The encryption scope to use to encrypt the request contents. This header is supported in version 2019-02-02 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets the specified query-string-encoded tags on the blob. For more information, go to the Remarks section. Supported in version 2019-12-12 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-copy-source-tag-option", Description = "Optional. Possible values are REPLACE or COPY (case-sensitive). The default value is REPLACE.\r\n\r\nIf COPY is specified, the tags from the source blob are copied to the destination blob. The source blob must be private, and the request must have permission to Get Blob Tags on the source blob and Set Blob Tags on the destination blob. This incurs an extra call to the Get Blob Tags operation on the source account.\r\n\r\nREPLACE sets tags that are specified by the x-ms-tags header on the destination blob. If REPLACE is used and no tags are specified by x-ms-tags, no tags are set on the destination blob. Specifying COPY and x-ms-tags results in a 409 (Conflict).\r\n\r\nSupported in version 2021-04-10 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-copy-source-blob-properties", Description = "Optional. Specifies the copy source blob properties behavior. If set to True, the properties of the source blob will be copied to the new blob. The default value is True.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-modified-since", Description = "Optional. A DateTime value. Specify this conditional header to put the blob only if the source blob has been modified since the specified date/time. If the source blob hasn't been modified, Blob Storage returns status code 412 (Precondition Failed). This header can't be specified if the source is an Azure Files share.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-unmodified-since", Description = "Optional. A DateTime value. Specify this conditional header to put the blob only if the source blob hasn't been modified since the specified date/time. If the source blob has been modified, Blob Storage returns status code 412 (Precondition Failed). This header can't be specified if the source is an Azure Files share.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-match", Description = "Optional. An ETag value. Specify this conditional header to put the source blob only if its ETag matches the specified value. If the ETag values don't match, Blob Storage returns status code 412 (Precondition Failed). This header can't be specified if the source is an Azure Files share.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-none-match", Description = "Optional. An ETag value. Specify this conditional header to put the blob only if its ETag doesn't match the specified value. If the values are identical, Blob Storage returns status code 412 (Precondition Failed). This header can't be specified if the source is an Azure Files share.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A DateTime value. Specify this conditional header to put the blob only if the destination blob has been modified since the specified date/time. If the destination blob hasn't been modified, Blob Storage returns status code 412 (Precondition Failed).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A DateTime value. Specify this conditional header to put the blob only if the destination blob hasn't been modified since the specified date/time. If the destination blob has been modified, Blob Storage returns status code 412 (Precondition Failed).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify an ETag value for this conditional header to put the blob only if the specified ETag value matches the ETag value for an existing destination blob. If the ETag for the destination blob doesn't match the ETag specified for If-Match, Blob Storage returns status code 412 (Precondition Failed).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value, or the wildcard character (*).\r\n\r\nSpecify an ETag value for this conditional header to put the blob only if the specified ETag value doesn't match the ETag value for the destination blob.\r\n\r\nSpecify the wildcard character (*) to perform the operation only if the destination blob doesn't exist.\r\n\r\nIf the specified condition isn't met, Blob Storage returns status code 412 (Precondition Failed).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-disposition", Description = "Optional. Sets the blob’s Content-Disposition header. Available for version 2013-08-15 and later.\r\n\r\nThe Content-Disposition response header field conveys additional information about how to process the response payload, and it can be used to attach additional metadata. For example, if the header is set to attachment, it indicates that the user-agent shouldn't display the response. Instead, it should display a Save As dialog with a file name other than the specified blob name.\r\n\r\nThe response from the Get Blob and Get Blob Properties operations includes the content-disposition header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response. For more information, see CORS support for the Azure Storage services.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the analytics logs when storage analytics logging is enabled. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. Indicates the tier to be set on the blob. Valid values for block blob tiers are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. Hot, Cool, and Archive are supported for version 2018-11-09 and later. For more information about block blob tiering, see Hot, cool, and archive storage tiers.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-option", Description = "Optional. Version 2023-08-03 and later. Specifies the expiration date option for the request. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-expiry-time", Description = "Optional. Version 2023-08-03 and later. Specifies the time when the blob is set to expire. The format for expiration date varies according to x-ms-expiry-option. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Body = new ObservableCollection<BodyItem>();
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "18e8d3cf-77b7-471c-b868-782f49afdaf5":
                Name = "Get Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when it's present, specifies the blob snapshot to be retrieved. For more information about working with blob snapshots, see Create a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional, version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to be retrieved.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Range", Description = "Optional. Return the bytes of the blob only in the specified range.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-range", Description = "Optional. Return the bytes of the blob only in the specified range. If both Range and x-ms-range are specified, the service uses the value of x-ms-range. If neither range is specified, the entire blob contents are returned. For more information, see Specify the range header for Blob Storage operations.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If this header is specified, the operation is performed only if both of the following conditions are met:\r\n\r\n- The blob's lease is currently active.\r\n- The lease ID that's specified in the request matches the lease ID of the blob.\r\n\r\nIf this header is specified but either of these conditions isn't met, the request fails and the Get Blob operation fails with status code 412 (Precondition Failed).", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-range-get-content-md5", Description = "Optional. When this header is set to true and specified together with the Range header, the service returns the MD5 hash for the range, as long as the range is less than or equal to 4 mebibytes (MiB) in size.\r\n\r\nIf the header is specified without the Range header, the service returns status code 400 (Bad Request).\r\n\r\nIf the header is set to true when the range exceeds 4 MiB, the service returns status code 400 (Bad Request).", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-range-get-content-crc64", Description = "Optional. When this header is set to true and specified together with the Range header, the service returns the CRC64 hash for the range, as long as the range is less than or equal to 4 MiB in size.\r\n\r\nIf the header is specified without the Range header, the service returns status code 400 (Bad Request).\r\n\r\nIf the header is set to true when the range exceeds 4 MiB, the service returns status code 400 (Bad Request).\r\n\r\nIf both the x-ms-range-get-content-md5 and x-ms-range-get-content-crc64 headers are present, the request fails with a 400 (Bad Request).\r\n\r\nThis header is supported in versions 2019-02-02 and later.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-upn", Description = "Optional. Version 2023-11-03 and later. Valid for accounts with hierarchical namespace enabled. If true, the user identity values that are returned in the x-ms-owner, x-ms-group and x-ms-acl response headers will be transformed from Microsoft Entra object IDs to User Principal Names. If the value is false, they're returned as Microsoft Entra object IDs. The default value is false. Note that group and application object IDs are not translated, because they don't have unique friendly names.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit, which is recorded in the analytics logs when storage analytics logging is enabled. We highly recommend that you use this header when you're correlating client-side activities with requests that are received by the server. For more information, see About Azure Storage Analytics logging.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd":
                Name = "Get Blob Properties";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "HEAD");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when it's present, specifies the blob snapshot to retrieve. For more information about working with blob snapshots, see Create a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional. Version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when it's present, specifies the version of the blob to retrieve.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Optional for anonymous requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If this header is specified, the Get Blob Properties operation is performed only if both of the following conditions are met:\r\n\r\n- The blob's lease is currently active.\r\n- The lease ID that's specified in the request matches the lease ID of the blob.\r\n\r\nIf either of these conditions isn't met, the request fails, and the Get Blob Properties operation fails with status code 412 (Precondition Failed).", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-upn", Description = "Optional. Version 2020-06-12 and later. Valid for accounts with hierarchical namespace enabled. If true, the user identity values that are returned in the x-ms-owner, x-ms-group and x-ms-acl response headers will be transformed from Microsoft Entra object IDs to User Principal Names. If the value is false, they're returned as Microsoft Entra object IDs. The default value is false. Note that group and application object IDs are not translated, because they don't have unique friendly names.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit, which is recorded in the analytics logs when storage analytics logging is enabled. We highly recommend that you use this header when you're correlating client-side activities with requests that are received by the server. For more information, see About Azure Storage Analytics logging.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "186bbfdd-abcb-4003-9efe-6504986bcff5":
                Name = "Set Blob Properties";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="properties", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Optional for anonymous requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-cache-control", Description = "Optional. Modifies the cache control string for the blob.\r\n\r\nIf this property isn't specified on the request, the property is cleared for the blob. Subsequent calls to Get Blob Properties don't return this property, unless it's explicitly set on the blob again.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-type", Description = "Optional. Sets the blob’s content type.\r\n\r\nIf this property isn't specified on the request, the property is cleared for the blob. Subsequent calls to Get Blob Properties don't return this property, unless it's explicitly set on the blob again.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. Sets the blob's MD5 hash.\r\n\r\nIf this property isn't specified on the request, the property is cleared for the blob. Subsequent calls to Get Blob Properties don't return this property, unless it's explicitly set on the blob again.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-encoding", Description = "Optional. Sets the blob's content encoding.\r\n\r\nIf this property isn't specified on the request, the property is cleared for the blob. Subsequent calls to Get Blob Properties don't return this property, unless it's explicitly set on the blob again.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-language", Description = "Optional. Sets the blob's content language.\r\n\r\nIf this property isn't specified on the request, the property is cleared for the blob. Subsequent calls to Get Blob Properties don't return this property, unless it's explicitly set on the blob again.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-disposition", Description = "Optional. Sets the blob’s Content-Disposition header. Available for version 2013-08-15 and later.\r\n\r\nThe Content-Disposition response header field conveys additional information about how to process the response payload, and it can be used to attach additional metadata. For example, if it's set to attachment, it indicates that the user-agent shouldn't display the response, but instead show a Save As dialog with a file name other than the specified blob name.\r\n\r\nThe response from the Get Blob and Get Blob Properties operations includes the content-disposition header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing headers on the response. For more information, see CORS (cross-origin resource sharing) support for the Azure Storage services.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-content-length", Description = "Optional. Resizes a page blob to the specified size. If the specified value is less than the current size of the blob, all page blobs with values that are greater than the specified value are cleared.\r\n\r\nThis property can't be used to change the size of a block blob or an append blob. Setting this property for a block blob or an append blob returns status code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-sequence-number-action", Description = "Optional, but required if the x-ms-blob-sequence-number header is set for the request. This property applies to page blobs only.\r\n\r\nThis property indicates how the service should modify the blob's sequence number. Specify one of the following options for this property:\r\n\r\n- max: Sets the sequence number to be the higher of the value included with the request and the value currently stored for the blob.\r\n- update: Sets the sequence number to the value that's included with the request.\r\n- increment: Increments the value of the sequence number by 1. If you're specifying this option, don't include the x-ms-blob-sequence-number header. Doing so returns status code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-blob-sequence-number", Description = "Optional, but required if the x-ms-sequence-number-action property is set to max or update. This property applies to page blobs only.\r\n\r\nThe property sets the blob's sequence number. The sequence number is a user-controlled property that you can use to track requests and manage concurrency issues. For more information, see the Put Page operation.\r\n\r\nUse this property together with x-ms-sequence-number-action to update the blob's sequence number to either the specified value or the higher of the values specified with the request or currently stored with the blob. This header should not be specified if x-ms-sequence-number-action is set to increment, in which case the service automatically increments the sequence number by one.\r\n\r\nTo set the sequence number to a value of your choosing, this property must be specified on the request together with x-ms-sequence-number-action.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "d3a570a7-c5bc-4a20-ac53-29d8631e8445":
                Name = "Get Blob Metadata";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=metadata" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="metadata", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when present, specifies the blob snapshot to retrieve. For more information about working with blob snapshots, see Create a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional. Version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to retrieve.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Optional for anonymous requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If this header is specified, the Get Blob Metadata operation is performed only if both of the following conditions are met:\r\n\r\n- The blob's lease is currently active.\r\n- The lease ID that's specified in the request matches the lease ID of the blob.\r\n\r\nIf either of these conditions is not met, the request fails, and the Get Blob Metadata operation fails with status code 412 (Precondition Failed).", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "1a971155-adfa-47eb-b739-5aa04551e13c":
                Name = "Set Blob Metadata";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=metadata" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="metadata", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set time-outs for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Sets a name-value pair for the blob.\r\n\r\nEach call to this operation replaces all existing metadata that's attached to the blob. To remove all metadata from the blob, call this operation with no metadata headers.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope to use to encrypt the request contents. This header is supported in version 2019-02-02 or later.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "d992088f-86b7-4268-a697-a1e7ec719fb4":
                Name = "Get Blob Tags";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=tags" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="tags", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when it's present, specifies the blob snapshot to retrieve. For more information about working with blob snapshots, see Create a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when it's present, specifies the version of the blob to be retrieved.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Optional for anonymous requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease.\r\n\r\nTo perform this operation on a blob with an active lease, specify the valid lease ID for this header. If a valid lease ID isn't specified on the request, the operation fails with status code 403 (Forbidden).", IsEnabledActive = "false", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "b2dde2c7-86ec-4667-a5db-8c75118f175a":
                Name = "Set Blob Tags";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=tags" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="tags", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to retrieve.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set time-outs for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request content in bytes. This header refers to the content length of the tags document, not of the blob itself.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Type", Description = "Required. The value of this header should be application/xml; charset=UTF-8.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease.\r\n\r\nTo perform this operation on a blob with an active lease, specify the valid lease ID for this header. If a valid lease ID isn't specified on the request, the operation fails with status code 403 (Forbidden).", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Content-MD5", Description = "Optional. An MD5 hash of the request content. This hash is used to verify the integrity of the request content during transport. If the two hashes don't match, the operation fails with error code 400 (Bad Request).\r\n\r\nThis header is associated with the request content, and not with the content of the blob itself.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-content-crc64", Description = "Optional. A CRC64 hash of the request content. This hash is used to verify the integrity of the request content during transport. If the two hashes don't match, the operation fails with error code 400 (Bad Request).\r\n\r\nThis header is associated with the request content, and not with the content of the blob itself.\r\n\r\nIf both Content-MD5 and x-ms-content-crc64 headers are present, the request fails with error code 400 (Bad Request).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "Xml";
                RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<Tags>  \r\n    <TagSet>  \r\n        <Tag>  \r\n            <Key>tag-name-1</Key>  \r\n            <Value>tag-value-1</Value>  \r\n        </Tag>  \r\n        <Tag>  \r\n            <Key>tag-name-2</Key>  \r\n            <Value>tag-value-2</Value>  \r\n        </Tag>  \r\n    </TagSet>  \r\n</Tags>";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "a1bcea4c-3c36-40ba-b133-b841d6a5bf06":
                Name = "Find Blobs by Tags";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net?comp=blobs&where=<expression>" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "GET");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="blobs", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "where", Value="<expression>", Description="Required. Filters the result set to include only blobs whose tags match the specified expression.\r\n\r\nFor more information on how to construct this expression, see Remarks.", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "marker", Description="Optional. A string value that identifies the portion of the result set to be returned with the next operation. The operation returns a marker value within the response body if the returned result set was not complete. The marker value can then be used in a subsequent call to request the next set of items.\r\n\r\nThe marker value is opaque to the client.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "maxresults", Description="Optional. Specifies the maximum number of blobs to return. If the request doesn't specify maxresults or specifies a value greater than 5,000, the server returns up to 5,000 items. If there are additional results to return, the service returns a continuation token in the NextMarker response element. In certain cases, the service might return fewer results than maxresults specifies. The service might also return a continuation token.\r\n\r\nSetting maxresults to a value less than or equal to zero results in error response code 400 (Bad Request).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. Expressed in seconds. For more information, see Set timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests, but optional for anonymous requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "061c4c88-981e-482f-bbd8-356f3594484c":
                Name = "Lease Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=lease" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="lease", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-lease-id", Description = "Required to renew, change, or release the lease.\r\n\r\nYou can specify the value of x-ms-lease-id in any valid GUID string format. See Guid Constructor (String) for a list of valid formats.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Optional. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-action", Description = "acquire: Requests a new lease. If the blob doesn't have an active lease, Blob Storage creates a lease on the blob, and returns a new lease ID. If the blob has an active lease, you can only request a new lease by using the active lease ID. You can, however, specify a new x-ms-lease-duration, including negative one (-1) for a lease that never expires.\r\n\r\nrenew: Renews the lease. You can renew the lease if the lease ID specified on the request matches that associated with the blob. Note that the lease can be renewed even if it has expired, as long as the blob hasn't been modified or leased again since the expiration of that lease. When you renew a lease, the lease duration clock resets.\r\n\r\nchange: Version 2012-02-12 and later. Changes the lease ID of an active lease. A change must include the current lease ID in x-ms-lease-id, and a new lease ID in x-ms-proposed-lease-id.\r\n\r\nrelease: Releases the lease. You can release the lease if the lease ID specified on the request matches that associated with the blob. Releasing the lease allows another client to immediately acquire the lease for the blob, as soon as the release is complete.\r\n\r\nbreak: Breaks the lease, if the blob has an active lease. After a lease is broken, it can't be renewed. Any authorized request can break the lease; the request isn't required to specify a matching lease ID. When a lease is broken, the lease break period is allowed to elapse, during which time break and release are the only lease operations you can perform on the blob. When a lease is successfully broken, the response indicates the interval in seconds until a new lease can be acquired.\r\n\r\nA lease that has been broken can also be released, in which case another client can immediately acquire the lease on the blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-break-period", Description = "Optional. Version 2012-02-12 and later. For a break operation, this is the proposed duration of seconds that the lease should continue before it is broken, between 0 and 60 seconds. This break period is only used if it's shorter than the time remaining on the lease. If longer, the time remaining on the lease is used. A new lease will not be available before the break period has expired, but the lease can be held for longer than the break period. If this header doesn't appear with a break operation, a fixed-duration lease breaks after the remaining lease period elapses, and an infinite lease breaks immediately.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-duration", Description = "Version 2012-02-12 and later. Only allowed and required on an acquire operation. Specifies the duration of the lease, in seconds, or negative one (-1) for a lease that never expires. A non-infinite lease can be between 15 and 60 seconds. A lease duration can't be changed by using renew or change.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Version 2012-02-12 and later. Optional for acquire, and required for change. Proposed lease ID, in a GUID string format. Blob Storage returns 400 (Invalid request) if the proposed lease ID isn't in the correct format. See Guid Constructor (String) for a list of valid formats.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response. See CORS support for the Storage services for details.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "bf386510-9394-4720-8769-b712a6ea08fb":
                Name = "Snapshot Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=snapshot" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="snapshot", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Specifies a user-defined, name-value pair associated with the blob. If you don't specify any name-value pairs, the operation copies the base blob metadata to the snapshot. If you specify one or more name-value pairs, the snapshot is created with the specified metadata, and metadata isn't copied from the base blob.\r\n\r\nNote that beginning with version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers. See Naming and referencing containers, blobs, and metadata for more information.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A DateTime value. Specify this conditional header to take a snapshot of the blob, only if it has been modified since the specified date/time. If the base blob hasn't been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A DateTime value. Specify this conditional header to take a snapshot of the blob, only if it hasn't been modified since the specified date/time. If the base blob has been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify an ETag value for this conditional header to take a snapshot of the blob, only if its ETag value matches the value specified. If the values don't match, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value.\r\n\r\nSpecify an ETag value for this conditional header to take a snapshot of the blob, only if its ETag value doesn't match the value specified. If the values are identical, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope to use to encrypt the request contents. This header is supported in version 2019-02-02 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional. If you specify this header, the operation is performed only if both of the following conditions are met:\r\n\r\n- The blob's lease is currently active.\r\n- The lease ID specified in the request matches that of the blob.\r\n\r\nIf this header is specified, and either of these conditions aren't met, the request fails. The Snapshot Blob operation fails with status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "8f5e0e97-744c-4218-bdd3-8f45ea545e95":
                Name = "Copy Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-copy-source", Description = "Required. Specifies the name of the source blob or file.\r\n\r\nBeginning with version 2012-02-12, this value can be a URL of up to 2 kibibytes (KiB) in length that specifies a blob. The value should be URL encoded as it would appear in a request URI.\r\n\r\nThe read operation on a source blob in the same storage account can be authorized via shared key. Beginning with version 2017-11-09, you can also use Microsoft Entra ID to authorize the read operation on the source blob. However, if the source is a blob in another storage account, the source blob must be public, or access to it must be authorized via a shared access signature. If the source blob is public, no authorization is required to perform the copy operation.\r\n\r\nBeginning with version 2015-02-21, the source object can be a file in Azure Files. If the source object is a file that will be copied to a blob, then the source file must be authorized through a shared access signature, whether it resides in the same account or in a different account.\r\n\r\nOnly storage accounts created on or after June 7, 2012, allow the Copy Blob operation to copy from another storage account.\r\n\r\nHere are some examples of source object URLs:\r\n\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?snapshot=<DateTime>\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?versionid=<DateTime>\r\n\r\nWhen the source object is a file in Azure Files, the source URL uses the following format. Note that the URL must include a valid SAS token for the file.\r\n\r\n- https://myaccount.file.core.windows.net/myshare/mydirectorypath/myfile?sastoken\r\n\r\nIn versions before 2012-02-12, blobs can be copied only within the same account, and a source name can use these formats:\r\n\r\n- Blob in named container: /accountName/containerName/blobName\r\n- Snapshot in named container: /accountName/containerName/blobName?snapshot=<DateTime>\r\n- Blob in root container: /accountName/blobName\r\n- Snapshot in root container: /accountName/blobName?snapshot=<DateTime>", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-lease-id", Description = "Required if the destination blob has an active lease. The lease ID specified for this header must match the lease ID of the destination blob. If the request doesn't include the lease ID or the ID isn't valid, the operation fails with status code 412 (Precondition Failed).\r\n\r\nIf this header is specified and the destination blob doesn't currently have an active lease, the operation fails with status code 412 (Precondition Failed).\r\n\r\nIn version 2012-02-12 and later, this value must specify an active infinite lease for a leased blob. A finite-duration lease ID fails with status code 412 (Precondition Failed).", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Specifies a user-defined name/value pair associated with the blob. If no name/value pairs are specified, the operation copies the metadata from the source blob or file to the destination blob. If one or more name/value pairs are specified, the destination blob is created with the specified metadata, and metadata is not copied from the source blob or file.\r\n\r\nBeginning with version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers. For more information, see Naming and referencing containers, blobs, and metadata.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets the given query-string-encoded tags on the blob. Tags are not copied from the copy source. For more information, see Remarks. Supported in version 2019-12-12 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-modified-since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the source blob has been modified since the specified date/time. If the source blob has not been modified, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-unmodified-since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the source blob has not been modified since the specified date/time. If the source blob has been modified, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-match", Description = "Optional. An ETag value. Specify this conditional header to copy the source blob only if its ETag value matches the specified value. If the values don't match, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-none-match", Description = "Optional. An ETag value. Specify this conditional header to copy the blob only if its ETag value doesn't match the specified value. If the values are identical, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the destination blob has been modified since the specified date/time. If the destination blob has not been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the destination blob has not been modified since the specified date/time. If the destination blob has been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify an ETag value for this conditional header to copy the blob only if the specified ETag value matches the ETag value for an existing destination blob. If the values don't match, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value, or the wildcard character (*).\r\n\r\nSpecify an ETag value for this conditional header to copy the blob only if the specified ETag value doesn't match the ETag value for the destination blob.\r\n\r\nSpecify the wildcard character (*) to perform the operation only if the destination blob doesn't exist.\r\n\r\nIf the specified condition isn't met, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-lease-id", Description = "Optional for versions before 2012-02-12 (unsupported in 2012-02-12 and later). Specify this header to perform the Copy Blob operation only if the provided lease ID matches the active lease ID of the source blob.\r\n\r\nIf this header is specified and the source blob doesn't currently have an active lease, the operation fails with status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-KiB character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. Specifies the tier to be set on the target blob. This header is for page blobs on a premium account only with version 2017-04-17 and later. For a full list of supported tiers, see High-performance premium storage and managed disks for VMs. This header is supported on version 2018-11-09 and later for block blobs. Block blob tiering is supported on Blob Storage or General Purpose v2 accounts. Valid values are Hot, Cool, Cold and Archive. Note: Cold tier is supported for version 2021-12-02 and later. For detailed information about block blob tiering, see Hot, cool, and archive storage tiers.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-rehydrate-priority", Description = "Optional. Indicates the priority with which to rehydrate an archived blob. This header is supported on version 2019-02-02 and later for block blobs. Valid values are High and Standard. You can set the priority on a blob only once. This header will be ignored on subsequent requests to the same blob. Default priority without this header is Standard.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-seal-blob", Description = "Optional. Supported on version 2019-12-12 or later. This header is valid for append blobs only. It seals the destination blob after the copy operation is finished.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-immutability-policy-until-date", Description = "Version 2020-06-12 and later. Specifies the retention-until date to be set on the blob. This is the date until which the blob can be protected from modification or deletion. It follows RFC1123 format.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-immutability-policy-mode", Description = "Version 2020-06-12 and later. Specifies the immutability policy mode to be set on the blob. Valid values are unlocked and locked. An unlocked value indicates that the user can change the policy by increasing or decreasing the retention-until date. A locked value indicates that these actions are prohibited.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-legal-hold", Description = "Version 2020-06-12 and later. Specifies the legal hold to be set on the blob. Valid values are true and false.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "9487af8f-941f-40c0-8671-98e8a461d0ad":
                Name = "Copy Blob From URL";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-copy-source", Description = "Required. Specifies the URL of the source blob. The value can be a URL of up to 2 kibibytes (KiB) in length that specifies a blob. The value should be URL-encoded as it would appear in a request URI. The source blob must either be public or be authorized via a shared access signature. If the source blob is public, no authorization is required to perform the operation. If the size of the source blob is greater than 256 MiB, the request fails with a 409 (Conflict) error. The blob type of the source blob has to be block blob. Here are some examples of source object URLs:\r\n\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?snapshot=<DateTime>\r\n- https://myaccount.blob.core.windows.net/mycontainer/myblob?versionid=<DateTime>", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-requires-sync", Description = "Required. Indicates that this is a synchronous Copy Blob From URL operation instead of an asynchronous Copy Blob operation.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Specifies a user-defined name/value pair associated with the blob. If no name/value pairs are specified, the operation will copy the metadata from the source blob or file to the destination blob. If one or more name/value pairs are specified, the destination blob is created with the specified metadata, and metadata is not copied from the source blob or file.\r\n\r\nBeginning with version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers. For more information, see Naming and referencing containers, blobs, and metadata.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope for encrypting the request contents. This header is supported in version 2020-12-06 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets query-string-encoded tags on the blob. Tags are not copied from the copy source. For more information, see Remarks. Supported in version 2019-12-12 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-copy-source-tag-option", Description = "Optional. Possible values are REPLACE and COPY (case-sensitive). The default value is REPLACE.\r\n\r\nIf COPY is specified, the tags from the source blob will be copied to the destination blob. The source blob must be private, and the request must have permission to the Get Blob Tags operation on the source blob and the Set Blob Tags operation on the destination blob. This incurs an extra call to the Get Blob Tags operation on the source account.\r\n\r\nREPLACE will set tags that the x-ms-tags header specifies on the destination blob. If x-ms-tags specifies REPLACE and no tags, then no tags will be set on the destination blob. Specifying COPY and x-ms-tags will result in a 409 (Conflict) error.\r\n\r\nSupported in version 2021-04-10 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-modified-since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the source blob has been modified since the specified date/time. If the source blob has not been modified, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-unmodified-since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the source blob has not been modified since the specified date/time. If the source blob has been modified, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-match", Description = "Optional. An ETag value. Specify this conditional header to copy the source blob only if its ETag value matches the specified value. If the values don't match, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-if-none-match", Description = "Optional. An ETag value. Specify this conditional header to copy the blob only if its ETag value doesn't match the specified value. If the values are identical, Blob Storage returns status code 412 (Precondition Failed). You can't specify this header if the source is an Azure file.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Modified-Since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the destination blob has been modified since the specified date/time. If the destination blob has not been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Unmodified-Since", Description = "Optional. A DateTime value. Specify this conditional header to copy the blob only if the destination blob has not been modified since the specified date/time. If the destination blob has been modified, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-Match", Description = "Optional. An ETag value. Specify an ETag value for this conditional header to copy the blob only if the specified ETag value matches the ETag value for an existing destination blob. If the values don't match, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "If-None-Match", Description = "Optional. An ETag value, or the wildcard character (*).\r\n\r\nSpecify an ETag value for this conditional header to copy the blob only if the specified ETag value doesn't match the ETag value for the destination blob.\r\n\r\nSpecify the wildcard character (*) to perform the operation only if the destination blob doesn't exist.\r\n\r\nIf the specified condition isn't met, Blob Storage returns status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-copy-source-authorization", Description = "Optional. Specifies the authorization scheme and signature for the copy source. For more information, see Authorize requests to Azure Storage.\r\nOnly the scheme bearer is supported for Microsoft Entra.\r\nThis header is supported in version 2020-10-02 and later.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-source-content-md5", Description = "Optional. Specifies an MD5 hash of the blob content from the URI. This hash is used to verify the integrity of the blob during transport of the data from the URI. When this header is specified, the storage service compares the hash of the content that has arrived from the copy source with this header value.\r\n\r\nThe MD5 hash is not stored with the blob.\r\n\r\nIf the two hashes don't match, the operation fails with error code 400 (Bad Request).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the destination blob has an active lease. The lease ID specified for this header must match the lease ID of the destination blob. If the request doesn't include the lease ID or it isn't valid, the operation fails with status code 412 (Precondition Failed).\r\n\r\nIf this header is specified and the destination blob doesn't currently have an active lease, the operation fails with status code 412 (Precondition Failed).\r\n\r\nIn version 2012-02-12 and later, this value must specify an active, infinite lease for a leased blob. A finite-duration lease ID fails with status code 412 (Precondition Failed).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-KiB character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. Specifies the tier to be set on the target blob. This header is for page blobs on a premium account only with version 2017-04-17 and later. For a full list of supported tiers, see High-performance premium storage and managed disks for VMs. This header is supported on version 2018-11-09 and later for block blobs. Block blob tiering is supported on Blob Storage or General Purpose v2 accounts. Valid values are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. For detailed information about block blob tiering, see Hot, cool, and archive storage tiers.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c":
                Name = "Abort Copy Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=copy&copyid=<id>" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="copy", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "copyid", Value="<id>", Description="Replace <id> with the copy identifier provided in the x-ms-copy-id header of the original Copy Blob operation.", IsEnabledActive="false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-copy-action", Value="abort", Description = "Required", DeleteButtonVisibility="Collapsed", IsEnabledActive = "false", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the destination blob has an active infinite lease.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "faee6c9b-b52c-4ccd-844d-1425f2b81663":
                Name = "Delete Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "DELETE");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when present, specifies the blob snapshot to delete. For more information on working with blob snapshots, see Creating a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional, version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to delete.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "deletetype", Description = "Optional, version 2020-02-10 or later. The value of deletetype can only be permanent.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease.\r\n\r\nTo perform this operation on a blob with an active lease, specify the valid lease ID for this header. If a valid lease ID isn't specified on the request, the operation fails with status code 403 (Forbidden).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-delete-snapshots", Description = "Required if the blob has associated snapshots. Specify one of the following options:\r\n\r\n- include: Delete the base blob and all of its snapshots.\r\n- only: Delete only the blob's snapshots, and not the blob itself.\r\n\r\nSpecify this header only for a request against the base blob resource. If this header is specified on a request to delete an individual snapshot, Blob Storage returns status code 400 (Bad Request).\r\n\r\nIf this header isn't specified on the request and the blob has associated snapshots, Blob Storage returns status code 409 (Conflict).", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "799b6e8b-3aab-4521-9507-2fec5b7e894a":
                Name = "Undelete Blob";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=undelete" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="undelete", Description="Required", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-undelete-source", Description = "Optional. Version 2020-08-04 and later. Only for accounts enabled with hierarchical namespace. The path of the soft-deleted blob to undelete. The format is blobPath?deletionid=<id>. The account and container name aren't included in the path. DeletionId is the unique identifier of the soft-deleted blob. You can retrieve it by listing soft-deleted blobs with the List Blobs REST API for accounts enable with hierarchical namespace. The path should be percent encoded.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "16744da0-054b-4fea-baee-2a8c0e4ee36f":
                Name = "Set Blob Tier";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=tier" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="tier", Description="Required", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when present, specifies the blob snapshot to set a tier on. For more information about working with blob snapshots, see Create a snapshot of a blob", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to set a tier on.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-access-tier", Description = "Required. Indicates the tier to be set on the blob. For a list of allowed premium page blob tiers, see High-performance Premium Storage and managed disks for VMs. For blob storage or general purpose v2 account, valid values are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. For detailed information about standard blob account blob level tiering see Hot, cool and archive storage tiers.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage Services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kB character limit that is recorded in the analytics logs when storage analytics logging is enabled. Using this header is highly recommended for correlating client-side activities with requests received by the server. For more information, see About Storage Analytics Logging.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-rehydrate-priority", Description = "Optional. Indicates the priority with which to rehydrate an archived blob. Supported on version 2019-02-02 and newer for block blobs. Valid values are High/Standard. The priority can be set on a blob only once for versions prior to 2020-06-12; this header will be ignored on subsequent requests. The default priority setting is Standard.\r\n\r\nBeginning with version 2020-06-12, the rehydration priority can be updated after it was previously set. The priority setting can be changed from Standard to High by calling Set Blob Tier with this header set to High and setting x-ms-access-tier to the same value as previously set. The priority setting cannot be lowered from High to Standard.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "27400b89-4193-4804-9c97-8fc98bd4ec73":
                Name = "Blob Batch";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/?comp=batch" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "POST");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="batch", Description="Required", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds, with a maximum value of 120 seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "restype", Description="Optional, version 2020-04-08 and later. The only value supported for the restype parameter is container. When this parameter is specified, the URI must include the container name. Any subrequests must be scoped to the same container." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", UTCVisibility="Visible", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. This version will be used for all subrequests. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "Content-Type", Description = "Required. The value of this header must be multipart/mixed, with a batch boundary. Example header value: multipart/mixed; boundary=batch_a81786c8-e301-4e42-a729-a32ca24ae252.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "Text";
                RawBody = "POST http://account.blob.core.windows.net/?comp=batch HTTP/1.1\r\nContent-Type: multipart/mixed; boundary=batch_357de4f7-6d0b-4e02-8cd2-6361411a9525\r\nx-ms-version: 2018-11-09\r\nAuthorization: SharedKey account:QvaoYDQ+0VcaA/hKFjUmQmIxXv2RT3XwwTsOTHL39HI=\r\nHost: 127.0.0.1:10000\r\nContent-Length: 1569\r\n\r\n--batch_357de4f7-6d0b-4e02-8cd2-6361411a9525\r\nContent-Type: application/http\r\nContent-Transfer-Encoding: binary\r\nContent-ID: 0\r\n\r\nDELETE /container0/blob0 HTTP/1.1\r\nx-ms-date: Thu, 14 Jun 2018 16:46:54 GMT\r\nAuthorization: SharedKey account:G4jjBXA7LI/RnWKIOQ8i9xH4p76pAQ+4Fs4R1VxasaE=\r\nContent-Length: 0\r\n\r\n--batch_357de4f7-6d0b-4e02-8cd2-6361411a9525\r\nContent-Type: application/http\r\nContent-Transfer-Encoding: binary\r\nContent-ID: 1\r\n\r\nDELETE /container1/blob1 HTTP/1.1\r\nx-ms-date: Thu, 14 Jun 2018 16:46:54 GMT\r\nAuthorization: SharedKey account:IvCoYDQ+0VcaA/hKFjUmQmIxXv2RT3XwwTsOTHL39HI=\r\nContent-Length: 0\r\n\r\n--batch_357de4f7-6d0b-4e02-8cd2-6361411a9525\r\nContent-Type: application/http\r\nContent-Transfer-Encoding: binary\r\nContent-ID: 2\r\n\r\nDELETE /container2/blob2 HTTP/1.1\r\nx-ms-date: Thu, 14 Jun 2018 16:46:54 GMT\r\nAuthorization: SharedKey account:S37N2JTjcmOQVLHLbDmp2johz+KpTJvKhbVc4M7+UqI=\r\nContent-Length: 0\r\n\r\n--batch_357de4f7-6d0b-4e02-8cd2-6361411a9525--";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "965b3e05-3adc-4413-ba39-cf82ab6e26db":
                Name = "Set Blob Immutability Policy";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=immutabilityPolicies" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="immutabilityPolicies", IsEnabledActive="false", IsValueReadyOnly="true", Description="Required", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when it's present, specifies the blob snapshot to set a tier on. For more information about working with blob snapshots, see Create a snapshot of a blob", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when it's present, specifies the version of the blob to set a tier on.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set time-outs for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-immutability-policy-until-date", Description = "Required. Indicates the retention until date to be set on the blob. This is the date until which the blob can be protected from being modified or deleted. For blob storage or general purpose v2 account, valid values are with RFC1123 format. Past times are not valid.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-immutability-policy-mode", Description = "Optional. If not specified, default value is Unlocked. Indicates the immutability policy mode to be set on the blob. For blob storage or general purpose v2 account, valid values are Unlocked/Locked. unlocked indicates the user may change the policy by increasing or decreasing the retention until date. locked indicates that these actions are prohibited.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "7e7c8aba-d49d-4de2-bb62-d1507cf00030":
                Name = "Delete Blob Immutability Policy";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=immutabilityPolicies" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "DELETE");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="immutabilityPolicies", IsEnabledActive="false", IsValueReadyOnly="true", Description="Required", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when present, specifies the blob snapshot to set the tier on. For more information on working with blob snapshots, see Creating a snapshot of a blob.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to set the tier on.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Setting timeouts for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
                break;

            case "2548c991-90c9-4159-98a8-c6d076787b01":
                Name = "Set Blob Legal Hold";
                URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob?comp=legalhold" };
                SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PUT");
                IsMethodComboEnabled = "false";
                TabIconVisibility = "Collapsed";
                Parameters = new ObservableCollection<ParameterItem>() {
                        new(_messenger) { IsEnabled = true, Key = "comp", Value="legalhold", Description="Required", IsEnabledActive="false", IsValueReadyOnly="true", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "snapshot", Description="Optional. The snapshot parameter is an opaque DateTime value that, when present, specifies the blob snapshot to set a tier on. For more information about working with blob snapshots, see Create a snapshot of a blob", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "versionid", Description="Optional for version 2019-12-12 and later. The versionid parameter is an opaque DateTime value that, when present, specifies the version of the blob to set a tier on.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds. For more information, see Set time-outs for Blob Storage operations." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                Headers = new ObservableCollection<HeaderItem>() {
                        new(_messenger) { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request. For more information, see Authorize requests to Azure Storage.", IsEnabledActive = "false", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For more information, see Versioning for the Azure Storage services.", IsEnabledActive = "false", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-legal-hold", Description = "Optional. Sets or removes the legal hold on the blob. For Blob Storage or general purpose v2 accounts, the valid values are true and false.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                        new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives. For more information, see Monitor Azure Blob Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}};
                IsBodyComboEnabled = "false";
                SelectedBodyType = "None";

                Response = new ResponseViewModel();
                Response.Visibility = "Collapsed";
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