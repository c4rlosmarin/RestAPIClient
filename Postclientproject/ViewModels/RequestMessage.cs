namespace PostClient.ViewModels;

public class RequestMessage
{
    public Command Name
    {
        get;
    }
    public ParameterItem? ParameterItem
    {
        get;
    }
    public HeaderItem? HeaderItem
    {
        get;
    }
    public BodyItem? BodyItem
    {
        get;
    }

    public RequestMessage(Command commandName, ParameterItem? parameterItem = null, HeaderItem? headerItem = null, BodyItem? bodyItem = null)
    {
        Name = commandName;
        ParameterItem = parameterItem;
        HeaderItem = headerItem;
        BodyItem = bodyItem;
    }
}
