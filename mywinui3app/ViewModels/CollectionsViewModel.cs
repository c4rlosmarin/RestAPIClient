using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class CollectionsViewModel : ObservableRecipient
{
    [ObservableProperty]
    ObservableCollection<CollectionItem> collections;

    public CollectionsViewModel()
    {
        Collections = new ObservableCollection<CollectionItem>();
        InitializeCollections();
    }

    private void InitializeCollections()
    {
        var collection = new CollectionItem();
        collection.Name = "Blob Service REST API";
        collection.Requests = new ObservableCollection<RequestItem>();

        var foregroundColorHelper = new MethodForegroundColor();
        var request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "List Containers",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "comp", Value="list", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "prefix", Description = "Optional. Filters the results to return only containers with a name that begins with the specified prefix." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the list of containers to be returned with the next listing operation. The operation returns the NextMarker value within the response body, if the listing operation didn't return all containers remaining to be listed with the current page. You can use the NextMarker value as the value for the marker parameter in a subsequent call to request the next page of list items. The marker value is opaque to the client." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of containers to return. If the request doesn't specify maxresults, or specifies a value greater than 5000, the server will return up to 5000 items.Note that if the listing operation crosses a partition boundary, then the service will return a continuation token for retrieving the remainder of the results. For this reason, it's possible that the service will return fewer results than specified by maxresults, or than the default of 5000.If the parameter is set to a value less than or equal to zero, the server returns status code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "include", Description="Optional. Specifies one or more datasets to include in the response:-metadata: Note that metadata requested with this parameter must be stored in accordance with the naming restrictions imposed by the 2009-09-19 version of Blob Storage. Beginning with this version, all metadata names must adhere to the naming conventions for C# identifiers.-deleted: Version 2019-12-12 and later. Specifies that soft-deleted containers should be included in the response.-system: Version 2020-10-02 and later. Specifies if system containers are to be included in the response. Including this option will list system containers, such as $logs and $changefeed. Note that the specific system containers returned will vary, based on which service features are enabled on the storage account.",DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            Body = new ObservableCollection<BodyItem>(),
            IsBodyComboEnabled = "false",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Set Blob Service Properties",
            URL = new URL() { RawURL = "https://account-name.blob.core.windows.net/" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="service", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="properties", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },            
            IsBodyComboEnabled = "false",
            SelectedBodyType = "Xml",
            RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<StorageServiceProperties>  \r\n    <Logging>  \r\n        <Version>version-number</Version>  \r\n        <Delete>true|false</Delete>  \r\n        <Read>true|false</Read>  \r\n        <Write>true|false</Write>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </Logging>  \r\n    <HourMetrics>  \r\n        <Version>version-number</Version>  \r\n        <Enabled>true|false</Enabled>  \r\n        <IncludeAPIs>true|false</IncludeAPIs>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </HourMetrics>  \r\n    <MinuteMetrics>  \r\n        <Version>version-number</Version>  \r\n        <Enabled>true|false</Enabled>  \r\n        <IncludeAPIs>true|false</IncludeAPIs>  \r\n        <RetentionPolicy>  \r\n            <Enabled>true|false</Enabled>  \r\n            <Days>number-of-days</Days>  \r\n        </RetentionPolicy>  \r\n    </MinuteMetrics>  \r\n    <Cors>  \r\n        <CorsRule>  \r\n            <AllowedOrigins>comma-separated-list-of-allowed-origins</AllowedOrigins>  \r\n            <AllowedMethods>comma-separated-list-of-HTTP-verbs</AllowedMethods>  \r\n            <MaxAgeInSeconds>max-caching-age-in-seconds</MaxAgeInSeconds>  \r\n            <ExposedHeaders>comma-separated-list-of-response-headers</ExposedHeaders>  \r\n            <AllowedHeaders>comma-separated-list-of-request-headers</AllowedHeaders>  \r\n        </CorsRule>  \r\n    </Cors>    \r\n    <DefaultServiceVersion>default-service-version-string</DefaultServiceVersion>\r\n    <DeleteRetentionPolicy>\r\n        <Enabled>true|false</Enabled>\r\n        <Days>number-of-days</Days>\r\n    </DeleteRetentionPolicy>\r\n    <StaticWebsite>\r\n        <Enabled>true|false</Enabled>\r\n        <IndexDocument>default-name-of-index-page-under-each-directory</IndexDocument>\r\n        <DefaultIndexDocumentPath>absolute-path-of-the-default-index-page</DefaultIndexDocumentPath>\r\n        <ErrorDocument404Path>absolute-path-of-the-custom-404-page</ErrorDocument404Path>\r\n    </StaticWebsite>\r\n</StorageServiceProperties>",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Blob Service Properties",
            URL = new URL() { RawURL = "https://<account-name>.blob.core.windows.net/?restype=service&comp=properties" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="service", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="properties", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Preflight Blob Request",
            URL = new URL() { RawURL = "http://<account-name>.blob.core.windows.net/<blob-resource>" },
            SelectedMethod = new MethodsItemViewModel() { Name = "OPTIONS" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.OPTIONS,
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Origin", Description = "Required. Specifies the origin from which the request will be issued. The origin is checked against the service's CORS rules to determine the success or failure of the preflight request.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "Access-Control-Request-Method", Description = "Required. Specifies the method (or HTTP verb) for the request. The method is checked against the service's CORS rules to determine the failure or success of the preflight request.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Access-Control-Request-Headers", Description = "Optional. Specifies the request headers that will be sent. If it's not present, the service assumes that the request doesn't include headers.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Blob Service Stats",
            URL = new URL() { RawURL = "https://myaccount-secondary.blob.core.windows.net/?restype=service&comp=stats" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() { new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Account Information",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/?restype=account&comp=properties" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="service", Description="Required. The restype parameter value must be account.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="properties", Description = "Required. Required. The comp parameter value must be properties." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get User Delegation Key",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/?restype=service&comp=userdelegationkey" },
            SelectedMethod = new MethodsItemViewModel() { Name = "POST" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.POST,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="service", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="userdelegationkey", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "Xml",
            RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<KeyInfo>  \r\n    <Start>String, formatted ISO Date</Start>\r\n    <Expiry>String, formatted ISO Date </Expiry>\r\n</KeyInfo>",
            ResponseVisibility = "Collapsed"
        };

        collection.Requests.Add(request);

        Collections.Add(collection);

        collection = new CollectionItem() { Name = "Data Lake Storage Gen2 REST API" };
        Collections.Add(collection);

        collection = new CollectionItem() { Name = "Queue Storage REST API" };
        Collections.Add(collection);

        collection = new CollectionItem() { Name = "Table Storage REST API" };
        Collections.Add(collection);

        collection = new CollectionItem() { Name = "Azure Files REST API" };
        Collections.Add(collection);
    }
}

public partial class CollectionItem : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public DateTime? creationTime;
    [ObservableProperty]
    public DateTime? lastModifiedTime;
    [ObservableProperty]
    public ObservableCollection<RequestItem> requests;
}

public partial class RequestItem : ObservableRecipient
{
    public string RequestId { get; set; }    
    public bool IsExistingRequest { get; set; }
    [ObservableProperty]
    public string name;
    public URL URL { get; set; }
    public MethodsItemViewModel SelectedMethod { get; set; }
    public string IsMethodComboEnabled { get; set; }
    public string TabIconVisibility { get; set; }
    public string TabMethodForegroundColor { get; set; }
    public ObservableCollection<ParameterItem> Parameters;
    public ObservableCollection<HeaderItem> Headers;
    public ObservableCollection<BodyItem> Body;
    public string IsBodyComboEnabled { get; set; }  
    public string SelectedBodyType { get; set; }
    public string ResponseVisibility { get; set; }
    public string RawBody { get; set; }
}

public class MethodForegroundColor
{
    public string GET = "#22B14C";
    public string POST = "#218BEB";
    public string PUT = "FF8C00";
    public string PATCH = "#A347D1";
    public string DELETE = "#ED2B2B";
    public string OPTIONS = "#FF6593";
    public string HEAD = "#2E7817";

    public string GetColorByMethod(string method)
    {
        switch (method)
        {
            case "GET":
                return GET;
            case "POST":
                return POST;
            case "PUT":
                return PUT;
            case "PATCH":
                return PATCH;
            case "DELETE":
                return DELETE;
            case "OPTIONS":
                return OPTIONS;
            default:
                return HEAD;
        }
    }
}