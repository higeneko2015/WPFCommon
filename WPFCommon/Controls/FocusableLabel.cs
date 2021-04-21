using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCommon
{
    public class FocusableLabel : Label
    {
        public static readonly DependencyProperty FocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(FocusedBackgroundColor), typeof(Brush), typeof(FocusableLabel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty FocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(FocusedForegroundColor), typeof(Brush), typeof(FocusableLabel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(MouseOverBackgroundColor), typeof(Brush), typeof(FocusableLabel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(MouseOverForegroundColor), typeof(Brush), typeof(FocusableLabel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static FocusableLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FocusableLabel), new FrameworkPropertyMetadata(typeof(FocusableLabel)));
        }

        public Brush FocusedBackgroundColor
        {
            get { return (Brush)this.GetValue(FocusedBackgroundColorProperty); }
            set { this.SetValue(FocusedBackgroundColorProperty, value); }
        }

        public Brush FocusedForegroundColor
        {
            get { return (Brush)this.GetValue(FocusedForegroundColorProperty); }
            set { this.SetValue(FocusedForegroundColorProperty, value); }
        }

        public Brush MouseOverBackgroundColor
        {
            get { return (Brush)this.GetValue(MouseOverBackgroundColorProperty); }
            set { this.SetValue(MouseOverBackgroundColorProperty, value); }
        }

        public Brush MouseOverForegroundColor
        {
            get { return (Brush)this.GetValue(MouseOverForegroundColorProperty); }
            set { this.SetValue(MouseOverForegroundColorProperty, value); }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            this.Focus();
        }
    }
}
