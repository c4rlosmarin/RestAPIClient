using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using RestAPIClient.Models;
using RestAPIClient.ViewModels;

namespace RestAPIClient.Helpers;

public class TemplateHelper
{
    public RequestModel GetExistingRequestFromTemplate(string RequestId, AzureService azureService)
    {
        var resourcePath = "RestAPIClient.Templates." + azureService;
        var assembly = Assembly.GetExecutingAssembly();
        string[] allResources = assembly.GetManifestResourceNames();

        foreach (var resource in allResources)
        {
            if (resource.StartsWith(resourcePath) && resource.EndsWith(RequestId + ".json"))
            {
                using var stream = assembly.GetManifestResourceStream(resource);
                using StreamReader reader = new StreamReader(stream);
                var jsonString = reader.ReadToEnd();
                //Debug.WriteLine(jsonString);
                return JsonSerializer.Deserialize<RequestModel>(jsonString);
            }
        }
        return null;
    }
}
