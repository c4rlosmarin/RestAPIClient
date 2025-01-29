using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using RestAPIClient.Models;
using RestAPIClient.ViewModels;

namespace RestAPIClient.Helpers;

public class TemplateHelper
{
    public RequestModel GetTemplate(string RestAPIName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "RestAPIClient.Templates." + RestAPIName + ".json";

        using var stream = assembly.GetManifestResourceStream(resourceName);

        using StreamReader reader = new StreamReader(stream);
        var jsonString = reader.ReadToEnd();
        Debug.WriteLine(jsonString);
        return JsonSerializer.Deserialize<RequestModel>(jsonString);
    }

    public RequestModel GetExistingRequestFromTemplate(string RequestId)
    {
        switch (RequestId)
        {
            case "975b53b7-f48f-4682-8434-893f5a324278":
                return GetTemplate("BlobService.OperationsOnTheAccount.ListContainers");
            case "e192945c-7d70-49c6-8e50-521a2f7f01c2":
                return GetTemplate("BlobService.OperationsOnTheAccount.SetBlobServiceProperties");
            case "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94":
                return GetTemplate("BlobService.OperationsOnTheAccount.GetBlobServiceProperties");
            case "e4e5c062-82a1-4282-a571-19d5acdec6d3":
                return GetTemplate("BlobService.OperationsOnTheAccount.PreflightBlobRequest");
            case "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17":
                return GetTemplate("BlobService.OperationsOnTheAccount.GetBlobServiceStats");
            case "30e6c366-35d0-47e5-9a5c-def2ee2cfe31":
                return GetTemplate("BlobService.OperationsOnTheAccount.GetAccountInformation");
            case "53d9941c-dcc4-48b8-a717-24064108ecda":
                return GetTemplate("BlobService.OperationsOnTheAccount.GetUserDelegationKey");
            case "042368e5-65ec-4a73-8462-86b65be8c353":
                return GetTemplate("BlobService.OperationsOnContainers.CreateContainer");
            case "18fd3171-d359-4e1e-bd48-44ea44ee842c":
                return GetTemplate("BlobService.OperationsOnContainers.GetContainerProperties");
            case "2117dd4c-e41f-4c15-b5c3-2b266ca3c225":
                return GetTemplate("BlobService.OperationsOnContainers.GetContainerMetadata");
            case "78f4583c-ced5-4b21-98a6-e82690abd782":
                return GetTemplate("BlobService.OperationsOnContainers.SetContainerMetadata");
            case "4944ad75-ad15-4ae2-bfd7-79d8051c427c":
                return GetTemplate("BlobService.OperationsOnContainers.GetContainerACL");
            case "9f93eb42-cc6b-414e-8156-c39828f8d2d3":
                return GetTemplate("BlobService.OperationsOnContainers.SetContainerACL");
            case "7661e957-a0ba-4188-a53d-4decd8672dac":
                return GetTemplate("BlobService.OperationsOnContainers.DeleteContainer");
            case "7a2948eb-9751-4f53-b7cc-272284108344":
                return GetTemplate("BlobService.OperationsOnContainers.LeaseContainer");
            case "76dbc6cf-f1ba-49af-ac15-b0447114d933":
                return GetTemplate("BlobService.OperationsOnContainers.RestoreContainer");
            case "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e":
                return GetTemplate("BlobService.OperationsOnContainers.ListBlobs");
            case "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92":
                return GetTemplate("BlobService.OperationsOnContainers.FindBlobsByTagsInContainer");
            case "1fd54291-9b89-4348-8f08-85ecce5114ae":
                return GetTemplate("BlobService.OperationsOnBlobs.PutBlob");
            case "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e":
                return GetTemplate("BlobService.OperationsOnBlobs.PutBlobFromURL");
            case "18e8d3cf-77b7-471c-b868-782f49afdaf5":
                return GetTemplate("BlobService.OperationsOnBlobs.GetBlob");
            case "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd":
                return GetTemplate("BlobService.OperationsOnBlobs.GetBlobProperties");
            case "186bbfdd-abcb-4003-9efe-6504986bcff5":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobProperties");
            case "d3a570a7-c5bc-4a20-ac53-29d8631e8445":
                return GetTemplate("BlobService.OperationsOnBlobs.GetBlobMetadata");
            case "1a971155-adfa-47eb-b739-5aa04551e13c":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobMetadata");
            case "d992088f-86b7-4268-a697-a1e7ec719fb4":
                return GetTemplate("BlobService.OperationsOnBlobs.GetBlobTags");
            case "b2dde2c7-86ec-4667-a5db-8c75118f175a":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobTags");
            case "a1bcea4c-3c36-40ba-b133-b841d6a5bf06":
                return GetTemplate("BlobService.OperationsOnBlobs.FindBlobsByTags");
            case "061c4c88-981e-482f-bbd8-356f3594484c":
                return GetTemplate("BlobService.OperationsOnBlobs.LeaseBlob");
            case "bf386510-9394-4720-8769-b712a6ea08fb":
                return GetTemplate("BlobService.OperationsOnBlobs.SnapshotBlob");
            case "8f5e0e97-744c-4218-bdd3-8f45ea545e95":
                return GetTemplate("BlobService.OperationsOnBlobs.CopyBlob");
            case "9487af8f-941f-40c0-8671-98e8a461d0ad":
                return GetTemplate("BlobService.OperationsOnBlobs.CopyBlobFromURL");
            case "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c":
                return GetTemplate("BlobService.OperationsOnBlobs.AbortCopyBlob");
            case "faee6c9b-b52c-4ccd-844d-1425f2b81663":
                return GetTemplate("BlobService.OperationsOnBlobs.DeleteBlob");
            case "799b6e8b-3aab-4521-9507-2fec5b7e894a":
                return GetTemplate("BlobService.OperationsOnBlobs.UndeleteBlob");
            case "16744da0-054b-4fea-baee-2a8c0e4ee36f":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobTier");
            case "27400b89-4193-4804-9c97-8fc98bd4ec73":
                return GetTemplate("BlobService.OperationsOnBlobs.BlobBatch");
            case "965b3e05-3adc-4413-ba39-cf82ab6e26db":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobImmutabilityPolicy");
            case "7e7c8aba-d49d-4de2-bb62-d1507cf00030":
                return GetTemplate("BlobService.OperationsOnBlobs.DeleteBlobImmutabilityPolicy");
            case "2548c991-90c9-4159-98a8-c6d076787b01":
                return GetTemplate("BlobService.OperationsOnBlobs.SetBlobLegalHold");
            case "6ccb4aba-4209-4192-aa87-a0c24f9e26bf":
                return GetTemplate("DataLakeStorageGen2.OperationsOnFileSystem.Create");
            case "57dc4b51-a0d4-4917-8ba9-c7acde42cb71":
                return GetTemplate("DataLakeStorageGen2.OperationsOnFileSystem.Delete");
            case "e8f351eb-7408-4913-bf19-8002ea9da72d":
                return GetTemplate("DataLakeStorageGen2.OperationsOnFileSystem.GetProperties");
            case "232cc021-77e1-4b8e-a7cd-88bd8d1434c7":
                return GetTemplate("DataLakeStorageGen2.OperationsOnFileSystem.List");
            case "e71bf0b4-b2eb-4325-a8f0-d51d402124c7":
                return GetTemplate("DataLakeStorageGen2.OperationsOnFileSystem.SetProperties");
            case "b418189f-0b7e-4e5d-a85f-5305eea05bd7":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.Create");
            case "3c43a681-da19-4303-8388-70b82771cdea":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.Delete");
            case "978153ac-94cd-4c35-8b1f-b8044109a7f9":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.GetProperties");
            case "351df3a9-3e1f-425e-af99-31d94888fad8":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.Lease");
            case "b9bbf718-1be0-4d6e-b6ca-941af99a1383":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.List");
            case "fd373385-ac1d-49a0-bf38-e79ab98036a5":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.Read");
            case "9f787102-8ff4-419b-af44-de620d842ebc":
                return GetTemplate("DataLakeStorageGen2.OperationsOnPath.Update");
            case "fda74fd0-e408-4c6d-894d-4e5dba70afe5":
                return GetTemplate("FileService.OperationsOnFileService.GetFileServiceProperties");
            case "d75480b6-7625-4c80-9252-519df187629d":
                return GetTemplate("FileService.OperationsOnFileService.SetFileServiceProperties");
            case "7345d46d-1656-4b01-b859-abed3c8346a9":
                return GetTemplate("FileService.OperationsOnFileService.PreflightFileRequest");
            case "3efdeb4d-15ad-499a-8116-1f6609c6f74b":
                return GetTemplate("FileService.OperationsOnFileShares.ListShares");
            case "b83808d0-c15f-498e-b783-4e18aff7e7a4":
                return GetTemplate("FileService.OperationsOnFileShares.CreateShare");
            case "d36d37f4-084a-444c-8412-461a2bea7033":
                return GetTemplate("FileService.OperationsOnFileShares.SnapshotShare");
            case "1632ab9f-de7e-4d89-af34-3aa083581c65":
                return GetTemplate("FileService.OperationsOnFileShares.GetShareProperties");
            case "84390458-9e19-4bcf-8fd0-0170dc37ce58":
                return GetTemplate("FileService.OperationsOnFileShares.SetShareProperties");
            case "d5628fe9-a983-4f2e-89ae-b1a9582e504b":
                return GetTemplate("FileService.OperationsOnFileShares.GetShareMetadata");
            case "b93b91c6-c4f4-407d-9e94-caa2a0660027":
                return GetTemplate("FileService.OperationsOnFileShares.SetShareMetadata");
            case "79f1c6f1-3acd-463b-ae75-099464837801":
                return GetTemplate("FileService.OperationsOnFileShares.DeleteShare");
            case "5a6d906f-e182-4664-8289-785bc64967a3":
                return GetTemplate("FileService.OperationsOnFileShares.RestoreShare");
            case "040789bc-8f8e-4f6b-9c22-60197ddac6e0":
                return GetTemplate("FileService.OperationsOnFileShares.GetShareACL");
            case "63a6d979-b9b6-4cf3-9d96-185cc8812455":
                return GetTemplate("FileService.OperationsOnFileShares.SetShareACL");
            case "c1a70342-0109-4f6c-b631-d0733566fcc0":
                return GetTemplate("FileService.OperationsOnFileShares.GetShareStats");
            case "82917fc6-6b51-4a71-a907-dfca8ba1f693":
                return GetTemplate("FileService.OperationsOnFileShares.CreatePermission");
            case "c2127ec5-d23e-43ee-beff-9cdc1bd84ffb":
                return GetTemplate("FileService.OperationsOnFileShares.GetPermission");
            case "03a0cfd5-e1cd-4c36-b4c3-5219c861279f":
                return GetTemplate("FileService.OperationsOnFileShares.LeaseShare");
            case "ba6350a8-fbff-43c1-9b99-ae853d24ccd2":
                return GetTemplate("FileService.OperationsOnDirectories.ListDirectoriesAndFiles");
            case "bc5299e9-3ffa-4c5d-8178-0c3f79bff3fa":
                return GetTemplate("FileService.OperationsOnDirectories.CreateDirectory");
            case "40f31006-5af9-44a3-a4cc-10f6cef0b17f":
                return GetTemplate("FileService.OperationsOnDirectories.GetDirectoryProperties");
            case "0fe35b2c-71ec-4143-a315-374a1d16a827":
                return GetTemplate("FileService.OperationsOnDirectories.SetDirectoryProperties");
            case "c56eb257-eca8-4aea-a49e-46c849b4ca62":
                return GetTemplate("FileService.OperationsOnDirectories.DeleteDirectory");
            case "a0f4328a-5214-42ce-a392-4e7efed6935f":
                return GetTemplate("FileService.OperationsOnDirectories.GetDirectoryMetadata");
            case "cc472edb-7716-4a07-8455-da69c28d1bd0":
                return GetTemplate("FileService.OperationsOnDirectories.SetDirectoryMetadata");
            case "0e46e182-febf-460b-a163-99fb63e730c7":
                return GetTemplate("FileService.OperationsOnDirectories.ListHandles");
            case "56d9ebf2-5f34-4a4e-8740-b6ff2dcb056d":
                return GetTemplate("FileService.OperationsOnDirectories.ForceCloseHandles");
            case "300e7812-c622-414f-a08b-f6ebd7eadb09":
                return GetTemplate("FileService.OperationsOnDirectories.RenameDirectory");
            case "7b03ae8e-a105-4dd7-8200-76edbbed397c":
                return GetTemplate("FileService.OperationsOnFiles.CreateFile");
            case "1e0e7b2b-3640-4af0-85d1-ccdbd6b22083":
                return GetTemplate("FileService.OperationsOnFiles.GetFile");
            case "4b957a6b-5332-490e-be78-e6e7714701dc":
                return GetTemplate("FileService.OperationsOnFiles.GetFileProperties");
            case "6b03738c-96b4-4379-85f4-e6a974e0f9a3":
                return GetTemplate("FileService.OperationsOnFiles.SetFileProperties");
            case "6434ecbe-9c13-4c61-a213-c445eaed11a5":
                return GetTemplate("FileService.OperationsOnFiles.PutRange");
            case "0f174895-4faf-454b-b5f9-87d4f4926b75":
                return GetTemplate("FileService.OperationsOnFiles.PutRangeFromURL");
            case "eb89bc63-19cc-4d4f-8b66-7879ec7d5327":
                return GetTemplate("FileService.OperationsOnFiles.ListRanges");
            case "15f4e2c0-7449-47c4-829d-2b41ec006117":
                return GetTemplate("FileService.OperationsOnFiles.GetFileMetadata");
            case "79a5c844-c46e-4539-91da-9e1561da664c":
                return GetTemplate("FileService.OperationsOnFiles.SetFileMetadata");
            case "cba8f8e1-e38b-47fe-9021-db1069ec200e":
                return GetTemplate("FileService.OperationsOnFiles.DeleteFile");
            case "2fb2b290-5d68-4a8a-a2b6-883677bb0d89":
                return GetTemplate("FileService.OperationsOnFiles.CopyFile");
            case "9c15a57f-a065-41ac-b5b6-0059d22e856f":
                return GetTemplate("FileService.OperationsOnFiles.AbortCopyFile");
            case "b9b979dd-1b10-4692-9e5c-c4b90b0df736":
                return GetTemplate("FileService.OperationsOnFiles.ListHandles");
            case "15c0a083-2a2f-4cf8-8fdc-4dbca6175923":
                return GetTemplate("FileService.OperationsOnFiles.ForceCloseHandles");
            case "56d542e5-82e5-495e-8e23-cf1b365e0153":
                return GetTemplate("FileService.OperationsOnFiles.LeaseFile");
            case "1bab907a-a65f-46b9-a28d-3632089dd7f2":
                return GetTemplate("FileService.OperationsOnFiles.RenameFile");
            case "40059afb-187a-4fee-ac88-66e95c1bfdd7":
                return GetTemplate("QueueService.OperationsOnTheAccount.ListQueues");
            case "98fe8758-72fa-44cb-b9b5-e515162fc972":
                return GetTemplate("QueueService.OperationsOnTheAccount.SetQueueServiceProperties");
            case "027f0ce7-3a93-4e8c-ba1e-7451a15e658d":
                return GetTemplate("QueueService.OperationsOnTheAccount.GetQueueServiceProperties");
            case "279c3bfe-2a3c-47e0-81bb-007e5a6d2dd5":
                return GetTemplate("QueueService.OperationsOnTheAccount.PreflightQueueRequest");
            case "f386215a-fc2a-4f88-ba26-e12fe970c39e":
                return GetTemplate("QueueService.OperationsOnTheAccount.GetQueueServiceStats");
            case "80aece03-d070-4c05-8de3-0ca847cbade8":
                return GetTemplate("QueueService.OperationsOnQueues.CreateQueue");
            case "9b780a7a-a31d-4b79-a45b-9a31788effc5":
                return GetTemplate("QueueService.OperationsOnQueues.DeleteQueue");
            case "ffece7ea-ab8e-42f5-ae95-78966ea06c2d":
                return GetTemplate("QueueService.OperationsOnQueues.GetQueueMetadata");
            case "aa7c031e-10a3-4182-bdc7-abd10d8565a8":
                return GetTemplate("QueueService.OperationsOnQueues.SetQueueMetadata");
            case "931fcb2f-184c-4398-abfa-d8fc8223690a":
                return GetTemplate("QueueService.OperationsOnQueues.GetQueueACL");
            case "675b45bd-3aed-484d-8930-6a3f7d1c8645":
                return GetTemplate("QueueService.OperationsOnQueues.SetQueueACL");
            case "fcb835d4-b764-45c4-93c3-be2fc3c13096":
                return GetTemplate("QueueService.OperationsOnMessages.PutMessage");
            case "d690b5ea-04c0-4a14-843a-5f5c2b80edb9":
                return GetTemplate("QueueService.OperationsOnMessages.GetMessages");
            case "58b52aaa-674f-4346-af9a-c0279869cd52":
                return GetTemplate("QueueService.OperationsOnMessages.PeekMessages");
            case "3c8aa12b-0e0d-478d-868c-0c14a05ba465":
                return GetTemplate("QueueService.OperationsOnMessages.DeleteMessage");
            case "77846541-5bd7-489c-aa06-4ebb7aa30ec3":
                return GetTemplate("QueueService.OperationsOnMessages.ClearMessages");
            case "23037207-f969-4ac4-af83-8e1bb957dff4":
                return GetTemplate("QueueService.OperationsOnMessages.UpdateMessage");

            case "06b8cdc5-77f8-4922-aca0-ab1ee578cd81":
                return GetTemplate("TableService.OperationsOnTheAccount.SetTableServiceProperties");
            case "70e2c40c-2f34-4e8d-b896-a655bebe345f":
                return GetTemplate("TableService.OperationsOnTheAccount.GetTableServiceProperties");
            case "6f2cba8a-4a34-4503-a42c-1d0304c83bf6":
                return GetTemplate("TableService.OperationsOnTheAccount.PreflightTableRequest");
            case "02d1bed7-aba2-4b6d-9895-46909219b064":
                return GetTemplate("TableService.OperationsOnTheAccount.GetTableServiceStats");
            case "7956fedd-b747-472a-a4da-9d05dbe47c3e":
                return GetTemplate("TableService.OperationsOnTables.QueryTables");
            case "a436a46c-2e0b-4247-96a0-ab4d77c21089":
                return GetTemplate("TableService.OperationsOnTables.CreateTable");
            case "e235ad9a-443b-447b-917d-ffc8afac34ad":
                return GetTemplate("TableService.OperationsOnTables.DeleteTable");
            case "35094808-c0d0-447f-834c-fcbb3ac763a3":
                return GetTemplate("TableService.OperationsOnTables.GetTableACL");
            case "6cb7516c-3eb4-4ae7-8dbf-04426fc6a16e":
                return GetTemplate("TableService.OperationsOnTables.SetTableACL");
            case "60de20cd-274f-4d84-b50e-92233bc3ae57":
                return GetTemplate("TableService.OperationsOnEntities.QueryEntities");
            case "7b95547a-e08c-4d7e-b673-6067023d06bf":
                return GetTemplate("TableService.OperationsOnEntities.InsertEntity");
            case "c9f5c60a-fdb9-4688-ab81-9d02085b2eb3":
                return GetTemplate("TableService.OperationsOnEntities.UpdateEntity");
            case "f810ebdb-67d5-4038-942d-7abf6a457442":
                return GetTemplate("TableService.OperationsOnEntities.MergeEntity");
            case "e606e471-754b-4306-978b-8042d9690ea2":
                return GetTemplate("TableService.OperationsOnEntities.DeleteEntity");
            case "86a144b6-976e-4d19-a74d-e2220175c131":
                return GetTemplate("TableService.OperationsOnEntities.InsertOrReplaceEntity");
            case "cf2e2dc3-a646-46b1-b0c8-59e100cc733c":
                return GetTemplate("TableService.OperationsOnEntities.InsertOrMergeEntity");

            default:
                return null;


                //RequestModel requestModel;
                //switch (request.RequestId)
                //{

                //    case "9f787102-8ff4-419b-af44-de620d842ebc":

                //        requestModel = new RequestModel
                //        {
                //            Name = "Path - Update",
                //            URL = new URL() { RawURL = "https://{accountName}.{dnsSuffix}/{filesystem}/{path}?action={action}" },
                //            SelectedMethod = Methods.FirstOrDefault(item => item.Name == "PATCH"),
                //            IsMethodComboEnabled = "false",
                //            TabIconVisibility = "Collapsed",
                //            Parameters = new ObservableCollection<ParameterItem>() {
                //                new(_messenger) { IsEnabled = true, Key = "action", Value="{action}", Description="Required. The action must be \"append\" to upload data to be appended to a file, \"flush\" to flush previously uploaded data to a file, \"setProperties\" to set the properties of a file or directory, or \"setAccessControl\" to set the owner, group, permissions, or access control list for a file or directory, or \"setAccessControlRecursive\" to set the access control list for a directory recursively. Note that Hierarchical Namespace must be enabled for the account in order to use access control. Also note that the Access Control List (ACL) includes permissions for the owner, owning group, and others, so the x-ms-permissions and x-ms-acl request headers are mutually exclusive.", IsEnabledActive="false", DeleteButtonVisibility = "Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //            Headers = new ObservableCollection<HeaderItem>() {
                //                new(_messenger) { IsEnabled = false, Key = "Authorization", Description = "Specifies the authorization scheme, storage account name, and signature. For more information, see Authorize requests to Azure Storage.", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //                new(_messenger) { IsEnabled = false, Key = "x-ms-date", Description = "Specifies the Coordinated Universal Time (UTC) for the request. This is required when using shared key authorization.", UTCVisibility="Visible", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //                new(_messenger) { IsEnabled = false, Key = "x-ms-version", Description = "Specifies the version of the REST protocol used for processing the request. This is required when using shared key authorization.", DatePickerButtonVisibility="Visible",DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"}},
                //                new(_messenger) { IsEnabled = false, Key = "x-ms-client-request-id", Description = "A UUID recorded in the analytics logs for troubleshooting and correlation.\r\n\r\nRegex pattern: ^[{(]?[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}[)}]?$", DeleteButtonVisibility="Collapsed", IsKeyReadyOnly = "true", IsDescriptionReadyOnly = "true"},
                //            IsBodyComboEnabled = "true",
                //            SelectedBodyType = "Text",

                //            Response = new ResponseViewModel(),
                //        };
                //        requestModel.Response.Visibility = "Collapsed";

                //        string jsonString = JsonSerializer.Serialize(requestModel);
                //        Debug.WriteLine(jsonString);

                //        break;
                //}
        }
    }
}

