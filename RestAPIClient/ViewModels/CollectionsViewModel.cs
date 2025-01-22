using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RestAPIClient.ViewModels;

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
            new RequestItem() { RequestId = "975b53b7-f48f-4682-8434-893f5a324278",Name = "List Containers", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "e192945c-7d70-49c6-8e50-521a2f7f01c2",Name = "Set Blob Service Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94",Name = "Get Blob Service Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "e4e5c062-82a1-4282-a571-19d5acdec6d3",Name = "Preflight Blob Request", Method = new MethodsItemViewModel("OPTIONS")},
            new RequestItem() { RequestId = "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17",Name = "Get Blob Service Stats", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "30e6c366-35d0-47e5-9a5c-def2ee2cfe31",Name = "Get Account Information", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "53d9941c-dcc4-48b8-a717-24064108ecda",Name = "Get User Delegation Key", Method = new MethodsItemViewModel("POST")}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Containers" };
        group.Requests =
        [
            new RequestItem() { RequestId = "042368e5-65ec-4a73-8462-86b65be8c353",Name = "Create Container", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "18fd3171-d359-4e1e-bd48-44ea44ee842c",Name = "Get Container Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "2117dd4c-e41f-4c15-b5c3-2b266ca3c225",Name = "Get Container Metadata", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "78f4583c-ced5-4b21-98a6-e82690abd782",Name = "Set Container Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "4944ad75-ad15-4ae2-bfd7-79d8051c427c",Name = "Get Container ACL", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "9f93eb42-cc6b-414e-8156-c39828f8d2d3",Name = "Set Container ACL", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "7661e957-a0ba-4188-a53d-4decd8672dac",Name = "Delete Container", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "7a2948eb-9751-4f53-b7cc-272284108344",Name = "Lease Container", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "76dbc6cf-f1ba-49af-ac15-b0447114d933",Name = "Restore Container", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e",Name = "List Blobs", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92",Name = "Find Blobs by Tags in Container", Method = new MethodsItemViewModel("GET")}
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Blobs" };
        group.Requests =
        [
            new RequestItem() { RequestId = "1fd54291-9b89-4348-8f08-85ecce5114ae",Name = "Put Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e",Name = "Put Blob From URL", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "18e8d3cf-77b7-471c-b868-782f49afdaf5",Name = "Get Blob", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd",Name = "Get Blob Properties", Method = new MethodsItemViewModel("HEAD")},
            new RequestItem() { RequestId = "186bbfdd-abcb-4003-9efe-6504986bcff5",Name = "Set Blob Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "d3a570a7-c5bc-4a20-ac53-29d8631e8445",Name = "Get Blob Metadata", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "1a971155-adfa-47eb-b739-5aa04551e13c",Name = "Set Blob Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "d992088f-86b7-4268-a697-a1e7ec719fb4",Name = "Get Blob Tags", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "b2dde2c7-86ec-4667-a5db-8c75118f175a",Name = "Set Blob Tags", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "a1bcea4c-3c36-40ba-b133-b841d6a5bf06",Name = "Find Blobs by Tags", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "061c4c88-981e-482f-bbd8-356f3594484c",Name = "Lease Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "bf386510-9394-4720-8769-b712a6ea08fb",Name = "Snapshot Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "8f5e0e97-744c-4218-bdd3-8f45ea545e95",Name = "Copy Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "9487af8f-941f-40c0-8671-98e8a461d0ad",Name = "Copy Blob From URL", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c",Name = "Abort Copy Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "faee6c9b-b52c-4ccd-844d-1425f2b81663",Name = "Delete Blob", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "799b6e8b-3aab-4521-9507-2fec5b7e894a",Name = "Undelete Blob", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "16744da0-054b-4fea-baee-2a8c0e4ee36f",Name = "Set Blob Tier", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "27400b89-4193-4804-9c97-8fc98bd4ec73",Name = "Blob Batch", Method = new MethodsItemViewModel("POST")},
            new RequestItem() { RequestId = "965b3e05-3adc-4413-ba39-cf82ab6e26db",Name = "Set Blob Immutability Policy", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "7e7c8aba-d49d-4de2-bb62-d1507cf00030",Name = "Delete Blob Immutability Policy", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "2548c991-90c9-4159-98a8-c6d076787b01",Name = "Set Blob Legal Hold", Method = new MethodsItemViewModel("PUT")},
        ];
        collection.Groups.Add(group);
        Collections.Add(collection);

        collection = new CollectionItem();
        collection.Name = "Data Lake Storage Gen2 REST API";
        collection.Groups = new ObservableCollection<GroupItem>();

        group = new GroupItem() { Name = "Operations on FileSystem" };
        group.Requests =
        [
            new RequestItem() { RequestId = "6ccb4aba-4209-4192-aa87-a0c24f9e26bf",Name = "Create", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "57dc4b51-a0d4-4917-8ba9-c7acde42cb71",Name = "Delete", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "e8f351eb-7408-4913-bf19-8002ea9da72d",Name = "Get Properties", Method = new MethodsItemViewModel("HEAD")},
            new RequestItem() { RequestId = "232cc021-77e1-4b8e-a7cd-88bd8d1434c7",Name = "List", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "e71bf0b4-b2eb-4325-a8f0-d51d402124c7",Name = "Set Properties", Method = new MethodsItemViewModel("PATCH")},
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Path" };
        group.Requests =
        [
            new RequestItem() { RequestId = "b418189f-0b7e-4e5d-a85f-5305eea05bd7",Name = "Create", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "3c43a681-da19-4303-8388-70b82771cdea",Name = "Delete", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "978153ac-94cd-4c35-8b1f-b8044109a7f9",Name = "Get Properties", Method = new MethodsItemViewModel("HEAD")},
            new RequestItem() { RequestId = "351df3a9-3e1f-425e-af99-31d94888fad8",Name = "Lease", Method = new MethodsItemViewModel("POST")},
            new RequestItem() { RequestId = "b9bbf718-1be0-4d6e-b6ca-941af99a1383",Name = "List", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "fd373385-ac1d-49a0-bf38-e79ab98036a5",Name = "Read", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "9f787102-8ff4-419b-af44-de620d842ebc",Name = "Update", Method = new MethodsItemViewModel("PATCH")},
        ];
        collection.Groups.Add(group);

        Collections.Add(collection);


        collection = new CollectionItem();
        collection.Name = "File Service REST API";
        collection.Groups = new ObservableCollection<GroupItem>();

        group = new GroupItem() { Name = "Operations on FileService" };
        group.Requests =
        [
            new RequestItem() { RequestId = "fda74fd0-e408-4c6d-894d-4e5dba70afe5",Name = "Get File Service Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "d75480b6-7625-4c80-9252-519df187629d",Name = "Set File Service Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "7345d46d-1656-4b01-b859-abed3c8346a9",Name = "Preflight File Request", Method = new MethodsItemViewModel("OPTIONS")},
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on FileShares" };
        group.Requests =
        [
            new RequestItem() { RequestId = "3efdeb4d-15ad-499a-8116-1f6609c6f74b",Name = "List Shares", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "b83808d0-c15f-498e-b783-4e18aff7e7a4",Name = "Create Share", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "d36d37f4-084a-444c-8412-461a2bea7033",Name = "Snapshot Share", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "1632ab9f-de7e-4d89-af34-3aa083581c65",Name = "Get Share Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "84390458-9e19-4bcf-8fd0-0170dc37ce58",Name = "Set Share Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "d5628fe9-a983-4f2e-89ae-b1a9582e504b",Name = "Get Share Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "b93b91c6-c4f4-407d-9e94-caa2a0660027",Name = "Set Share Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "79f1c6f1-3acd-463b-ae75-099464837801",Name = "Delete Share", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "5a6d906f-e182-4664-8289-785bc64967a3",Name = "Restore Share", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "040789bc-8f8e-4f6b-9c22-60197ddac6e0",Name = "Get Share ACL", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "63a6d979-b9b6-4cf3-9d96-185cc8812455",Name = "Set Share ACL", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "c1a70342-0109-4f6c-b631-d0733566fcc0",Name = "Get Share Stats", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "82917fc6-6b51-4a71-a907-dfca8ba1f693",Name = "Create Permission", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "c2127ec5-d23e-43ee-beff-9cdc1bd84ffb",Name = "Get Permission", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "03a0cfd5-e1cd-4c36-b4c3-5219c861279f",Name = "Lease Share", Method = new MethodsItemViewModel("PUT")},
        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Directories" };
        group.Requests =
        [
            new RequestItem() { RequestId = "ba6350a8-fbff-43c1-9b99-ae853d24ccd2",Name = "List Directories and Files", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "bc5299e9-3ffa-4c5d-8178-0c3f79bff3fa",Name = "Create Directory", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "40f31006-5af9-44a3-a4cc-10f6cef0b17f",Name = "Get Directory Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "0fe35b2c-71ec-4143-a315-374a1d16a827",Name = "Set Directory Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "c56eb257-eca8-4aea-a49e-46c849b4ca62",Name = "Delete Directory", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "a0f4328a-5214-42ce-a392-4e7efed6935f",Name = "Get Directory Metadata", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "cc472edb-7716-4a07-8455-da69c28d1bd0",Name = "Set Directory Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "0e46e182-febf-460b-a163-99fb63e730c7",Name = "List Handles", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "56d9ebf2-5f34-4a4e-8740-b6ff2dcb056d",Name = "Force Close Handles", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "300e7812-c622-414f-a08b-f6ebd7eadb09",Name = "Rename Directory", Method = new MethodsItemViewModel("PUT")},

        ];
        collection.Groups.Add(group);

        group = new GroupItem() { Name = "Operations on Files" };
        group.Requests =
        [
            new RequestItem() { RequestId = "7b03ae8e-a105-4dd7-8200-76edbbed397c",Name = "Create File", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "1e0e7b2b-3640-4af0-85d1-ccdbd6b22083",Name = "Get File", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "4b957a6b-5332-490e-be78-e6e7714701dc",Name = "Get File Properties", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "6b03738c-96b4-4379-85f4-e6a974e0f9a3",Name = "Set File Properties", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "6434ecbe-9c13-4c61-a213-c445eaed11a5",Name = "Put Range", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "0f174895-4faf-454b-b5f9-87d4f4926b75",Name = "Put Range From URL", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "eb89bc63-19cc-4d4f-8b66-7879ec7d5327",Name = "List Ranges", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "15f4e2c0-7449-47c4-829d-2b41ec006117",Name = "Get File Metadata", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "79a5c844-c46e-4539-91da-9e1561da664c",Name = "Set File Metadata", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "cba8f8e1-e38b-47fe-9021-db1069ec200e",Name = "Delete File", Method = new MethodsItemViewModel("DELETE")},
            new RequestItem() { RequestId = "2fb2b290-5d68-4a8a-a2b6-883677bb0d89",Name = "Copy File", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "9c15a57f-a065-41ac-b5b6-0059d22e856f",Name = "Abort Copy File", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "b9b979dd-1b10-4692-9e5c-c4b90b0df736",Name = "List Handles", Method = new MethodsItemViewModel("GET")},
            new RequestItem() { RequestId = "15c0a083-2a2f-4cf8-8fdc-4dbca6175923",Name = "Force Close Handles", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "56d542e5-82e5-495e-8e23-cf1b365e0153",Name = "Lease File", Method = new MethodsItemViewModel("PUT")},
            new RequestItem() { RequestId = "1bab907a-a65f-46b9-a28d-3632089dd7f2",Name = "Rename File", Method = new MethodsItemViewModel("PUT")},

        ];
        collection.Groups.Add(group);
        Collections.Add(collection);
    }
}