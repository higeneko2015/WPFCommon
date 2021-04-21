using System.ComponentModel;
using System.Windows;
using System.Windows.Shell;

namespace WPFCommon
{
    public class BaseWindowBehavior
    {
        public static readonly DependencyProperty BaseWindowProperty = DependencyProperty.RegisterAttached(
            "BaseWindow",
            typeof(bool),
            typeof(BaseWindowBehavior),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender, OnShowMessage));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        [Category("WPFCommon")]
        [DisplayName("")]
        [Description("説明です")]
        public static bool GetBaseWindow(DependencyObject target)
        {
            return (bool)target.GetValue(BaseWindowProperty);
        }

        public static void SetBaseWindow(DependencyObject target, bool value)
        {
            target.SetValue(BaseWindowProperty, value);
        }

        private static void OnShowMessage(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not Window element)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                var chrome = new WindowChrome
                {
                    CaptionHeight = 28,
                    CornerRadius = new CornerRadius(0),
                    GlassFrameThickness = new Thickness(1),
                    ResizeBorderThickness = new Thickness(5),
                };
                WindowChrome.SetWindowChrome(element, chrome);

                element.MinHeight = 768;
                element.MinWidth = 1024;
                element.Height = 768;
                element.Width = 1024;
                element.BorderThickness = new Thickness(0);
                element.SizeToContent = SizeToContent.WidthAndHeight;
                element.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                element.VerticalContentAlignment = VerticalAlignment.Stretch;
                element.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                element.SetValue(Window.BorderThicknessProperty, new Thickness(0));
                element.SetValue(Window.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
            }
            else
            {
                WindowChrome.SetWindowChrome(element, null);
            }
        }
    }
}