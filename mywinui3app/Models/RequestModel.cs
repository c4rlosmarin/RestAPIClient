using System.Collections.ObjectModel;

namespace mywinui3app.Models;
public class RequestModel
{
    public string RequestId { get; set; }
    public string Name { get; set; }
    public string Method { get; set; }
    public URL URL { get; set; }
    public ObservableCollection<FormData> Parameters { get; set; }
    public ObservableCollection<FormData> Headers { get; set; }
    public ObservableCollection<FormData> Body { get; set; }
}

public class URL
{
    public string RawURL { get; set; }
    public string Protocol { get; set; }
    public ICollection<string> Host { get; set; }
    public ICollection<string> Path { get; set; }
    public IDictionary<string, string> Variables { get; set; }
 }

public class FormData
{
    public bool IsSelected { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}

public class Method
{
    public string Name { get; set; }
    public string Foreground { get; set; }
}