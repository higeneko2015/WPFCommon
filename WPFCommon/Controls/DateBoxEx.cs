using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Calendar = System.Windows.Controls.Calendar;

namespace WPFCommon
{
    public enum EnumDateDisplayMode
    {
        YYYYMMDD,
        YYYYMM,
        MMDD,
    }

    public class DateBoxEx : ContentControl
    {
        public static readonly DependencyProperty BlackoutDatesProperty =
            DependencyProperty.Register(nameof(BlackoutDates), typeof(CalendarBlackoutDatesCollection), typeof(DateBoxEx));

        public static readonly DependencyProperty CalendarButtonStyleProperty =
            DependencyProperty.Register(nameof(CalendarButtonStyle), typeof(Style), typeof(DateBoxEx));

        public static readonly DependencyProperty CalendarDayButtonStyleProperty =
            DependencyProperty.Register(nameof(CalendarDayButtonStyle), typeof(Style), typeof(DateBoxEx));

        public static readonly DependencyProperty CalendarItemStyleProperty =
            DependencyProperty.Register(nameof(CalendarItemStyle), typeof(Style), typeof(DateBoxEx));

        public static readonly DependencyProperty DateDisplayModeProperty =
            DependencyProperty.Register(nameof(DateDisplayMode), typeof(EnumDateDisplayMode), typeof(DateBoxEx), new PropertyMetadata(EnumDateDisplayMode.YYYYMMDD));

        public static readonly DependencyProperty DisplayDateEndProperty =
            DependencyProperty.Register(nameof(DisplayDateEnd), typeof(DateTime?), typeof(DateBoxEx));

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register(nameof(DisplayDate), typeof(DateTime), typeof(DateBoxEx), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register(nameof(DisplayDateStart), typeof(DateTime?), typeof(DateBoxEx));

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode), typeof(CalendarMode), typeof(DateBoxEx), new PropertyMetadata(CalendarMode.Decade));

        public static readonly DependencyProperty FirstDayOfWeekProperty =
            DependencyProperty.Register(nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(DateBoxEx), new PropertyMetadata(DayOfWeek.Sunday));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(DateBoxEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsReadOnlyChanged)));

        public static readonly DependencyProperty IsTodayHighlightedProperty =
            DependencyProperty.Register(nameof(IsTodayHighlighted), typeof(bool), typeof(DateBoxEx), new PropertyMetadata(true));

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(DateBoxEx));

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(DateBoxEx));

        public static readonly DependencyProperty SelectionTextBrushProperty =
            DependencyProperty.Register(nameof(SelectionTextBrush), typeof(Brush), typeof(DateBoxEx));

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(DateBoxEx), new PropertyMetadata("yyyy/MM/dd"));

        // BindsTwoWayByDefaultを指定しておかないとthis.Text = hogehoge;という操作を行ったときにBindingがクリアされてしまう。
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(DateBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty WaterMarkStringProperty =
            DependencyProperty.Register(nameof(WaterMarkString), typeof(string), typeof(DateBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private ToggleButton _PartsButton = null;
        private Calendar _PartsCalender = null;
        private Popup _PartsPopup = null;
        private TextBox _PartsTextBox = null;
        private string BeforeText = string.Empty;
        private bool CheckedFlg = false;

        static DateBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateBoxEx), new FrameworkPropertyMetadata(typeof(DateBoxEx)));
        }

        public DateBoxEx()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // イベントハンドラの追加
            //this.Loaded += this.TextBoxBase_Loaded;
            this.Unloaded += this.DateBoxEx_Unloaded;
            Validation.AddErrorHandler(this, DateBoxEx_ValidationError);

            // 貼り付けコマンドのハンドラを追加
            //DataObject.AddPastingHandler(this, this.TextBoxBase_PastingHandler);
        }

        [Category("カレンダー"), Description("カレンダーコントロールで選択不可とする日付のコレクションを取得または設定します。")]
        public CalendarBlackoutDatesCollection BlackoutDates
        {
            get { return (CalendarBlackoutDatesCollection)this.GetValue(BlackoutDatesProperty); }
            set { this.SetValue(BlackoutDatesProperty, value); }
        }

        [Category("カレンダー"), Description("年月選択形式のカレンダーコントロール内の各月ボタンのスタイルを取得または設定します。")]
        public Style CalendarButtonStyle
        {
            get { return (Style)this.GetValue(CalendarButtonStyleProperty); }
            set { this.SetValue(CalendarButtonStyleProperty, value); }
        }

        [Category("カレンダー"), Description("年月日選択形式のカレンダーコントロール内の各日付ボタンのスタイルを取得または設定します。")]
        public Style CalendarDayButtonStyle
        {
            get { return (Style)this.GetValue(CalendarDayButtonStyleProperty); }
            set { this.SetValue(CalendarDayButtonStyleProperty, value); }
        }

        [Category("カレンダー"), Description("カレンダーコントロール自体のスタイルを取得または設定します。")]
        public Style CalendarItemStyle
        {
            get { return (Style)this.GetValue(CalendarItemStyleProperty); }
            set { this.SetValue(CalendarItemStyleProperty, value); }
        }

        [Category("カレンダー"), Description("日付テキストボックスの表示書式を取得または設定します。")]
        public EnumDateDisplayMode DateDisplayMode
        {
            get { return (EnumDateDisplayMode)this.GetValue(DateDisplayModeProperty); }
            set { this.SetValue(DateDisplayModeProperty, value); }
        }

        [Category("カレンダー")]
        public DateTime DisplayDate
        {
            get { return (DateTime)this.GetValue(DisplayDateProperty); }
            set { this.SetValue(DisplayDateProperty, value); }
        }

        [Category("カレンダー"), Description("カレンダーコントロールで選択可能な最大の日付を取得または設定します。")]
        public DateTime? DisplayDateEnd
        {
            get { return (DateTime?)this.GetValue(DisplayDateEndProperty); }
            set { this.SetValue(DisplayDateEndProperty, value); }
        }

        [Category("カレンダー"), Description("カレンダーコントロールで選択可能な最小の日付を取得または設定します。")]
        public DateTime? DisplayDateStart
        {
            get { return (DateTime?)this.GetValue(DisplayDateStartProperty); }
            set { this.SetValue(DisplayDateStartProperty, value); }
        }

        [Category("カレンダー"), Description("カレンダーコントロールの表示モードを取得または設定します。")]
        public CalendarMode DisplayMode
        {
            get { return (CalendarMode)this.GetValue(DisplayModeProperty); }
            set { this.SetValue(DisplayModeProperty, value); }
        }

        [Category("カレンダー"), Description("年月日選択形式のカレンダーコントロールで週の初めの曜日を取得または設定します。")]
        public DayOfWeek FirstDayOfWeek
        {
            get { return (DayOfWeek)this.GetValue(FirstDayOfWeekProperty); }
            set { this.SetValue(FirstDayOfWeekProperty, value); }
        }

        [Category("Text"), Description("コントロールが編集不可モードになっているかどうかを取得または設定します。")]
        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        [Category("カレンダー"), Description("カレンダーコントロールで今日の日付の背景色をハイライト表示するかどうかを取得または設定します。")]
        public bool IsTodayHighlighted
        {
            get { return (bool)this.GetValue(IsTodayHighlightedProperty); }
            set { this.SetValue(IsTodayHighlightedProperty, value); }
        }

        [Category("Text"), Description("カレンダーコントロールで選択した日付を取得または設定します。")]
        public DateTime? SelectedDate
        {
            get { return (DateTime?)this.GetValue(SelectedDateProperty); }
            set { this.SetValue(SelectedDateProperty, value); }
        }

        [Category("Text"), Description("日付テキストボックスで選択している文字列を取得または設定します。")]
        public string SelectedText
        {
            get { return this.PartsTextBox.SelectedText; }
            set { this.PartsTextBox.SelectedText = value; }
        }

        [Category("Text"), Description("日付テキストボックスで選択している文字列の背景色を取得または設定します。")]
        public Brush SelectionBrush
        {
            get { return (Brush)this.GetValue(SelectionBrushProperty); }
            set { this.SetValue(SelectionBrushProperty, value); }
        }

        [Category("Text"), Description("日付テキストボックスで選択している文字列の文字色を取得または設定します。")]
        public Brush SelectionTextBrush
        {
            get { return (Brush)this.GetValue(SelectionTextBrushProperty); }
            set { this.SetValue(SelectionTextBrushProperty, value); }
        }

        [Category("Text"), Description("日付テキストボックスの表示書式を取得または設定します。")]
        public string StringFormat
        {
            get { return (string)this.GetValue(StringFormatProperty); }
            set { this.SetValue(StringFormatProperty, value); }
        }

        [Category("Text"), Description("日付テキストボックスに入力されている値を取得または設定します。")]
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        [Category("Text"), Description("日付テキストボックスになにも入力されていない時に表示するすかし文字を取得または設定します。")]
        public string WaterMarkString
        {
            get { return (string)this.GetValue(WaterMarkStringProperty); }
            set { this.SetValue(WaterMarkStringProperty, value); }
        }

        /// <summary>
        /// 日付テキストボックス内のカレンダーコントロールを表示するためのボタンコントロールへの参照
        /// </summary>
        private ToggleButton PartsButton
        {
            get
            {
                if (this._PartsButton.IsEmpty())
                {
                    this._PartsButton = GetTemplateChild("PART_Button1") as ToggleButton;
                }
                return this._PartsButton;
            }
        }

        /// <summary>
        /// 日付テキストボックスから表示するカレンダーコントロールへの参照
        /// </summary>
        private Calendar PartsCalender
        {
            get
            {
                if (this._PartsCalender.IsEmpty())
                {
                    this._PartsCalender = GetTemplateChild("PART_Calender") as Calendar;
                }
                return this._PartsCalender;
            }
        }

        /// <summary>
        /// 日付テキストボックスから表示するカレンダーコントロールを内包しているポップアップへの参照
        /// </summary>
        private Popup PartsPopup
        {
            get
            {
                if (this._PartsPopup.IsEmpty())
                {
                    this._PartsPopup = GetTemplateChild("PART_Popup1") as Popup;
                }
                return this._PartsPopup;
            }
        }

        /// <summary>
        /// 日付テキストボックス内のテキスト入力コントロールへの参照
        /// </summary>
        private TextBox PartsTextBox
        {
            get
            {
                if (this._PartsTextBox.IsEmpty())
                {
                    this._PartsTextBox = GetTemplateChild("PART_TextBox1") as TextBox;
                }
                return this._PartsTextBox;
            }
        }

        /// <summary>
        /// コントロールがLoadされてテンプレートが適用されたときに呼び出されるハンドラ
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // 以下の２つのイベントはボタン押下でカレンダー開く→ボタン押下としたときに
            // MouseDownでカレンダーが閉じられ、MouseUpで再度カレンダーが開いてしまう問題に対応
            this.PartsPopup.Opened += this.Popup_Opened;
            this.PartsPopup.Closed += this.Popup_Closed;

            // カレンダーの日付のダブルクリックで選択・カレンダーを閉じる操作を行うためにハンドラを登録
            var ee = new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(CalenderDayButton_MouseDoubleClick));
            var style = this.PartsCalender.CalendarDayButtonStyle;
            if (style == null)
            {
                style = new Style() { TargetType = typeof(CalendarDayButton) };
            }
            style.Setters.Add(ee);
            this.PartsCalender.CalendarDayButtonStyle = style;

            // 日付テキストボックスのIsReadOnlyとカレンダーを起動するためのボタンのIsEnabledを連動させる
            this.PartsButton.IsEnabled = !this.IsReadOnly;

            // TextプロパティへのBindingが定義されている場合は、デフォルトの表示書式などを定義する
            // UpdateSourceTriggerやNotifyOnValidationErrorなど、基盤側で必要な定義も設定する
            var expression = BindingOperations.GetBindingExpression(this, TextProperty);
            if (expression.IsEmpty())
            {
                return;
            }

            var binding = BindingOperations.GetBinding(this, TextProperty);
            if (binding.IsEmpty())
            {
                // bindingが定義されていない場合は処理しない
                return;
            }

            var newBinding = new Binding(binding.Path.Path)
            {
                Mode = binding.Mode,
                NotifyOnValidationError = true,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                ConverterCulture = CultureInfo.CurrentCulture,
            };

            var displayFormat = string.Empty;
            var x = this.DataContext?.GetType().GetProperty(expression.ParentBinding?.Path.Path);
            //switch (x.PropertyType)
            //{
            //    case Type intType when intType == typeof(Date):
            //        displayFormat = expression.ParentBinding.StringFormat;
            //        if (displayFormat.IsEmpty())
            //        {
            //            displayFormat = "yyyy/MM/dd";
            //        }
            //        newBinding.Converter = new StringToDateConverter();
            //        newBinding.ValidationRules.Add(new DateValidationRule());
            //        break;

            //    default:
            //        break;
            //}
            if (x.PropertyType == typeof(Date))
            {
                displayFormat = expression.ParentBinding.StringFormat;
                if (displayFormat.IsEmpty())
                {
                    displayFormat = "yyyy/MM/dd";
                }
                newBinding.Converter = new StringToDateConverter();
                newBinding.ValidationRules.Add(new DateValidationRule());
            }
            newBinding.ConverterParameter = displayFormat;
            newBinding.StringFormat = displayFormat;
            BindingOperations.ClearBinding(this, TextProperty);
            BindingOperations.SetBinding(this, TextProperty, newBinding);
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.Dispatcher.InvokeAsync(this.GotKeyboardFocusInvoke);

            base.OnGotKeyboardFocus(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            this.PreviewMouseUp += this.DateBoxEx_PreviewMouseUp;
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.IsReadOnly)
            {
                //e.Handled = true; // これがあるとフォーカスが外れなくなる
                return;
            }

            // カレンダーコントールの各日付はCalendarDayButtonコントロールで実装されており
            // 日付を選択した瞬間にDateBoxExからボタンにフォーカス移動するのでKeyboardFocusが外れることになる。
            // そのときのイベントを無視するためにチェックを追加
            if (e.NewFocus is CalendarDayButton)
            {
                e.Handled = true;
                return;
            }

            this.CheckedFlg = false;

            // 画面を閉じるボタンを押下した場合は入力値のチェックしない
            // TODO Button03という名称は変更した方がよい
            var newFocusControl = e.NewFocus as Button;
            if (newFocusControl?.Name == "Button03")
            {
                e.Handled = true;
                return;
            }

            // 入力値に変更が内場合は処理しない
            if (this.Text == this.BeforeText)
            {
                return;
            }

            if (this.IsKeyboardFocusWithin == false)
            {
                base.OnPreviewLostKeyboardFocus(e);
                return;
            }

            this.CheckedFlg = true;

            // Validationを実施
            var expression = BindingOperations.GetBindingExpression(this, TextProperty);
            expression?.UpdateSource();
            if (expression?.ValidationErrors?.Count > 0)
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewLostKeyboardFocus(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (this.IsReadOnly)
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewTextInput(e);

            var ret = this.CheckInputText(this, e.Text);
            if (ret == false)
            {
                e.Handled = true;
            }
        }

        private static void OnIsReadOnlyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var IsReadOnly = (bool)e.NewValue;

            if (obj is DateBoxEx ctrl)
            {
                if (ctrl.PartsButton.IsEmpty() == false)
                {
                    ctrl.PartsButton.IsEnabled = !IsReadOnly;
                }
            }
        }

        private void CalenderDayButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var button = sender as CalendarDayButton;
            this.SelectedDate = (DateTime)button.DataContext;

            if (!this.PartsCalender.BlackoutDates.Contains(this.SelectedDate.Value))
            {
                this.PartsPopup.IsOpen = false;
                this.Text = this.SelectedDate?.ToString(StringFormat);
            }
        }

        private void ChangeHitTestVisible(bool value)
        {
            var btn = GetTemplateChild("PART_Button1") as ToggleButton;
            btn.IsHitTestVisible = value;
            // コントロール外にフォーカスがある状態から直接ボタンをクリックしたときに強制的にフォーカスを当てる
            this.Dispatcher.InvokeAsync(() => { this.PartsTextBox.Focus(); this.GotKeyboardFocusInvoke(); });
        }

        /// <summary>
        /// キー押下時に入力可能な文字種かどうか判定を行うメソッド
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="inputText">入力文字</param>
        /// <returns>入力可能(True)、入力不可(False)</returns>
        private bool CheckInputText(DateBoxEx sender, string inputText)
        {
            if (".0123456789".Contains(inputText) == false)
            {
                return false;
            }

            //var target = sender as DateBoxEx;

            // テキストが全選択されている場合は今回の入力は１文字目として
            // 扱うためにフラグを立てる
            var allSelectedFlg = false;

            if (sender.SelectedText == sender.Text)
            {
                allSelectedFlg = true;
            }

            if (inputText == ".")
            {
                if (allSelectedFlg)
                {
                    return true;
                }
                if (sender.Text.Length == 0)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        private void DateBoxEx_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.InvokeAsync(this.GotKeyboardFocusInvoke);
            this.ReleaseMouseCapture();
            // この記述があると２回目にマウスクリックしたときに画面が固まってしまう
            //this.CaptureMouse();
            this.PreviewMouseUp -= this.DateBoxEx_PreviewMouseUp;
        }

        private void DateBoxEx_Unloaded(object sender, RoutedEventArgs e)
        {
            Validation.RemoveErrorHandler(this, DateBoxEx_ValidationError);
            this.Unloaded -= this.DateBoxEx_Unloaded;
        }

        private void DateBoxEx_ValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Textプロパティに対するエラーが無くなっている場合は処理継続
            var expression = this.GetBindingExpression(TextProperty);

            if (expression.ValidationErrors.IsEmpty() || expression.ValidationErrors?.Count == 0)
            {
                e.Handled = true;
                // ここでも更新しておかないと99→.→99と入力したときにチェックされなくなる
                if (this.CheckedFlg)
                {
                    this.BeforeText = this.Text;
                }
                return;
            }

            // エラーが残っている場合はフォーカスをセット
            this.Dispatcher.InvokeAsync(() =>
            {
                var control = sender as DateBoxEx;
                control.Focus();

                // GoFocus→Converter→本メソッドの順に呼び出されている。
                // GoFocusでOldTextへコピーするとCoverterによる書式編集前の値がコピーされてしまい、
                // 6と入力して6.00に編集されるパターンで
                // 値が異なっていると判断されてしまうのでここでコピーする。
                if (this.CheckedFlg)
                {
                    this.BeforeText = this.Text;
                }
            });

            e.Handled = true;
        }

        private void GotKeyboardFocusInvoke()
        {
            if (this.IsReadOnly == false)
            {
                this.Text = this.Text?.Replace("/", string.Empty);
            }
            this.PartsTextBox.SelectAll();
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            ChangeHitTestVisible(true);
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            ChangeHitTestVisible(false);
        }
    }
}
