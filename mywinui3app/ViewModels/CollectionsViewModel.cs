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

        var request = new RequestItem()
        {
            Name = "List Containers",
            URL = new URL() { RawURL = "https://myaccount.blob.core.windows.net/" },
            Method = "GET",
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "comp", Value="list", DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = false, Key = "prefix", Description = "Optional. Filters the results to return only containers with a name that begins with the specified prefix." , DeleteButtonVisibility = "Collapsed"},
                new() { IsEnabled = false, Key = "marker", Description = "Optional. A string value that identifies the portion of the list of containers to be returned with the next listing operation. The operation returns the NextMarker value within the response body, if the listing operation didn't return all containers remaining to be listed with the current page. You can use the NextMarker value as the value for the marker parameter in a subsequent call to request the next page of list items. The marker value is opaque to the client." , DeleteButtonVisibility = "Collapsed"},
                new() { IsEnabled = false, Key = "maxresults", Description = "Optional. Specifies the maximum number of containers to return. If the request doesn't specify maxresults, or specifies a value greater than 5000, the server will return up to 5000 items.Note that if the listing operation crosses a partition boundary, then the service will return a continuation token for retrieving the remainder of the results. For this reason, it's possible that the service will return fewer results than specified by maxresults, or than the default of 5000.If the parameter is set to a value less than or equal to zero, the server returns status code 400 (Bad Request).", DeleteButtonVisibility = "Collapsed"    },
                new() { IsEnabled = false, Key = "include", Description="Optional. Specifies one or more datasets to include in the response:-metadata: Note that metadata requested with this parameter must be stored in accordance with the naming restrictions imposed by the 2009-09-19 version of Blob Storage. Beginning with this version, all metadata names must adhere to the naming conventions for C# identifiers.-deleted: Version 2019-12-12 and later. Specifies that soft-deleted containers should be included in the response.-system: Version 2020-10-02 and later. Specifies if system containers are to be included in the response. Including this option will list system containers, such as $logs and $changefeed. Note that the specific system containers returned will vary, based on which service features are enabled on the storage account.",DeleteButtonVisibility = "Collapsed" },
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed"},
            },
            Body = new ObservableCollection<BodyItem>()
        };

        request = new RequestItem()
        {
            Name = "Set Blob Service Properties",
            URL = new URL() { RawURL = "https://account-name.blob.core.windows.net/" },
            Method = "PUT",
            Parameters = new ObservableCollection<ParameterItem>() {
                new() { IsEnabled = true, Key = "restype", Value="service", Description="restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties.", DeleteButtonVisibility="Collapsed" },
                new() { IsEnabled = false, Key = "comp", Value="properties", Description = "restype=service&comp=properties | Required. The combination of both query strings is required to set the storage service properties." , DeleteButtonVisibility = "Collapsed"},
                new() { IsEnabled = false, Key = "timeout", Description="Optional. The timeout parameter is expressed in seconds" , DeleteButtonVisibility = "Collapsed"}},
            Headers = new ObservableCollection<HeaderItem>()
            {
                new() { IsEnabled = true, Key = "Authorization", Description = "Required. Specifies the authorization scheme, account name, and signature", DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = true, Key = "x-ms-date", Description = "Required. Specifies the Coordinated Universal Time (UTC) for the request", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = true, Key = "x-ms-version", Description = "Required for all authorized requests. Specifies the version of the operation to use for this request.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed"},
                new() { IsEnabled = false, Key = "x-ms-client-request-id", Description = "Optional. Provides a client-generated, opaque value with a 1-kibibyte (KiB) character limit that's recorded in the logs when logging is configured. We highly recommend that you use this header to correlate client-side activities with requests that the server receives.",DeleteButtonVisibility="Collapsed"},
            },
            Body = new ObservableCollection<BodyItem>()
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

public partial class RequestItem
{
    public string RequestId;
    public string Name;
    public string Method;
    public URL URL;
    public ObservableCollection<ParameterItem> Parameters;
    public ObservableCollection<HeaderItem> Headers;
    public ObservableCollection<BodyItem> Body;
}