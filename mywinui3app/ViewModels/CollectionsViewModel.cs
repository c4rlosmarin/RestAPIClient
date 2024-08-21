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
        collection.Groups = new ObservableCollection<GroupItem>();

        var group = new GroupItem() { Name = "Operations on the Account (Blob Service)" };
        group.Requests = new ObservableCollection<RequestItem>();

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

        group.Requests.Add(request);

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

        group.Requests.Add(request);

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

        group.Requests.Add(request);

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
        group.Requests.Add(request);

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
            Parameters = new ObservableCollection<ParameterItem>() { new() { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" } },
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

        group.Requests.Add(request);

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

        group.Requests.Add(request);

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

        group.Requests.Add(request);
        collection.Groups.Add(group);


        group = new GroupItem() { Name = "Operations on containers" };
        group.Requests = new ObservableCollection<RequestItem>();
        collection.Groups.Add(group);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Create Container",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-meta-name:value", Description = "Optional. A name-value pair to associate with the container as metadata. Note: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-public-access", Description = "Optional. Specifies whether data in the container can be accessed publicly and the level of access. Possible values include:\r\n\r\n- container: Specifies full public read access for container and blob data. Clients can enumerate blobs within the container via anonymous request, but they can't enumerate containers within the storage account.\r\n- blob: Specifies public read access for blobs. Blob data within this container can be read via anonymous request, but container data isn't available. Clients can't enumerate blobs within the container via anonymous request.\r\n\r\nIf this header isn't included in the request, container data is private to the account owner.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Container Properties",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container Properties succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID does not match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Container Metadata",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=metadata" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="metadata", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container Metadata succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID doesn't match, error code 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Set Container Metadata",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=metadata" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="metadata", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Set Container Metadata succeeds only if the container's lease is active and matches this ID. If there's no active lease or the ID doesn't match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-meta-name:value", Description = "Optional. A name-value pair to associate with the container as metadata.\r\n\r\nEach call to this operation replaces all existing metadata that's attached to the container. To remove all metadata from the container, call this operation with no metadata headers.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Get Container ACL",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=acl" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="acl", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Get Container ACL succeeds only if the container’s lease is active and matches this ID. If there's no active lease or the ID does not match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Set Container ACL",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=acl" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="acl", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-public-access", Description = "Optional. Specifies whether data in the container may be accessed publicly and the level of access. Possible values include:\r\n\r\n- container: Specifies full public read access for container and blob data. Clients can enumerate blobs within the container via anonymous request, but can't enumerate containers within the storage account.\r\n- blob: Specifies public read access for blobs. Blob data within this container can be read via anonymous request, but container data isn't available. Clients can't enumerate blobs within the container via anonymous request.\r\n\r\nIf this header isn't included in the request, container data is private to the account owner.\r\n\r\nNote that setting public access for a container in an Azure Premium Storage account isn't permitted.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Optional, version 2012-02-12 and later. If it's specified, Set Container ACL succeeds only if the container's lease is active and matches this ID. If there's no active lease or the ID doesn't match, 412 (Precondition Failed) is returned.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "Xml",
            RawBody = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  \r\n<SignedIdentifiers>  \r\n  <SignedIdentifier>   \r\n    <Id>unique-64-character-value</Id>  \r\n    <AccessPolicy>  \r\n      <Start>start-time</Start>  \r\n      <Expiry>expiry-time</Expiry>  \r\n      <Permission>abbreviated-permission-list</Permission>  \r\n    </AccessPolicy>  \r\n  </SignedIdentifier>  \r\n</SignedIdentifiers>",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Delete Container",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container" },
            SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.DELETE,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-lease-id", Description = "Required for version 2012-02-12 and later if the container has an active lease. To call Delete Container on a container that has an active lease, specify the lease ID in this header. If this header isn't specified when there is an active lease, Delete Container returns a 409 (Conflict) error. If you specify the wrong lease ID, or a lease ID on a container that doesn't have an active lease, Delete Container returns a 412 (Precondition failed) error.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Lease Container",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?comp=lease&restype=container" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-version", Description = "Optional. Specifies the version of the operation to use for this request", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required to renew, change, or release the lease.\r\n\r\nYou can specify the value of x-ms-lease-id in any valid GUID string format. See Guid Constructor (String) for a list of valid formats.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-action", Description = "acquire: Requests a new lease. If the container doesn't have an active lease, Blob Storage creates a lease on the container, and returns a new lease ID. If the container has an active lease, you can only request a new lease by using the active lease ID. You can, however, specify a new x-ms-lease duration, including negative one (-1) for a lease that never expires.\r\n\r\nrenew: Renews the lease. You can renew the lease if the lease ID specified on the request matches that associated with the container. Note that the lease can be renewed even if it has expired, as long as the container hasn't been leased again since the expiration of that lease. When you renew a lease, the lease duration clock resets.\r\n\r\nchange: Change the lease ID of an active lease. A change must include the current lease ID in x-ms-lease-id, and a new lease ID in x-ms-proposed-lease-id.\r\n\r\nrelease: Release the lease. You can release the lease if the lease ID specified on the request matches that associated with the container. Releasing the lease allows another client to immediately acquire the lease for the container, as soon as the release is complete.\r\n\r\nbreak: Break the lease, if the container has an active lease. After a lease is broken, it can't be renewed. Any authorized request can break the lease. The request isn't required to specify a matching lease ID. When a lease is broken, the lease break period is allowed to elapse. You can only perform break and release lease operations on the container during this time. When a lease is successfully broken, the response indicates the interval in seconds until a new lease can be acquired.\r\n\r\nA lease that has been broken can also be released. A client can immediately acquire a container lease that has been released.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-break-period", Description = "Optional. For a break operation, this header is the proposed duration that the lease should continue before it's broken, between 0 and 60 seconds. This break period is only used if it's shorter than the time remaining on the lease. If longer, the time remaining on the lease is used. A new lease won't be available before the break period has expired, but the lease can be held for longer than the break period. If this header doesn't appear with a break operation, a fixed-duration lease breaks after the remaining lease period elapses, and an infinite lease breaks immediately.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-duration", Description = "Required for acquire. Specifies the duration of the lease, in seconds, or negative one (-1) for a lease that never expires. A non-infinite lease can be between 15 and 60 seconds. A lease duration can't be changed by using renew or change.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-proposed-lease-id", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Origin", Description = "Optional for acquire, and required for change. Proposed lease ID, in a GUID string format. Blob Storage returns 400 (Invalid request) if the proposed lease ID isn't in the correct format.", DatePickerButtonVisibility="Collapsed",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Restore Container",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/destinationcontainer?restype=container&comp=undelete" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", Description="Required. The restype parameter value must be container.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="undelete", Description="Required. The comp parameter value must be undelete.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For this operation, the version must be 2018-03-28 or later", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-deleted-container-name", Description = "Required. You use this header to uniquely identify the soft-deleted container that should be restored.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-deleted-container-version", Description = "Required. You use this header to uniquely identify the soft-deleted container that should be restored. You can obtain this value from specifying the deleted value in the include query parameter of the List Containers operation.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "List Blobs",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=list" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="list", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "prefix", Description="Optional. Filters the results to return only blobs with names that begin with the specified prefix. In accounts that have a hierarchical namespace, an error will occur in cases where the name of a file appears in the middle of the prefix path. For example, you might attempt to find blobs that are named readmefile.txt by using the prefix path folder1/folder2/readme/readmefile.txt. An error will appear if any subfolder contains a file named readme." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "delimiter", Description = "Optional. When the request includes this parameter, the operation returns a BlobPrefix element in the response body. This element acts as a placeholder for all blobs with names that begin with the same substring, up to the appearance of the delimiter character. The delimiter can be a single character or a string.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the list to be returned with the next list operation. The operation returns a marker value within the response body if the list returned was not complete. You can then use the marker value in a subsequent call to request the next set of list items.\r\n\r\nThe marker value is opaque to the client.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of blobs to return, including all BlobPrefix elements. If the request doesn't specify maxresults, or specifies a value greater than 5,000, the server will return up to 5,000 items. If there are additional results to return, the service returns a continuation token in the NextMarker response element. In certain cases, the service might return fewer results than specified by maxresults, and also return a continuation token.\r\n\r\nSetting maxresults to a value less than or equal to zero results in error response code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "include", Description = "Optional. Specifies one or more datasets to include in the response:\r\n\r\n- snapshots: Specifies that snapshots should be included in the enumeration. Snapshots are listed from oldest to newest in the response.\r\n- metadata: Specifies that blob metadata be returned in the response.\r\n- uncommittedblobs: Specifies that blobs for which blocks have been uploaded, but which haven't been committed by using Put Block List, be included in the response.\r\n- copy: Version 2012-02-12 and later. Specifies that metadata related to any current or previous Copy Blob operation should be included in the response.\r\n-deleted: Version 2017-07-29 and later. Specifies that soft-deleted blobs should be included in the response.\r\n-tags: Version 2019-12-12 and later. Specifies that user-defined, blob index tags should be included in the response.\r\n-versions: Version 2019-12-12 and later. Specifies that versions of blobs should be included in the enumeration.\r\n-deletedwithversions: Version 2020-10-02 and later. Specifies that deleted blobs with any versions (active or deleted) should be included in the response. Items that you've permanently deleted appear in the response until they are processed by garbage collection. Use the tag \\<HasVersionsOnly\\>, and the value true.\r\n-immutabilitypolicy: Version 2020-06-12 and later. Specifies that the enumeration should include the immutability policy until date, and the immutability policy mode of the blobs.\r\n-legalhold: Version 2020-06-12 and later. Specifies that the enumeration should include the legal hold of blobs.\r\n-permissions: Version 2020-06-12 and later. Supported only for accounts with a hierarchical namespace enabled. If a request includes this parameter, then the owner, group, permissions, and access control list for the listed blobs or directories will be included in the enumeration.\r\n\r\nTo specify more than one of these options on the URI, you must separate each option with a URL-encoded comma (\"%82\").", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "showonly", Description = "Optional. Specifies one of these datasets to be returned in the response:\r\n\r\n-deleted: Optional. Version 2020-08-04 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains soft-deleted blobs. Note that POSIX ACL authorization fallback is not supported for listing soft deleted blobs. If include=deleted is also specified, the request fails with Bad Request (400).\r\n-files: Optional. Version 2020-12-06 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains files.\r\n-directories: Optional. Version 2020-12-06 and later. Only for accounts enabled with hierarchical namespace. When a request includes this parameter, the list only contains directories.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "timeout", Description = "Optional. The timeout parameter is expressed in seconds", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }
        },

            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request. For this operation, the version must be 2018-03-28 or later", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-upn", Description = "Optional. Valid only when a hierarchical namespace is enabled for the account, and include=permissions is provided in the request. If true, the user identity values returned in the <Owner>, <Group>, and <Acl> fields are transformed from Microsoft Entra object IDs to user principal names. If false, the values are returned as Microsoft Entra object IDs. The default value is false. Note that group and application object IDs aren't translated because they don't have unique friendly names.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Find Blobs by Tags in Container",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer?restype=container&comp=blobs&where=expression" },
            SelectedMethod = new MethodsItemViewModel() { Name = "GET" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.GET,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="container", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "comp", Value="blobs", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "where", Value="expression", Description="Required. Filters the result set to include only blobs whose tags match the specified expression." , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the result set to be returned with the next operation. The operation returns a marker value within the response body if the returned result set was not complete. The marker value can then be used in a subsequent call to request the next set of items.\r\n\r\nThe marker value is opaque to the client.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of blobs to return. If the request doesn't specify maxresults or specifies a value greater than 5,000, the server returns up to 5,000 items. If there are additional results to return, the service returns a continuation token in the NextMarker response element. In certain cases, the service might return fewer results than maxresults specifies but still return a continuation token.\r\n\r\nSetting maxresults to a value less than or equal to zero results in error response code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" },
                new() { IsEnabled = false, Key = "timeout", Description = "Optional. Expressed in seconds.", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true" }
        },

            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests, but optional for anonymous requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
            },
            IsBodyComboEnabled = "false",
            SelectedBodyType = "None",
            ResponseVisibility = "Collapsed"
        };
        group.Requests.Add(request);

        group = new GroupItem() { Name = "Operations on Blobs" };
        group.Requests = new ObservableCollection<RequestItem>();

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Put Blob",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request.\r\n\r\nFor a page blob or an append blob, the value of this header must be set to zero, because Put Blob is used only to initialize the blob. To write content to an existing page blob, call Put Page. To write content to an append blob, call Append Block.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Type", Description = "Optional. The MIME content type of the blob. The default type is application/octet-stream.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Encoding", Description = "Optional. Specifies which content encodings have been applied to the blob. This value is returned to the client when the Get Blob operation is performed on the blob resource. When this value is returned, the client can use it to decode the blob content.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Language", Description = "Optional. Specifies the natural languages that are used by this resource.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-MD5", Description = "Optional. An MD5 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request).\r\n\r\nWhen the header is omitted in version 2012-02-12 or later, Blob Storage generates an MD5 hash.\r\n\r\nResults from Get Blob, Get Blob Properties, and List Blobs include the MD5 hash.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-content-crc64", Description = "Optional. A CRC64 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request). This header is supported in versions 02-02-2019 and later.\r\n\r\nIf both Content-MD5 and x-ms-content-crc64 headers are present, the request fails with a 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Cache-Control", Description = "Optional. Blob Storage stores this value but doesn't use or modify it.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-type", Description = "Optional. Set the blob’s content type.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-encoding", Description = "Optional. Set the blob’s content encoding.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-language", Description = "Optional. Set the blob's content language.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. Set the blob’s MD5 hash. For BlockBlob, this header takes precedence over Content-MD5 when verifying the integrity of the blob during transport. For PageBlob and AppendBlob, this header directly sets the MD5 hash of the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-cache-control", Description = "Optional. Sets the blob's cache control.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-blob-type", Description = "<BlockBlob ¦ PageBlob ¦ AppendBlob> Required. Specifies the type of blob to create: block blob, page blob, or append blob. Support for creating an append blob is available only in version 2015-02-21 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Name-value pairs associated with the blob as metadata.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope to use to encrypt the request contents. This header is supported in versions 2019-02-02 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-context", Description = "Optional. Default is “Empty”. If the value is set it will set blob system metadata. Max length-1024. Valid only when Hierarchical Namespace is enabled for the account. This header is supported in versions 2021-08-06 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets the given query-string encoded tags on the blob. See the Remarks for additional information. Supported in version 2019-12-12 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-disposition", Description = "Optional. Sets the blob’s Content-Disposition header. Available for versions 2013-08-15 and later.\r\n\r\nThe Content-Disposition response header field conveys additional information about how to process the response payload, and you can use it to attach additional metadata. For example, if the header is set to attachment, it indicates that the user-agent should not display the response. Instead, it should display a Save As dialog with a file name other than the specified blob name.\r\n\r\nThe response from the Get Blob and Get Blob Properties operations includes the content-disposition header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the analytics logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. The tier to be set on the blob. For page blobs on a Premium Storage account only with version 2017-04-17 and later. For a full list of page blob-supported tiers, see High-performance premium storage and managed disks for virtual machines (VMs). For block blobs, supported on blob storage or general purpose v2 accounts only with version 2018-11-09 and later. Valid values for block blob tiers are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. ",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-immutability-policy-until-date", Description = "Version 2020-06-12 and later. Specifies the retention-until date to be set on the blob. This is the date until which the blob can be protected from being modified or deleted. Follows RFC1123 format.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-immutability-policy-mode", Description = "Version 2020-06-12 and later. Specifies the immutability policy mode to be set on the blob. Valid values are unlocked and locked. With unlocked, users can change the policy by increasing or decreasing the retention-until date. With locked, these actions are prohibited.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-legal-hold", Description = "Version 2020-06-12 and later. Specifies the legal hold to be set on the blob. Valid values are true and false.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-expiry-option", Description = "Optional. Version 2023-08-03 and later. Specifies the expiration date option for the request. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-expiry-time", Description = "Optional. Version 2023-08-03 and later. Specifies the time when the blob is set to expire. The format for expiration date varies according to x-ms-expiry-option. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-length", Description = "Required for page blobs. This header specifies the maximum size for the page blob, up to 8 tebibytes (TiB). The page blob size must be aligned to a 512-byte boundary.\r\n\r\nIf this header is specified for a block blob or an append blob, Blob Storage returns status code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-sequence-number", Description = "Optional. Set for page blobs only. The sequence number is a user-controlled value that you can use to track requests. The value of the sequence number must be from 0 to 2^63 - 1. The default value is 0.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-access-tier", Description = "Version 2017-04-17 and later. For page blobs on a premium storage account only. Specifies the tier to be set on the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "This header can be used to troubleshoot requests and corresponding responses. The value of this header is equal to the value of the x-ms-client-request-id header if it's present in the request and the value contains no more than 1,024 visible ASCII characters. If the x-ms-client-request-id header isn't present in the request, it won't be present in the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
            },
            Body = new ObservableCollection<BodyItem>(),
            IsBodyComboEnabled = "true",
            SelectedBodyType = "Xml",
            ResponseVisibility = "Collapsed"
        };

        group.Requests.Add(request);

        request = new RequestItem()
        {
            RequestId = Guid.NewGuid().ToString(),
            IsExistingRequest = true,
            Name = "Put Blob",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/mycontainer/myblob" },
            SelectedMethod = new MethodsItemViewModel() { Name = "PUT" },
            IsMethodComboEnabled = "false",
            TabIconVisibility = "Collapsed",
            TabMethodForegroundColor = foregroundColorHelper.PUT,
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "Content-Length", Description = "Required. The length of the request.\r\n\r\nFor a page blob or an append blob, the value of this header must be set to zero, because Put Blob is used only to initialize the blob. To write content to an existing page blob, call Put Page. To write content to an append blob, call Append Block.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Type", Description = "Optional. The MIME content type of the blob. The default type is application/octet-stream.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Encoding", Description = "Optional. Specifies which content encodings have been applied to the blob. This value is returned to the client when the Get Blob operation is performed on the blob resource. When this value is returned, the client can use it to decode the blob content.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-Language", Description = "Optional. Specifies the natural languages that are used by this resource.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Content-MD5", Description = "Optional. An MD5 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request).\r\n\r\nWhen the header is omitted in version 2012-02-12 or later, Blob Storage generates an MD5 hash.\r\n\r\nResults from Get Blob, Get Blob Properties, and List Blobs include the MD5 hash.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-content-crc64", Description = "Optional. A CRC64 hash of the blob content. This hash is used to verify the integrity of the blob during transport. When this header is specified, the storage service checks the hash that has arrived against the one that was sent. If the two hashes don't match, the operation fails with error code 400 (Bad Request). This header is supported in versions 02-02-2019 and later.\r\n\r\nIf both Content-MD5 and x-ms-content-crc64 headers are present, the request fails with a 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Cache-Control", Description = "Optional. Blob Storage stores this value but doesn't use or modify it.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-type", Description = "Optional. Set the blob’s content type.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-encoding", Description = "Optional. Set the blob’s content encoding.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-language", Description = "Optional. Set the blob's content language.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-md5", Description = "Optional. Set the blob’s MD5 hash. For BlockBlob, this header takes precedence over Content-MD5 when verifying the integrity of the blob during transport. For PageBlob and AppendBlob, this header directly sets the MD5 hash of the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-cache-control", Description = "Optional. Sets the blob's cache control.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = true, Key = "x-ms-blob-type", Description = "<BlockBlob ¦ PageBlob ¦ AppendBlob> Required. Specifies the type of blob to create: block blob, page blob, or append blob. Support for creating an append blob is available only in version 2015-02-21 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-meta-name", Description = "Optional. Name-value pairs associated with the blob as metadata.\r\n\r\nNote: As of version 2009-09-19, metadata names must adhere to the naming rules for C# identifiers.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-scope", Description = "Optional. Indicates the encryption scope to use to encrypt the request contents. This header is supported in versions 2019-02-02 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-context", Description = "Optional. Default is “Empty”. If the value is set it will set blob system metadata. Max length-1024. Valid only when Hierarchical Namespace is enabled for the account. This header is supported in versions 2021-08-06 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-tags", Description = "Optional. Sets the given query-string encoded tags on the blob. See the Remarks for additional information. Supported in version 2019-12-12 and later.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-lease-id", Description = "Required if the blob has an active lease. To perform this operation on a blob with an active lease, specify the valid lease ID for this header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-disposition", Description = "Optional. Sets the blob’s Content-Disposition header. Available for versions 2013-08-15 and later.\r\n\r\nThe Content-Disposition response header field conveys additional information about how to process the response payload, and you can use it to attach additional metadata. For example, if the header is set to attachment, it indicates that the user-agent should not display the response. Instead, it should display a Save As dialog with a file name other than the specified blob name.\r\n\r\nThe response from the Get Blob and Get Blob Properties operations includes the content-disposition header.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "Origin", Description = "Optional. Specifies the origin from which the request is issued. The presence of this header results in cross-origin resource sharing (CORS) headers on the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the analytics logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-access-tier", Description = "Optional. The tier to be set on the blob. For page blobs on a Premium Storage account only with version 2017-04-17 and later. For a full list of page blob-supported tiers, see High-performance premium storage and managed disks for virtual machines (VMs). For block blobs, supported on blob storage or general purpose v2 accounts only with version 2018-11-09 and later. Valid values for block blob tiers are Hot, Cool, Cold, and Archive. Note: Cold tier is supported for version 2021-12-02 and later. ",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-immutability-policy-until-date", Description = "Version 2020-06-12 and later. Specifies the retention-until date to be set on the blob. This is the date until which the blob can be protected from being modified or deleted. Follows RFC1123 format.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-immutability-policy-mode", Description = "Version 2020-06-12 and later. Specifies the immutability policy mode to be set on the blob. Valid values are unlocked and locked. With unlocked, users can change the policy by increasing or decreasing the retention-until date. With locked, these actions are prohibited.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-legal-hold", Description = "Version 2020-06-12 and later. Specifies the legal hold to be set on the blob. Valid values are true and false.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-expiry-option", Description = "Optional. Version 2023-08-03 and later. Specifies the expiration date option for the request. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-expiry-time", Description = "Optional. Version 2023-08-03 and later. Specifies the time when the blob is set to expire. The format for expiration date varies according to x-ms-expiry-option. For more information, see ExpiryOption. This header is valid for accounts with hierarchical namespace enabled.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-content-length", Description = "Required for page blobs. This header specifies the maximum size for the page blob, up to 8 tebibytes (TiB). The page blob size must be aligned to a 512-byte boundary.\r\n\r\nIf this header is specified for a block blob or an append blob, Blob Storage returns status code 400 (Bad Request).",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-blob-sequence-number", Description = "Optional. Set for page blobs only. The sequence number is a user-controlled value that you can use to track requests. The value of the sequence number must be from 0 to 2^63 - 1. The default value is 0.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-access-tier", Description = "Version 2017-04-17 and later. For page blobs on a premium storage account only. Specifies the tier to be set on the blob.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "This header can be used to troubleshoot requests and corresponding responses. The value of this header is equal to the value of the x-ms-client-request-id header if it's present in the request and the value contains no more than 1,024 visible ASCII characters. If the x-ms-client-request-id header isn't present in the request, it won't be present in the response.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-key", Description = "The Base64-encoded AES-256 encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-key-sha256", Description = "The Base64-encoded SHA256 hash of the encryption key.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                new() { IsEnabled = false, Key = "x-ms-encryption-algorithm", Description = "Specifies the algorithm to use for encryption. The value of this header must be AES256.",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
            },
            Body = new ObservableCollection<BodyItem>(),
            IsBodyComboEnabled = "true",
            SelectedBodyType = "Xml",
            ResponseVisibility = "Collapsed"
        };

        group.Requests.Add(request);










        collection.Groups.Add(group);

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
    public ObservableCollection<GroupItem> groups;
}

public partial class GroupItem : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public ObservableCollection<RequestItem> requests;
}

public partial class RequestItem : ObservableRecipient
{
    public string RequestId
    {
        get; set;
    }
    public bool IsExistingRequest
    {
        get; set;
    }
    [ObservableProperty]
    public string name;
    public URL URL
    {
        get; set;
    }
    public MethodsItemViewModel SelectedMethod
    {
        get; set;
    }
    public string IsMethodComboEnabled
    {
        get; set;
    }
    public string TabIconVisibility
    {
        get; set;
    }
    public string TabMethodForegroundColor
    {
        get; set;
    }
    public ObservableCollection<ParameterItem> Parameters;
    public ObservableCollection<HeaderItem> Headers;
    public ObservableCollection<BodyItem> Body;
    public string IsBodyComboEnabled
    {
        get; set;
    }
    public string SelectedBodyType
    {
        get; set;
    }
    public string ResponseVisibility
    {
        get; set;
    }
    public string RawBody
    {
        get; set;
    }
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