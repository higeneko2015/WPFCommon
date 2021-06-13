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

        public static readonly DependencyProperty HoursDefaultBackgroundColorProperty =
            DependencyProperty.Register(nameof(HoursDefaultBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HoursDefaultForegroundColorProperty =
            DependencyProperty.Register(nameof(HoursDefaultForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HoursFocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(HoursFocusedBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HoursFocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(HoursFocusedForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HoursMouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(HoursMouseOverBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HoursMouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(HoursMouseOverForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public static readonly DependencyProperty ItemBackgroundColorProperty =
        //    DependencyProperty.Register(nameof(ItemBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        //public static readonly DependencyProperty ItemForegroundColorProperty =
        //    DependencyProperty.Register(nameof(ItemForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesDefaultBackgroundColorProperty =
            DependencyProperty.Register(nameof(MinutesDefaultBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesDefaultForegroundColorProperty =
            DependencyProperty.Register(nameof(MinutesDefaultForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesFocusedBackgroundColorProperty =
            DependencyProperty.Register(nameof(MinutesFocusedBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesFocusedForegroundColorProperty =
            DependencyProperty.Register(nameof(MinutesFocusedForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesMouseOverBackgroundColorProperty =
            DependencyProperty.Register(nameof(MinutesMouseOverBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinutesMouseOverForegroundColorProperty =
            DependencyProperty.Register(nameof(MinutesMouseOverForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsDefaultBackgroundColorProperty =
            DependencyProperty.Register(nameof(SecondsDefaultBackgroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SecondsDefaultForegroundColorProperty =
            DependencyProperty.Register(nameof(SecondsDefaultForegroundColor), typeof(Brush), typeof(TimeSelecter), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        private readonly TextBlockClickEventHandler handler = null;

        static TimeSelecter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelecter), new FrameworkPropertyMetadata(typeof(TimeSelecter)));
        }

        public TimeSelecter()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            this.handler = this.TextBlockClickEventHandler;

            // コンストラクタではLoadedハンドラだけ登録する
            this.Loaded += this.TimeSelecter_Loaded;
        }

        public TimeSelecterDisplayMode DisplayMode
        {
            get { return (TimeSelecterDisplayMode)this.GetValue(DisplayModeProperty); }
            set { this.SetValue(DisplayModeProperty, value); }
        }

        public Brush HoursDefaultBackgroundColor
        {
            get { return (Brush)this.GetValue(HoursDefaultBackgroundColorProperty); }
            set { this.SetValue(HoursDefaultBackgroundColorProperty, value); }
        }

        public Brush HoursDefaultForegroundColor
        {
            get { return (Brush)this.GetValue(HoursDefaultForegroundColorProperty); }
            set { this.SetValue(HoursDefaultForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロールで選択中のセルの背景色を取得または設定します。")]
        public Brush HoursFocusedBackgroundColor
        {
            get { return (Brush)this.GetValue(HoursFocusedBackgroundColorProperty); }
            set { this.SetValue(HoursFocusedBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロールで選択中のセルの文字色を取得または設定します。")]
        public Brush HoursFocusedForegroundColor
        {
            get { return (Brush)this.GetValue(HoursFocusedForegroundColorProperty); }
            set { this.SetValue(HoursFocusedForegroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロール上にマウスカーソルがあるセルの背景色を取得または設定します。")]
        public Brush HoursMouseOverBackgroundColor
        {
            get { return (Brush)this.GetValue(HoursMouseOverBackgroundColorProperty); }
            set { this.SetValue(HoursMouseOverBackgroundColorProperty, value); }
        }

        [Category("拡張プロパティ"), Description("時コントロール上にマウスカーソルがあるセルの文字色を取得または設定します。")]
        public Brush HoursMouseOverForegroundColor
        {
            get { return (Brush)this.GetValue(HoursMouseOverForegroundColorProperty); }
            set { this.SetValue(HoursMouseOverForegroundColorProperty, value); }
        }

        //[Category("拡張プロパティ"), Description("選択欄の背景色を取得または設定します。")]
        //public Brush ItemBackgroundColor
        //{
        //    get { return (Brush)this.GetValue(ItemBackgroundColorProperty); }
        //    set { this.SetValue(ItemBackgroundColorProperty, value); }
        //}

        //[Category("拡張プロパティ"), Description("選択欄の文字色を取得または設定します。")]
        //public Brush ItemForegroundColor
        //{
        //    get { return (Brush)this.GetValue(ItemForegroundColorProperty); }
        //    set { this.SetValue(ItemForegroundColorProperty, value); }
        //}

        public Brush MinutesDefaultBackgroundColor
        {
            get { return (Brush)this.GetValue(MinutesDefaultBackgroundColorProperty); }
            set { this.SetValue(MinutesDefaultBackgroundColorProperty, value); }
        }

        public Brush MinutesDefaultForegroundColor
        {
            get { return (Brush)this.GetValue(MinutesDefaultForegroundColorProperty); }
            set { this.SetValue(MinutesDefaultForegroundColorProperty, value); }
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

        public Brush SecondsDefaultBackgroundColor
        {
            get { return (Brush)this.GetValue(SecondsDefaultBackgroundColorProperty); }
            set { this.SetValue(SecondsDefaultBackgroundColorProperty, value); }
        }

        public Brush SecondsDefaultForegroundColor
        {
            get { return (Brush)this.GetValue(SecondsDefaultForegroundColorProperty); }
            set { this.SetValue(SecondsDefaultForegroundColorProperty, value); }
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

        private static void SelectedHoursChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectedTimesChanged(sender, e, "h");
        }

        private static void SelectedMinutesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectedTimesChanged(sender, e, "m");
        }

        private static void SelectedSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelectedTimesChanged(sender, e, "s");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        private static void SelectedTimesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e, string procKbn)
        {
            var target = sender as TimeSelecter;
            var beforeValue = e.OldValue as int?;
            var afterValue = e.NewValue as int?;

            target.UpdateSelectedStatus(procKbn, beforeValue, afterValue);

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

        private void TimeSelecter_Closed(object sender, System.EventArgs e)
        {
            // コンストラクタで登録したハンドラとLoadedで登録したハンドラを解放する
            this.Loaded -= this.TimeSelecter_Loaded;
            this.TimeSelecter_Unloaded(sender, new RoutedEventArgs());
        }

        private void TimeSelecter_Loaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded += this.TimeSelecter_Unloaded;
            // コンストラクタの時点ではWindow.GetWindow(this)はnullになるので
            // Loadedイベントで登録する。
            // Tabの切り替えでもUnload→Loadが発生するため
            // Loadedで登録した情報はUnLoadedイベントで解放させる
            Window.GetWindow(this).Closed += this.TimeSelecter_Closed;
            this.AddHandler(EventHelper.TextBlockClickEvent, this.handler);
        }

        private void TimeSelecter_Unloaded(object sender, RoutedEventArgs e)
        {
            // Loadedイベントで登録したハンドラはUnLoadedで解放する
            this.Unloaded -= this.TimeSelecter_Unloaded;
            Window.GetWindow(this).Closed -= this.TimeSelecter_Closed;
            this.RemoveHandler(EventHelper.TextBlockClickEvent, this.handler);
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
