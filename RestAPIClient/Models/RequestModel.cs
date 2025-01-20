using System.Collections.ObjectModel;
using RestAPIClient.ViewModels;

namespace RestAPIClient.Models;
public class RequestModel
{
    public string RequestId { get; set; }
    public string Name { get; set; }
    public string TabIconVisibility { get; set; }
    public MethodsItemViewModel SelectedMethod { get; set; }
    public string IsMethodComboEnabled { get; set; }
    public URL URL { get; set; }
    public ObservableCollection<ParameterItem> Parameters { get; set; }
    public ObservableCollection<HeaderItem> Headers { get; set; }
    public string IsBodyComboEnabled { get; set; }
    public string SelectedBodyType { get; set; }
    public string RawBody { get; set; }
    public ObservableCollection<BodyItem> Body { get; set; }
    public ResponseViewModel Response { get; set; }
}
