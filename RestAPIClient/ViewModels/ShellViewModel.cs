using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using RestAPIClient.Contracts.Services;
using RestAPIClient.Views;

namespace RestAPIClient.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    ObservableCollection<NavigationMenuItem> navigationMenuItems;

    [ObservableProperty]
    ObservableCollection<NavigationMenuItem> navigationFooterItems;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    private void InitializeServices()
    {
        NavigationMenuItems = new ObservableCollection<NavigationMenuItem>();

        var collectionsIcon = new FontIcon { Glyph = "\uE8A4", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        var collectionsMenuItem = new NavigationMenuItem() { Content = "Collections", FontIcon = collectionsIcon, SubMenus = new ObservableCollection<NavigationMenuItem>() };
        var azureTemplatesMenuItem = new NavigationMenuItem() { Content = "Azure Templates", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/Azure.png")) }, Margin=new(-20,0,0,0)};
        collectionsMenuItem.SubMenus.Add(azureTemplatesMenuItem);

        var serviceMenuItem = new NavigationMenuItem() { Content = "Storage Services", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Accounts.png")) }, Margin = new(-20, 0, 0, 0) };

        azureTemplatesMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        azureTemplatesMenuItem.SubMenus.Add(serviceMenuItem);

        serviceMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        var restApiMenuItem = new NavigationMenuItem() { Content = "Blob Service REST API", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Azure-Blob-Block.png")) }, Margin = new(-20, 0, 0, 0) };
        restApiMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        serviceMenuItem.SubMenus.Add(restApiMenuItem);

        var categoryMenuItem = new NavigationMenuItem() { Content = "Operations on the account (Blob service)", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "975b53b7-f48f-4682-8434-893f5a324278",Name = "List Containers",Content = "List Containers", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "e192945c-7d70-49c6-8e50-521a2f7f01c2",Name = "Set Blob Service Properties", Content = "Set Blob Service Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "be60e8b4-7cd1-4f83-a9cb-4a3b92303b94",Name = "Get Blob Service Properties", Content = "Get Blob Service Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "e4e5c062-82a1-4282-a571-19d5acdec6d3",Name = "Preflight Blob Request", Content = "Preflight Blob Request", Method = new MethodsItemViewModel("OPTIONS"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "9cc6eb22-c1d3-4ee0-b076-b4c58d4feb17",Name = "Get Blob Service Stats", Content = "Get Blob Service Stats", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "30e6c366-35d0-47e5-9a5c-def2ee2cfe31",Name = "Get Account Information", Content = "Get Account Information", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "53d9941c-dcc4-48b8-a717-24064108ecda",Name = "Get User Delegation Key", Content = "Get User Delegation Key", Method = new MethodsItemViewModel("POST"), Margin = new(-20, 0, 0, 0), IsRequest=true }
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on Containers", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "042368e5-65ec-4a73-8462-86b65be8c353",Name = "Create Container",Content = "Create Container", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "18fd3171-d359-4e1e-bd48-44ea44ee842c",Name = "Get Container Properties", Content = "Get Container Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "2117dd4c-e41f-4c15-b5c3-2b266ca3c225",Name = "Get Container Metadata", Content = "Get Container Metadata", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "78f4583c-ced5-4b21-98a6-e82690abd782",Name = "Set Container Metadata", Content = "Set Container Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "4944ad75-ad15-4ae2-bfd7-79d8051c427c",Name = "Get Container ACL", Content = "Get Container ACL", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "9f93eb42-cc6b-414e-8156-c39828f8d2d3",Name = "Set Container ACL", Content = "Set Container ACL", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "7661e957-a0ba-4188-a53d-4decd8672dac",Name = "Delete Container", Content = "Delete Container", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "7a2948eb-9751-4f53-b7cc-272284108344",Name = "Lease Container", Content = "Lease Container", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "76dbc6cf-f1ba-49af-ac15-b0447114d933",Name = "Restore Container", Content = "Restore Container", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "a8bd3fb3-f339-464f-97fe-3e6d8c94c18e",Name = "List Blobs", Content = "List Blobs", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "5a0ff8b0-02f2-4d62-be05-aa2f2f83cd92",Name = "Find Blobs by Tags in Container", Content = "Find Blobs by Tags in Container", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true }
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on Blobs", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "1fd54291-9b89-4348-8f08-85ecce5114ae",Name = "Put Blob",Content = "Put Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "acbdfae5-c66b-44ea-8d89-6b5ff4b27e8e",Name = "Put Blob From URL",Content = "Put Blob From URL", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "18e8d3cf-77b7-471c-b868-782f49afdaf5",Name = "Get Blob",Content = "Get Blob", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "a4d7a27b-670d-4ad4-8168-f81db1a9a0dd",Name = "Get Blob Properties",Content = "Get Blob Properties", Method = new MethodsItemViewModel("HEAD"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "186bbfdd-abcb-4003-9efe-6504986bcff5",Name = "Set Blob Properties",Content = "Set Blob Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "d3a570a7-c5bc-4a20-ac53-29d8631e8445",Name = "Get Blob Metadata",Content = "Get Blob Metadata", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "1a971155-adfa-47eb-b739-5aa04551e13c",Name = "Set Blob Metadata",Content = "Set Blob Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "d992088f-86b7-4268-a697-a1e7ec719fb4",Name = "Get Blob Tags",Content = "Get Blob Tags", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "b2dde2c7-86ec-4667-a5db-8c75118f175a",Name = "Set Blob Tags",Content = "Set Blob Tags", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "a1bcea4c-3c36-40ba-b133-b841d6a5bf06",Name = "Find Blobs by Tags",Content = "Find Blobs by Tags", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "061c4c88-981e-482f-bbd8-356f3594484c",Name = "Lease Blob",Content = "Lease Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "bf386510-9394-4720-8769-b712a6ea08fb",Name = "Snapshot Blob",Content = "Snapshot Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "8f5e0e97-744c-4218-bdd3-8f45ea545e95",Name = "Copy Blob",Content = "Copy Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "9487af8f-941f-40c0-8671-98e8a461d0ad",Name = "Copy Blob From URL",Content = "Copy Blob From URL", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "ba127e8f-faee-4c97-9a5f-908e3f5b3a2c",Name = "Abort Copy Blob",Content = "Abort Copy Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "faee6c9b-b52c-4ccd-844d-1425f2b81663",Name = "Delete Blob",Content = "Delete Blob", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "799b6e8b-3aab-4521-9507-2fec5b7e894a",Name = "Undelete Blob",Content = "Undelete Blob", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "16744da0-054b-4fea-baee-2a8c0e4ee36f",Name = "Set Blob Tier",Content = "Set Blob Tier", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "27400b89-4193-4804-9c97-8fc98bd4ec73",Name = "Blob Batch",Content = "Blob Batch", Method = new MethodsItemViewModel("POST"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "965b3e05-3adc-4413-ba39-cf82ab6e26db",Name = "Set Blob Immutability Policy",Content = "Set Blob Immutability Policy", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "7e7c8aba-d49d-4de2-bb62-d1507cf00030",Name = "Delete Blob Immutability Policy",Content = "Delete Blob Immutability Policy", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "2548c991-90c9-4159-98a8-c6d076787b01",Name = "Set Blob Legal Hold",Content = "Set Blob Legal Hold", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        restApiMenuItem = new NavigationMenuItem() { Content = "Queue Service REST API", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Azure-Queue.png")) }, Margin = new(-20, 0, 0, 0) };
        restApiMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        serviceMenuItem.SubMenus.Add(restApiMenuItem);

        restApiMenuItem = new NavigationMenuItem() { Content = "Table Service REST API", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Azure-Table.png")) }, Margin = new(-20, 0, 0, 0) };
        restApiMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        serviceMenuItem.SubMenus.Add(restApiMenuItem);

        restApiMenuItem = new NavigationMenuItem() { Content = "File Service REST API", ImageIcon = new ImageIcon { Source = new BitmapImage(new Uri("ms-appx:///Assets/AzureServiceLogos/Storage-Azure-Files.png")) }, Margin = new(-20, 0, 0, 0) };
        restApiMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        serviceMenuItem.SubMenus.Add(restApiMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on FileService", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "fda74fd0-e408-4c6d-894d-4e5dba70afe5",Name = "Get File Service Properties",Content = "Get File Service Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "d75480b6-7625-4c80-9252-519df187629d",Name = "Set File Service Properties",Content = "Set File Service Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "7345d46d-1656-4b01-b859-abed3c8346a9",Name = "Preflight File Request",Content = "Preflight File Request", Method = new MethodsItemViewModel("OPTIONS"), Margin = new(-20, 0, 0, 0), IsRequest=true },
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on FileShares", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "3efdeb4d-15ad-499a-8116-1f6609c6f74b",Name = "List Shares",Content = "List Shares", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "b83808d0-c15f-498e-b783-4e18aff7e7a4",Name = "Create Share",Content = "Create Share", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "d36d37f4-084a-444c-8412-461a2bea7033",Name = "Snapshot Share",Content = "Snapshot Share", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "1632ab9f-de7e-4d89-af34-3aa083581c65",Name = "Get Share Properties",Content = "Get Share Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "84390458-9e19-4bcf-8fd0-0170dc37ce58",Name = "Set Share Properties",Content = "Set Share Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "d5628fe9-a983-4f2e-89ae-b1a9582e504b",Name = "Get Share Metadata",Content = "Get Share Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "b93b91c6-c4f4-407d-9e94-caa2a0660027",Name = "Set Share Metadata",Content = "Set Share Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "79f1c6f1-3acd-463b-ae75-099464837801",Name = "Delete Share",Content = "Delete Share", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "5a6d906f-e182-4664-8289-785bc64967a3",Name = "Restore Share",Content = "Restore Share", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "040789bc-8f8e-4f6b-9c22-60197ddac6e0",Name = "Get Share ACL",Content = "Get Share ACL", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "63a6d979-b9b6-4cf3-9d96-185cc8812455",Name = "Set Share ACL",Content = "Set Share ACL", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "c1a70342-0109-4f6c-b631-d0733566fcc0",Name = "Get Share Stats",Content = "Get Share Stats", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "82917fc6-6b51-4a71-a907-dfca8ba1f693",Name = "Create Permission",Content = "Create Permission", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "c2127ec5-d23e-43ee-beff-9cdc1bd84ffb",Name = "Get Permission",Content = "Get Permission", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "03a0cfd5-e1cd-4c36-b4c3-5219c861279f",Name = "Lease Share",Content = "Lease Share", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true }
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on Directories", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "ba6350a8-fbff-43c1-9b99-ae853d24ccd2",Name = "List Directories and Files",Content = "List Directories and Files", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "bc5299e9-3ffa-4c5d-8178-0c3f79bff3fa",Name = "Create Directory",Content = "Create Directory", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "40f31006-5af9-44a3-a4cc-10f6cef0b17f",Name = "Get Directory Properties",Content = "Get Directory Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "0fe35b2c-71ec-4143-a315-374a1d16a827",Name = "Set Directory Properties",Content = "Set Directory Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "c56eb257-eca8-4aea-a49e-46c849b4ca62",Name = "Delete Directory",Content = "Delete Directory", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "a0f4328a-5214-42ce-a392-4e7efed6935f",Name = "Get Directory Metadata",Content = "Get Directory Metadata", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "cc472edb-7716-4a07-8455-da69c28d1bd0",Name = "Set Directory Metadata",Content = "Set Directory Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "0e46e182-febf-460b-a163-99fb63e730c7",Name = "List Handles",Content = "List Handles", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "56d9ebf2-5f34-4a4e-8740-b6ff2dcb056d",Name = "Force Close Handles",Content = "Force Close Handles", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "300e7812-c622-414f-a08b-f6ebd7eadb09",Name = "Rename Directory",Content = "Rename Directory", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true }
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        categoryMenuItem = new NavigationMenuItem() { Content = "Operations on Files", Margin = new(-20, 0, 0, 0) };
        categoryMenuItem.SubMenus = new ObservableCollection<NavigationMenuItem>();
        categoryMenuItem.SubMenus =
        [
            new NavigationMenuItem() { RequestId = "7b03ae8e-a105-4dd7-8200-76edbbed397c",Name = "Create File",Content = "Create File", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "1e0e7b2b-3640-4af0-85d1-ccdbd6b22083",Name = "Get File",Content = "Get File", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "4b957a6b-5332-490e-be78-e6e7714701dc",Name = "Get File Properties",Content = "Get File Properties", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "6b03738c-96b4-4379-85f4-e6a974e0f9a3",Name = "Set File Properties",Content = "Set File Properties", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "6434ecbe-9c13-4c61-a213-c445eaed11a5",Name = "Put Range",Content = "Put Range", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "0f174895-4faf-454b-b5f9-87d4f4926b75",Name = "Put Range From URL",Content = "Put Range From URL", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "eb89bc63-19cc-4d4f-8b66-7879ec7d5327",Name = "List Ranges",Content = "List Ranges", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "15f4e2c0-7449-47c4-829d-2b41ec006117",Name = "Get File Metadata",Content = "Get File Metadata", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "79a5c844-c46e-4539-91da-9e1561da664c",Name = "Set File Metadata",Content = "Set File Metadata", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "cba8f8e1-e38b-47fe-9021-db1069ec200e",Name = "Delete File",Content = "Delete File", Method = new MethodsItemViewModel("DELETE"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "2fb2b290-5d68-4a8a-a2b6-883677bb0d89",Name = "Copy File",Content = "Copy File", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "9c15a57f-a065-41ac-b5b6-0059d22e856f",Name = "Abort Copy File",Content = "Abort Copy File", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "b9b979dd-1b10-4692-9e5c-c4b90b0df736",Name = "List Handles",Content = "List Handles", Method = new MethodsItemViewModel("GET"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "15c0a083-2a2f-4cf8-8fdc-4dbca6175923",Name = "Force Close Handles",Content = "Force Close Handles", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "56d542e5-82e5-495e-8e23-cf1b365e0153",Name = "Lease File",Content = "Lease File", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
            new NavigationMenuItem() { RequestId = "1bab907a-a65f-46b9-a28d-3632089dd7f2",Name = "Rename File",Content = "Rename File", Method = new MethodsItemViewModel("PUT"), Margin = new(-20, 0, 0, 0), IsRequest=true },
        ];
        restApiMenuItem.SubMenus.Add(categoryMenuItem);

        NavigationMenuItems.Add(collectionsMenuItem);

        NavigationFooterItems = new ObservableCollection<NavigationMenuItem>();
        var aboutIcon = new FontIcon { Glyph = "\uE946", FontFamily = new FontFamily("Segoe MDL2 Assets") };
        var aboutFooterItem = new NavigationMenuItem() { Content = "About", FontIcon = aboutIcon };
        NavigationFooterItems.Add(aboutFooterItem);
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;

        InitializeServices();
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
