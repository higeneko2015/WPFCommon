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

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(FocusableLabel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
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
            TimeSelecterClieckGroup group = default;

            switch (this.Name.Substring(0, 1))
            {
                case "h":
                    group = TimeSelecterClieckGroup.Hours;
                    break;

                case "m":
                    group = TimeSelecterClieckGroup.Minutes;
                    break;

                case "s":
                    group = TimeSelecterClieckGroup.Seconds;
                    break;
            }
            var number = int.Parse(this.Name[1..]);

            var args = new TimeSelecterClickEventArgs(EventHelper.TextBlockClickEvent, group, number);
            this.RaiseEvent(args);

            base.OnPreviewMouseLeftButtonDown(e);
        }
    }
}
