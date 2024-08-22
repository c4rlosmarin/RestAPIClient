using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using static System.Net.WebRequestMethods;

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
        group.Requests =
        [
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "List Containers", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Service Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob Service Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Preflight Blob Request", SelectedMethod = new MethodsItemViewModel() { Name = "OPTIONS" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob Service Stats", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Account Information", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get User Delegation Key", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Containers" };
        group.Requests =
        [
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Create Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Container Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Container Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Container Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Container ACL", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Container ACL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Delete Container", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Lease Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Restore Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "List Blobs", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Find Blobs by Tags in Container", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Blobs" };
        group.Requests =
        [
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Put Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Put Blob From URL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Blob Tags", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Tags", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Find Blobs by Tags", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Lease Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Snapshot Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Copy Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Copy Blob From URL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Abort Copy Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Delete Blob", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Undelete Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Tier", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Blob Batch", SelectedMethod = new MethodsItemViewModel() { Name = "POST" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Immutability Policy", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Delete Blob Immutability Policy", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Blob Legal Hold", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
        ];
        collection.Groups.Add(group);
        Collections.Add(collection);

        collection = new CollectionItem();
        collection.Name = "Data Lake Storage Gen2 REST API";
        collection.Groups = new ObservableCollection<GroupItem>();

        group = new GroupItem() { Name = "Operations on FileSystem" };
        group.Requests =
        [
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Create", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Delete", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Properties", SelectedMethod = new MethodsItemViewModel() { Name = "HEAD" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Set Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PATCH" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "List", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Path" };
        group.Requests =
        [
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Create", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Delete", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Get Properties", SelectedMethod = new MethodsItemViewModel() { Name = "HEAD" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Lease", SelectedMethod = new MethodsItemViewModel() { Name = "POST" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "List", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Read", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = Guid.NewGuid().ToString(),Name = "Update", SelectedMethod = new MethodsItemViewModel() { Name = "PATCH" }}
        ];
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
    [ObservableProperty]
    public ObservableCollection<string> requestsList;
}

public partial class RequestItem : ObservableRecipient
{
    public string RequestId
    {
        get; set;
    }

    [ObservableProperty]
    public string name;

    public bool IsExistingRequest
    {
        get; set;
    }
    
    public MethodsItemViewModel SelectedMethod
    {
        get; set;
    }
}