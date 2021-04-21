using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCommon
{
    public class TimeSelecter : Control
    {
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
            DependencyProperty.Register(nameof(SelectedHour), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectedMinutesProperty =
            DependencyProperty.Register(nameof(SelectedMinutes), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectedSecondsProperty =
            DependencyProperty.Register(nameof(SelectedSeconds), typeof(int?), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register(nameof(SelectedTime), typeof(Time), typeof(TimeSelecter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static TimeSelecter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSelecter), new FrameworkPropertyMetadata(typeof(TimeSelecter)));
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

        public Time SelectedTime
        {
            get { return (Time)this.GetValue(SelectedTimeProperty); }
            set { this.SetValue(SelectedTimeProperty, value); }

            //get
            //{
            //    var hours = this.SelectedHour;
            //    var minutes = this.SelectedMinutes;
            //    var seconds = this.SelectedSeconds;
            //    if (hours.HasValue && minutes.HasValue && seconds.HasValue)
            //    {
            //        return new Time(hours.Value, minutes.Value, seconds.Value);
            //    }
            //    if (hours.HasValue && minutes.HasValue)
            //    {
            //        return new Time(hours.Value, minutes.Value, 0);
            //    }
            //    if (minutes.HasValue && seconds.HasValue)
            //    {
            //        return new Time(0, minutes.Value, seconds.Value);
            //    }
            //    if (hours.HasValue && seconds.HasValue)
            //    {
            //        return new Time(hours.Value, 0, seconds.Value);
            //    }
            //    if (hours.HasValue)
            //    {
            //        return new Time(hours.Value, 0, 0);
            //    }
            //    if (minutes.HasValue)
            //    {
            //        return new Time(0, minutes.Value, 0);
            //    }
            //    if (seconds.HasValue)
            //    {
            //        return new Time(0, 0, seconds.Value);
            //    }
            //    return null;
            //}
            //set
            //{
            //    if (value.IsEmpty())
            //    {
            //        this.SelectedHour = null;
            //        this.SelectedMinutes = null;
            //        this.SelectedSeconds = null;
            //    }
            //    else
            //    {
            //        this.SelectedHour = value.Hour;
            //        this.SelectedMinutes = value.Minute;
            //        this.SelectedSeconds = value.Second;
            //    }
            //}
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var hourGrid = GetTemplateChild("HourGrid") as Grid;
            var hh = FocusManager.GetFocusedElement(hourGrid) as FocusableLabel;
            if (hh.IsEmpty())
            {
                SelectedHour = null;
            }
            else
            {
                SelectedHour = int.Parse(hh.Content.ToString());
            }

            var minutesGrid = GetTemplateChild("MinutesGrid") as Grid;
            var mm = FocusManager.GetFocusedElement(minutesGrid) as FocusableLabel;
            if (mm.IsEmpty())
            {
                SelectedMinutes = null;
            }
            else
            {
                SelectedMinutes = int.Parse(mm.Content.ToString());
            }

            var secondsGrid = GetTemplateChild("SecondsGrid") as Grid;
            var ss = FocusManager.GetFocusedElement(secondsGrid) as FocusableLabel;
            if (ss.IsEmpty())
            {
                SelectedSeconds = null;
            }
            else
            {
                SelectedSeconds = int.Parse(ss.Content.ToString());
            }

            UpdateSelectedTime();
            base.OnMouseLeftButtonDown(e);
        }

        private void UpdateSelectedTime()
        {
            var hours = this.SelectedHour;
            var minutes = this.SelectedMinutes;
            var seconds = this.SelectedSeconds;
            if (hours.HasValue && minutes.HasValue && seconds.HasValue)
            {
                this.SelectedTime = new Time(hours.Value, minutes.Value, seconds.Value);
                return;
            }
            if (hours.HasValue && minutes.HasValue)
            {
                this.SelectedTime = new Time(hours.Value, minutes.Value, 0);
                return;
            }
            if (minutes.HasValue && seconds.HasValue)
            {
                this.SelectedTime = new Time(0, minutes.Value, seconds.Value);
                return;
            }
            if (hours.HasValue && seconds.HasValue)
            {
                this.SelectedTime = new Time(hours.Value, 0, seconds.Value);
                return;
            }
            if (hours.HasValue)
            {
                this.SelectedTime = new Time(hours.Value, 0, 0);
                return;
            }
            if (minutes.HasValue)
            {
                this.SelectedTime = new Time(0, minutes.Value, 0);
                return;
            }
            if (seconds.HasValue)
            {
                this.SelectedTime = new Time(0, 0, seconds.Value);
                return;
            }
            this.SelectedTime = null;
        }
    }
}
