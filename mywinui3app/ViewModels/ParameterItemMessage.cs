namespace mywinui3app.ViewModels;
public class ParameterItemMessage
{
    public ParameterItem Item
    {
        get;
    }

    public string PropertyName
    {
        get;
    }

    public ExecutedOperation Operation
    {
        get;
    }

    public ParameterItemMessage(ParameterItem item, string PropertyName, ExecutedOperation operation)
    {
        this.Item = item;
        this.PropertyName = PropertyName;
        this.Operation = operation;
    }
}

public enum ExecutedOperation
{
    Modified,
    Deleted
}
