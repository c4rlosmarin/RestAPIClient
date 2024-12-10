using System.Globalization;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Postclient.Helpers;
public static class ColorHelper
{
    public static Color ConvertHexToColor(string hex)
    {
        hex = hex.Replace("#", string.Empty);

        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            r = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
        }

        return Color.FromArgb(a, r, g, b);
    }

    public static SolidColorBrush CreateSolidColorBrushFromHex(string hex)
    {
        Color color = ConvertHexToColor(hex);
        return new SolidColorBrush(color);
    }
}
