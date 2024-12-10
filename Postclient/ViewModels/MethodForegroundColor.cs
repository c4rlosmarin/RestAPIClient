namespace Postclient.ViewModels;

public static class ForegroundColorHelper
{
    public static string GET = "#22B14C";
    public static string POST = "#218BEB";
    public static string PUT = "#FF8C00";
    public static string PATCH = "#A347D1";
    public static string DELETE = "#ED2B2B";
    public static string OPTIONS = "#FF6593";
    public static string HEAD = "#6BA10C";

    public static string GetColorByMethod(string method)
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

public enum Method
{
    GET,
    POST,
    PUT,
    PATCH,
    DELETE,
    OPTIONS,
    HEAD
}