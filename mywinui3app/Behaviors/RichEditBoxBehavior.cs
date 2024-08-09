using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace mywinui3app.Behaviors;

public class RichEditBoxBehavior : Behavior<RichEditBox>
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(RichEditBoxBehavior), new PropertyMetadata(string.Empty, OnTextChanged));

    private bool _isUpdating;

    public string Text
    {
        get
        {
            return (string)GetValue(TextProperty);
        }
        set
        {
            SetValue(TextProperty, value);
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = d as RichEditBoxBehavior;
        if (behavior?.AssociatedObject != null && !behavior._isUpdating)
        {
            behavior.UpdateDocumentText((string)e.NewValue);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        AssociatedObject.LostFocus += AssociatedObject_LostFocus;
        UpdateDocumentText(Text);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
    }

    private void AssociatedObject_TextChanged(object sender, RoutedEventArgs e)
    {
        if (!_isUpdating)
        {
            _isUpdating = true;
            AssociatedObject.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out string text);
            Text = text.TrimEnd('\r', '\n');
            _isUpdating = false;
        }
    }

    private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
    {
        // Clear the document to release resources
        AssociatedObject.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, string.Empty);
    }

    private void UpdateDocumentText(string text)
    {
        _isUpdating = true;
        AssociatedObject.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, text.TrimEnd('\r', '\n'));
        _isUpdating = false;
    }
}

