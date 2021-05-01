using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFCommon
{
    public class TimeBoxEx : ContentControl
    {
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TimeBoxEx), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnIsReadOnlyChanged)));

        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register(nameof(SelectedTime), typeof(Time), typeof(TimeBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(TimeBoxEx));

        public static readonly DependencyProperty SelectionTextBrushProperty =
            DependencyProperty.Register(nameof(SelectionTextBrush), typeof(Brush), typeof(TimeBoxEx));

        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(TimeBoxEx), new PropertyMetadata("HH:mm:ss"));

        // BindsTwoWayByDefaultを指定しておかないとthis.Text = hogehoge;という操作を行ったときにBindingがクリアされてしまう。
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TimeBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty WaterMarkStringProperty =
            DependencyProperty.Register(nameof(WaterMarkString), typeof(string), typeof(TimeBoxEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private ToggleButton _PartsButton = null;
        private Button _PartsCancelButton = null;
        private Button _PartsOkButton = null;
        private Popup _PartsPopup = null;
        private TimeSelecter _PartsSelecter = null;
        private TextBox _PartsTextBox = null;
        private string BeforeText = string.Empty;
        private bool CheckedFlg = false;

        static TimeBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeBoxEx), new FrameworkPropertyMetadata(typeof(TimeBoxEx)));
        }

        public TimeBoxEx()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // イベントハンドラの追加
            //this.Loaded += this.TextBoxBase_Loaded;
            this.Unloaded += this.TimeBoxEx_Unloaded;
            Validation.AddErrorHandler(this, TimeBoxEx_ValidationError);

            // 貼り付けコマンドのハンドラを追加
            //DataObject.AddPastingHandler(this, this.TextBoxBase_PastingHandler);
        }

        [Category("Text")]
        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        [Category("Text")]
        public string SelectedText
        {
            get { return this.PartsTextBox.SelectedText; }
            set { this.PartsTextBox.SelectedText = value; }
        }

        public Time SelectedTime
        {
            get { return (Time)this.GetValue(SelectedTimeProperty); }
            set { this.SetValue(SelectedTimeProperty, value); }
        }

        public Brush SelectionBrush
        {
            get { return (Brush)this.GetValue(SelectionBrushProperty); }
            set { this.SetValue(SelectionBrushProperty, value); }
        }

        public Brush SelectionTextBrush
        {
            get { return (Brush)this.GetValue(SelectionTextBrushProperty); }
            set { this.SetValue(SelectionTextBrushProperty, value); }
        }

        [Category("Text")]
        public string StringFormat
        {
            get { return (string)this.GetValue(StringFormatProperty); }
            set { this.SetValue(StringFormatProperty, value); }
        }

        [Category("Text")]
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public string WaterMarkString
        {
            get { return (string)this.GetValue(WaterMarkStringProperty); }
            set { this.SetValue(WaterMarkStringProperty, value); }
        }

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

        private Button PartsCancelButton
        {
            get
            {
                if (this._PartsCancelButton.IsEmpty())
                {
                    this._PartsCancelButton = GetTemplateChild("PART_CANCEL_Button") as Button;
                }
                return this._PartsCancelButton;
            }
        }

        private Button PartsOkButton
        {
            get
            {
                if (this._PartsOkButton.IsEmpty())
                {
                    this._PartsOkButton = GetTemplateChild("PART_OK_Button") as Button;
                }
                return this._PartsOkButton;
            }
        }

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

        private TimeSelecter PartsSelecter
        {
            get
            {
                if (this._PartsSelecter.IsEmpty())
                {
                    this._PartsSelecter = GetTemplateChild("PART_Selecter") as TimeSelecter;
                }
                return this._PartsSelecter;
            }
        }

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

        public override void OnApplyTemplate()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // 以下の２つのイベントはボタン押下でカレンダー開く→ボタン押下としたときに
            // MouseDownでカレンダーが閉じられ、MouseUpで再度カレンダーが開いてしまう問題に対応
            this.PartsPopup.Opened += this.Popup_Opened;
            this.PartsPopup.Closed += this.Popup_Closed;

            this.PartsOkButton.Click += this.PartsOkButton_Click;
            this.PartsCancelButton.Click += this.PartsCancelButton_Click;

            this.PartsButton.IsEnabled = !this.IsReadOnly;

            var expression = BindingOperations.GetBindingExpression(this, TextProperty);
            if (expression.IsEmpty())
            {
                return;
            }

            var binding = BindingOperations.GetBinding(this, TextProperty);
            if (binding.IsEmpty())
            {
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
            switch (x.PropertyType)
            {
                case Type intType when intType == typeof(Time):
                    displayFormat = expression.ParentBinding.StringFormat;
                    if (displayFormat.IsEmpty())
                    {
                        displayFormat = "HH:mm:ss";
                    }
                    newBinding.Converter = new StringToTimeConverter();
                    newBinding.ValidationRules.Add(new TimeValidationRule());
                    break;

                default:
                    break;
            }
            newBinding.ConverterParameter = displayFormat;
            newBinding.StringFormat = displayFormat;
            BindingOperations.ClearBinding(this, TextProperty);
            BindingOperations.SetBinding(this, TextProperty, newBinding);

            base.OnApplyTemplate();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            this.Dispatcher.InvokeAsync(this.GotKeyboardFocusInvoke);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            this.PreviewMouseUp += this.TimeBoxEx_PreviewMouseUp;
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.IsReadOnly)
            {
                //e.Handled = true; // これがあるとフォーカスが外れなくなる
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

            if (obj is TimeBoxEx ctrl)
            {
                if (ctrl.PartsButton.IsEmpty() == false)
                {
                    ctrl.PartsButton.IsEnabled = !IsReadOnly;
                }
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
        private bool CheckInputText(TimeBoxEx sender, string inputText)
        {
            if (".0123456789".Contains(inputText) == false)
            {
                return false;
            }

            //var target = sender as TimeBoxEx;

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

        private void GotKeyboardFocusInvoke()
        {
            if (this.IsReadOnly == false)
            {
                this.Text = this.Text?.Replace(":", string.Empty);
            }
            this.PartsTextBox.SelectAll();
        }

        private void PartsCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.PartsPopup.IsOpen = false;
        }

        private void PartsOkButton_Click(object sender, RoutedEventArgs e)
        {
            this.PartsPopup.IsOpen = false;

            this.SelectedTime = this.PartsSelecter.SelectedTime;
            this.Text = this.SelectedTime?.ToString(StringFormat);
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            ChangeHitTestVisible(true);
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            ChangeHitTestVisible(false);
        }

        private void TimeBoxEx_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.InvokeAsync(this.GotKeyboardFocusInvoke);
            this.ReleaseMouseCapture();
            // この記述があると２回目にマウスクリックしたときに画面が固まってしまう
            //this.CaptureMouse();
            this.PreviewMouseUp -= this.TimeBoxEx_PreviewMouseUp;
        }

        private void TimeBoxEx_Unloaded(object sender, RoutedEventArgs e)
        {
            Validation.RemoveErrorHandler(this, TimeBoxEx_ValidationError);
            this.Unloaded -= this.TimeBoxEx_Unloaded;
        }

        private void TimeBoxEx_ValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Textプロパティに対するエラーが無くなっている場合は処理継続
            var expression = this.GetBindingExpression(TextProperty);

            if (expression.ValidationErrors.IsEmpty() || expression.ValidationErrors?.Count == 0)
            {
                e.Handled = true;
                return;
            }

            // エラーが残っている場合はフォーカスをセット
            this.Dispatcher.InvokeAsync(() =>
            {
                var control = sender as TimeBoxEx;
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
    }
}
