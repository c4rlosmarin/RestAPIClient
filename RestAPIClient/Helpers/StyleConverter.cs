using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace RestAPIClient.Helpers;
public class StyleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var styleKey = value as string;
        if (styleKey != null)
        {
            return Application.Current.Resources[styleKey] as Style;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var style = value as Style;
        if (style != null)
        {
            if (style == Application.Current.Resources["MyStatusCodeSuccessfulStyle"])
                return "MyStatusCodeSuccessfulStyle";
            else if (style == Application.Current.Resources["MyStatusCodeWarningStyle"])
                return "MyStatusCodeWarningStyle";
            else if (style == Application.Current.Resources["MyStatusCodeErrorStyle"])
                return "MyStatusCodeErrorStyle";
        }
        return null;
    }
}
