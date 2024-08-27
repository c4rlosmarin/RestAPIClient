namespace mywinui3app.ViewModels;

public class MethodForegroundColor
{
    public string GET = "#22B14C";
    public string POST = "#218BEB";
    public string PUT = "FF8C00";
    public string PATCH = "#A347D1";
    public string DELETE = "#ED2B2B";
    public string OPTIONS = "#FF6593";
    public string HEAD = "#2E7817";

    public string GetColorByMethod(string method)
    {
        switch (method)
        {
            case "GET":
                return GET;
            case "POST":
                return POST;
            case "PUT":
                return PUT;
            case "PATCH":
                return PATCH;
            case "DELETE":
                return DELETE;
            case "OPTIONS":
                return OPTIONS;
            default:
                return HEAD;
        }
    }
}