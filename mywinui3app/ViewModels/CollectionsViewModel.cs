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
        group.Requests =
        [
            new RequestItem() { RequestId = "975b53b7-f48f-4682-8434-893f5a324278",Name = "List Containers", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "e192945c-7d70-49c6-8e50-521a2f7f01c2",Name = "Set Blob Service Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94",Name = "Get Blob Service Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "e4e5c062-82a1-4282-a571-19d5acdec6d3",Name = "Preflight Blob Request", SelectedMethod = new MethodsItemViewModel() { Name = "OPTIONS" }},
            new RequestItem() { RequestId = "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17",Name = "Get Blob Service Stats", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "30e6c366-35d0-47e5-9a5c-def2ee2cfe31",Name = "Get Account Information", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "53d9941c-dcc4-48b8-a717-24064108ecda",Name = "Get User Delegation Key", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Containers" };
        group.Requests =
        [
            new RequestItem() { RequestId = "042368e5-65ec-4a73-8462-86b65be8c353",Name = "Create Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "18fd3171-d359-4e1e-bd48-44ea44ee842c",Name = "Get Container Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "2117dd4c-e41f-4c15-b5c3-2b266ca3c225",Name = "Get Container Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "78f4583c-ced5-4b21-98a6-e82690abd782",Name = "Set Container Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "4944ad75-ad15-4ae2-bfd7-79d8051c427c",Name = "Get Container ACL", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "9f93eb42-cc6b-414e-8156-c39828f8d2d3",Name = "Set Container ACL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "7661e957-a0ba-4188-a53d-4decd8672dac",Name = "Delete Container", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = "7a2948eb-9751-4f53-b7cc-272284108344",Name = "Lease Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "76dbc6cf-f1ba-49af-ac15-b0447114d933",Name = "Restore Container", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e",Name = "List Blobs", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92",Name = "Find Blobs by Tags in Container", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Blobs" };
        group.Requests =
        [
            new RequestItem() { RequestId = "1fd54291-9b89-4348-8f08-85ecce5114ae",Name = "Put Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e",Name = "Put Blob From URL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "18e8d3cf-77b7-471c-b868-782f49afdaf5",Name = "Get Blob", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd",Name = "Get Blob Properties", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "186bbfdd-abcb-4003-9efe-6504986bcff5",Name = "Set Blob Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "d3a570a7-c5bc-4a20-ac53-29d8631e8445",Name = "Get Blob Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "1a971155-adfa-47eb-b739-5aa04551e13c",Name = "Set Blob Metadata", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "d992088f-86b7-4268-a697-a1e7ec719fb4",Name = "Get Blob Tags", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "b2dde2c7-86ec-4667-a5db-8c75118f175a",Name = "Set Blob Tags", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "a1bcea4c-3c36-40ba-b133-b841d6a5bf06",Name = "Find Blobs by Tags", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "061c4c88-981e-482f-bbd8-356f3594484c",Name = "Lease Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "bf386510-9394-4720-8769-b712a6ea08fb",Name = "Snapshot Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "8f5e0e97-744c-4218-bdd3-8f45ea545e95",Name = "Copy Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "9487af8f-941f-40c0-8671-98e8a461d0ad",Name = "Copy Blob From URL", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c",Name = "Abort Copy Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "faee6c9b-b52c-4ccd-844d-1425f2b81663",Name = "Delete Blob", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = "799b6e8b-3aab-4521-9507-2fec5b7e894a",Name = "Undelete Blob", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "16744da0-054b-4fea-baee-2a8c0e4ee36f",Name = "Set Blob Tier", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "27400b89-4193-4804-9c97-8fc98bd4ec73",Name = "Blob Batch", SelectedMethod = new MethodsItemViewModel() { Name = "POST" }},
            new RequestItem() { RequestId = "965b3e05-3adc-4413-ba39-cf82ab6e26db",Name = "Set Blob Immutability Policy", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "7e7c8aba-d49d-4de2-bb62-d1507cf00030",Name = "Delete Blob Immutability Policy", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = "2548c991-90c9-4159-98a8-c6d076787b01",Name = "Set Blob Legal Hold", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
        ];
        collection.Groups.Add(group);
        Collections.Add(collection);

        collection = new CollectionItem();
        collection.Name = "Data Lake Storage Gen2 REST API";
        collection.Groups = new ObservableCollection<GroupItem>();

        group = new GroupItem() { Name = "Operations on FileSystem" };
        group.Requests =
        [
            new RequestItem() { RequestId = "6ccb4aba-4209-4192-aa87-a0c24f9e26bf",Name = "Create", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "57dc4b51-a0d4-4917-8ba9-c7acde42cb71",Name = "Delete", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = "e8f351eb-7408-4913-bf19-8002ea9da72d",Name = "Get Properties", SelectedMethod = new MethodsItemViewModel() { Name = "HEAD" }},
            new RequestItem() { RequestId = "232cc021-77e1-4b8e-a7cd-88bd8d1434c7",Name = "List", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "e71bf0b4-b2eb-4325-a8f0-d51d402124c7",Name = "Set Properties", SelectedMethod = new MethodsItemViewModel() { Name = "PATCH" }}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Path" };
        group.Requests =
        [
            new RequestItem() { RequestId = "b418189f-0b7e-4e5d-a85f-5305eea05bd7",Name = "Create", SelectedMethod = new MethodsItemViewModel() { Name = "PUT" }},
            new RequestItem() { RequestId = "3c43a681-da19-4303-8388-70b82771cdea",Name = "Delete", SelectedMethod = new MethodsItemViewModel() { Name = "DELETE" }},
            new RequestItem() { RequestId = "978153ac-94cd-4c35-8b1f-b8044109a7f9",Name = "Get Properties", SelectedMethod = new MethodsItemViewModel() { Name = "HEAD" }},
            new RequestItem() { RequestId = "351df3a9-3e1f-425e-af99-31d94888fad8",Name = "Lease", SelectedMethod = new MethodsItemViewModel() { Name = "POST" }},
            new RequestItem() { RequestId = "b9bbf718-1be0-4d6e-b6ca-941af99a1383",Name = "List", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "fd373385-ac1d-49a0-bf38-e79ab98036a5",Name = "Read", SelectedMethod = new MethodsItemViewModel() { Name = "GET" }},
            new RequestItem() { RequestId = "9f787102-8ff4-419b-af44-de620d842ebc",Name = "Update", SelectedMethod = new MethodsItemViewModel() { Name = "PATCH" }}
        ];
        collection.Groups.Add(group);

        Collections.Add(collection);
    }
}