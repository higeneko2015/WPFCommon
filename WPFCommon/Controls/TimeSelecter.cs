using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFCommon
{
    public enum TimeSelecterDisplayMode
    {
        HMS,
        HM,
        H,
    }

    public class TimeSelecter : Control
    {
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode), typeof(TimeSelecterDisplayMode), typeof(TimeSelecter), new FrameworkPropertyMetadata(TimeSelecterDisplayMode.HMS, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourFocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(HourFocusedBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourFocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(HourFocusedForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourMouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(HourMouseOverBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourMouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(HourMouseOverForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesFocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(MinutesFocusedBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesFocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(MinutesFocusedForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesMouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(MinutesMouseOverBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesMouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(MinutesMouseOverForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsFocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(SecondsFocusedBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsFocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(SecondsFocusedForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsMouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(SecondsMouseOverBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsMouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(SecondsMouseOverForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectedHourProperty =
            DependencyProperty.Register(nameof(SelectedHour), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedHoursChanged)));

        public static readonly DependencyProperty SelectedMinutesProperty =
            DependencyProperty.Register(nameof(SelectedMinutes), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedMinutesChanged)));

        public static readonly DependencyProperty SelectedSecondsProperty =
            DependencyProperty.Register(nameof(SelectedSeconds), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedSecondsChanged)));

        //public static readonly DependencyProperty SelectedTimeProperty =
        //    DependencyProperty.Register(nameof(SelectedTime), typeof(Time), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private readonly TextBlockClickEventHandler handler = null;

        static TimeSelecter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelecter), new FrameworkPropertyMetadata(typeof(TimeSelecter)));
        }

        public TimeSelecter()
        {
            // 各ボタンから発火するTextBlockClickEventイベントを購読する。
            this.handler = this.TextBlockClickEventHandler;
            this.AddHandler(EventHelper.TextBlockClickEvent, this.handler);
        }

        public TimeSelecterDisplayMode DisplayMode
        {
            get { return (TimeSelecterDisplayMode)this.GetValue(DisplayModeProperty); }
            set { this.SetValue(DisplayModeProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロールで選択中のセルの背景色を取得または設定します。")]
        public Brush HourFocusedBackgroundColor
        {
            get { return (Brush)this.GetValue(HourFocusedBackgroundColorProperty); }
            set { this.SetValue(HourFocusedBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロールで選択中のセルの文字色を取得または設定します。")]
        public Brush HourFocusedForegroundColor
        {
            get { return (Brush)this.GetValue(HourFocusedForegroundColorProperty); }
            set { this.SetValue(HourFocusedForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロール上にマウスカーソルがあるセルの背景色を取得または設定します。")]
        public Brush HourMouseOverBackgroundColor
        {
            get { return (Brush)this.GetValue(HourMouseOverBackgroundColorProperty); }
            set { this.SetValue(HourMouseOverBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロール上にマウスカーソルがあるセルの文字色を取得または設定します。")]
        public Brush HourMouseOverForegroundColor
        {
            get { return (Brush)this.GetValue(HourMouseOverForegroundColorProperty); }
            set { this.SetValue(HourMouseOverForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("分コントロールで選択中のセルの背景色を取得または設定します。")]
        public Brush MinutesFocusedBackgroundColor
        {
            get { return (Brush)this.GetValue(MinutesFocusedBackgroundColorProperty); }
            set { this.SetValue(MinutesFocusedBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("分コントロールで選択中のセルの文字色を取得または設定します。")]
        public Brush MinutesFocusedForegroundColor
        {
            get { return (Brush)this.GetValue(MinutesFocusedForegroundColorProperty); }
            set { this.SetValue(MinutesFocusedForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("分コントロール上にマウスカーソルがあるセルの背景色を取得または設定します。")]
        public Brush MinutesMouseOverBackgroundColor
        {
            get { return (Brush)this.GetValue(MinutesMouseOverBackgroundColorProperty); }
            set { this.SetValue(MinutesMouseOverBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("分コントロール上にマウスカーソルがあるセルの文字色を取得または設定します。")]
        public Brush MinutesMouseOverForegroundColor
        {
            get { return (Brush)this.GetValue(MinutesMouseOverForegroundColorProperty); }
            set { this.SetValue(MinutesMouseOverForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("秒コントロールで選択中のセルの背景色を取得または設定します。")]
        public Brush SecondsFocusedBackgroundColor
        {
            get { return (Brush)this.GetValue(SecondsFocusedBackgroundColorProperty); }
            set { this.SetValue(SecondsFocusedBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("秒コントロールで選択中のセルの文字色を取得または設定します。")]
        public Brush SecondsFocusedForegroundColor
        {
            get { return (Brush)this.GetValue(SecondsFocusedForegroundColorProperty); }
            set { this.SetValue(SecondsFocusedForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("秒コントロール上にマウスカーソルがあるセルの背景色を取得または設定します。")]
        public Brush SecondsMouseOverBackgroundColor
        {
            get { return (Brush)this.GetValue(SecondsMouseOverBackgroundColorProperty); }
            set { this.SetValue(SecondsMouseOverBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("秒コントロール上にマウスカーソルがあるセルの文字色を取得または設定します。")]
        public Brush SecondsMouseOverForegroundColor
        {
            get { return (Brush)this.GetValue(SecondsMouseOverForegroundColorProperty); }
            set { this.SetValue(SecondsMouseOverForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("現在選択されている「時」を取得します。")]
        public int? SelectedHour
        {
            get { return (int?)this.GetValue(SelectedHourProperty); }
            set { this.SetValue(SelectedHourProperty, value); }
        }

        [Category("拡張プロパティ"), Description("現在選択されている「分」を取得します。")]
        public int? SelectedMinutes
        {
            get { return (int?)this.GetValue(SelectedMinutesProperty); }
            set { this.SetValue(SelectedMinutesProperty, value); }
        }

        [Category("拡張プロパティ"), Description("現在選択されている「秒」を取得します。")]
        public int? SelectedSeconds
        {
            get { return (int?)this.GetValue(SelectedSecondsProperty); }
            set { this.SetValue(SelectedSecondsProperty, value); }
        }

        //public Time SelectedTime
        //{
        //    get { return (Time)this.GetValue(SelectedTimeProperty); }
        //    set { this.SetValue(SelectedTimeProperty, value); }
        //}

        private static void SelectedHoursChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = sender as TimeSelecter;
            var beforeValue = e.OldValue as int?;
            var afterValue = e.NewValue as int?;

            target.UpdateSelectedStatus("h", beforeValue, afterValue);

            var args = new RoutedEventArgs(EventHelper.TimeSelecterChangeEvent);
            target.RaiseEvent(args);
        }

        private static void SelectedMinutesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = sender as TimeSelecter;
            var beforeValue = e.OldValue as int?;
            var afterValue = e.NewValue as int?;

            target.UpdateSelectedStatus("m", beforeValue, afterValue);

            var args = new RoutedEventArgs(EventHelper.TimeSelecterChangeEvent);
            target.RaiseEvent(args);
        }

        private static void SelectedSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = sender as TimeSelecter;
            var beforeValue = e.OldValue as int?;
            var afterValue = e.NewValue as int?;

            target.UpdateSelectedStatus("s", beforeValue, afterValue);

            var args = new RoutedEventArgs(EventHelper.TimeSelecterChangeEvent);
            target.RaiseEvent(args);
        }

        private void TextBlockClickEventHandler(object sender, TimeSelecterClickEventArgs e)
        {
            switch (e.ClickGroup)
            {
                case TimeSelecterClickGroup.Hours:
                    this.UpdateSelectedStatus("h", this.SelectedHour, e.SelectedNumber);
                    this.SelectedHour = e.SelectedNumber;
                    break;

                case TimeSelecterClickGroup.Minutes:
                    this.UpdateSelectedStatus("m", this.SelectedMinutes, e.SelectedNumber);
                    this.SelectedMinutes = e.SelectedNumber;
                    break;

                case TimeSelecterClickGroup.Seconds:
                    this.UpdateSelectedStatus("s", this.SelectedSeconds, e.SelectedNumber);
                    this.SelectedSeconds = e.SelectedNumber;
                    break;
            }
            // これ以上イベントを伝播させないようにフラグを立てる。
            e.Handled = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateSelectedStatus(string groupSyntax, int? beforeValue, int? afterValue)
        {
            // 変更前の値が設定されている場合は未選択に戻す
            if (beforeValue.HasValue)
            {
                var beforeControl = this.GetTemplateChild($"{groupSyntax}{beforeValue.Value.ToString("00")}") as FocusableLabel;
                if (beforeControl.IsEmpty() == false)
                {
                    beforeControl.IsSelected = false;
                }
            }

            // 変更後の値が設定されている場合は選択状態にする
            if (afterValue.HasValue)
            {
                var afterControl = this.GetTemplateChild($"{groupSyntax}{afterValue.Value.ToString("00")}") as FocusableLabel;
                if (afterControl.IsEmpty() == false)
                {
                    afterControl.IsSelected = true;
                }
            }
        }
    }
}
