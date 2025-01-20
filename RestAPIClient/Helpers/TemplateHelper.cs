using System.Reflection;
using System.Text.Json;
using RestAPIClient.Models;

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

