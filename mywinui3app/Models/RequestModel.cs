namespace mywinui3app.Models;
public class RequestModel
{
    public string RequestId { get; set; }
    public string Name { get; set; }
    public string Method { get; set; }
    public URL URL { get; set; }
    public Parameters Parameters { get; set; }
    public Parameters Headers { get; set; }
    public Body Body { get; set; }

}

public class URL
{
    public string RawURL { get; set; }
    public string Protocol { get; set; }
    public ICollection<string> Host { get; set; }
    public ICollection<string> Path { get; set; }
    public IDictionary<string, string> Variables { get; set; }
 }

public class Parameters
{
    public ICollection<FormData> Items { get; set; }
}

public class Headers
{
    public ICollection<FormData> Items { get; set; }
}


public class Body
{
    public string Mode { get; set; }
    public ICollection<FormData> Items { get; set; }
}

public class FormData
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}