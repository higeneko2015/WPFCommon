using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFCommon
{
    /// <summary>
    /// テキストボックスの基底となるクラス
    /// このコントロールを直接使用してはいけません。
    /// </summary>
    [DesignTimeVisible(false)]
    public class TextBoxBase : TextBox
    {
        public static readonly DependencyProperty WaterMarkStringProperty =
            DependencyProperty.Register(nameof(WaterMarkString), typeof(string), typeof(TextBoxBase), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// 入力された文字の有効性をチェックするデリゲート変数
        /// OnPreviewTextInputイベント時に実行される。
        /// </summary>
        public CheckInputTextDelegate CheckInputCharacterHandler = null;

        /// <summary>
        /// 貼り付けされた文字の有効性をチェックするデリゲート変数
        /// TextBoxBase_PastingHandlerで実行される。
        /// </summary>
        public CheckInputTextDelegate CheckPasteCharacterHandler = null;

        /// <summary>
        /// キーボードフォーカス取得時の最後に実行するアクション定義
        /// </summary>
        public Action GotKeybordFocusInvokeHandler = null;

        /// <summary>
        /// 表示用の書式文字列
        /// </summary>
        protected string Format = string.Empty;

        /// <summary>
        /// 貼付時にクリップボードから取得した文字列から強制的に除去する文字の一覧
        /// </summary>
        protected string[] RemovePastingCharacters = null;

        /// <summary>
        /// 入力値の変更有無を判定するために使用する入力前値
        /// </summary>
        private string BeforeText = string.Empty;

        /// <summary>
        /// 入力値チェックを実施済みか判定するためのフラグ
        /// このフラグがTrueの時に入力値がBeforeTextへ退避される
        /// </summary>
        private bool CheckedFlg = false;

        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static TextBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxBase), new FrameworkPropertyMetadata(typeof(TextBoxBase)));
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        public TextBoxBase()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // TextWrappingProperty.OverrideMetadataを行ってもデザイナ上で反映されないのでここで設定
            this.TextWrapping = TextWrapping.NoWrap;

            // Spaceキーの入力をPreviewTextInputでハンドリングできるようにする
            this.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.Space, ModifierKeys.None));
            this.InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.Space, ModifierKeys.Shift));

            // アプリケーション終了時にこのイベントは起きない
            //this.Unloaded += this.TextBoxBase_Unloaded;

            // Validationエラーが発生したときにWPFフレームワークから呼び出されるハンドラを登録
            Validation.AddErrorHandler(this, this.TextBoxBase_ValidationError);

            // 貼り付けコマンド用のハンドラを登録
            DataObject.AddPastingHandler(this, this.TextBoxBase_PastingHandler);
        }

        /// <summary>
        /// 入力された文字の有効性をチェックするメソッドのデリゲート定義
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="inputText">入力文字</param>
        /// <returns>入力可能(True)、入力不可(False)</returns>
        public delegate bool CheckInputTextDelegate(object sender, string inputText);

        /// <summary>
        /// テキストボックスが未入力時に表示するすかし文字を取得または設定します。
        /// </summary>
        public string WaterMarkString
        {
            get { return (string)this.GetValue(WaterMarkStringProperty); }
            set { this.SetValue(WaterMarkStringProperty, value); }
        }

        /// <summary>
        /// GotKeyboardFocusイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            // 派生先クラス側のイベントを処理してから最後に全選択させる
            base.OnGotKeyboardFocus(e);

            this.SelectAllText();
        }

        /// <summary>
        /// LostKeyboardFocusイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            this.PreviewMouseUp += this.TextBoxBase_PreviewMouseUp;
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.CheckedFlg = false;

            // 画面を閉じるボタンを押下した場合は入力値のチェックしない
            // TODO Button03という名称は変更した方がよい
            var newFocusControl = e.NewFocus as Button;
            if (newFocusControl?.Name == "Button03")
            {
                e.Handled = true;
                return;
            }

            // 入力値に変更が無い場合は処理しない
            if (this.Text == this.BeforeText)
            {
                return;
            }

            // キーボードフォーカスが無い場合は処理しない
            if (this.IsKeyboardFocusWithin == false)
            {
                base.OnPreviewLostKeyboardFocus(e);
                return;
            }

            this.CheckedFlg = true;

            var expression = GetBindingExpression(this, TextProperty);

            // Validationを実施
            expression?.UpdateSource();
            if (expression?.ValidationErrors?.Count > 0)
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewLostKeyboardFocus(e);
        }

        /// <summary>
        /// キーボードのキー押下時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (this.CheckInputCharacterHandler.IsEmpty())
            {
                // 押下可能文字をチェックするハンドラが定義されていない場合は
                // 全文字入力可能なので処理を継続する。
                base.OnPreviewTextInput(e);
                return;
            }

            // 押下されたキーが入力可能かチェックする
            var ret = this.CheckInputCharacterHandler(this, e.Text);
            if (ret == false)
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewTextInput(e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        private BindingExpression GetBindingExpression(Control target, DependencyProperty dp)
        {
            var expression = BindingOperations.GetBindingExpression(target, dp);

            // TemplateBindingを使用している時は子コントロールのBindingはnullになる。
            // そのため親TemplateのBinding情報から取得する。
            if (expression.IsEmpty() && target.TemplatedParent.IsEmpty() == false)
            {
                expression = BindingOperations.GetBindingExpression(target.TemplatedParent, dp);
            }
            return expression;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 積極的にinline化されるように。
        private void SelectAllText()
        {
            if (this.GotKeybordFocusInvokeHandler.IsEmpty() == false)
            {
                this.Dispatcher.InvokeAsync(this.GotKeybordFocusInvokeHandler, DispatcherPriority.ApplicationIdle);
            }
        }

        /// <summary>
        /// クリップボードからの貼付時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void TextBoxBase_PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            // クリップボードの内容がテキスト以外の場合は受け付けない
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (isText == false)
            {
                return;
            }

            // クリップボードからテキストを取得
            var pasteText = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            // 特定の文字を除去(NumberBoxEx時のカンマ)
            if (this.RemovePastingCharacters.IsEmpty() == false)
            {
                foreach (var item in this.RemovePastingCharacters.AsSpan())
                {
                    pasteText = pasteText.Replace(item, string.Empty);
                }
            }

            // 貼り付け可能な内容かをチェック
            if (this.CheckPasteCharacterHandler.IsEmpty() == false)
            {
                var ret = this.CheckPasteCharacterHandler(this, pasteText);
                if (ret == false)
                {
                    e.Handled = true;
                    e.CancelCommand();
                    return;
                }
            }

            e.Handled = true;
            e.CancelCommand();

            // テキストが選択されている場合はその部分を置換、そうで無ければカーソルのある位置に挿入
            var target = sender as TextBox;
            var generateText = target.Text;

            if (target.SelectedText.Length > 0)
            {
                // 選択済みのテキストがある場合はそれを除去する
                generateText = generateText.Remove(target.SelectionStart, target.SelectionLength);
            }

            // カーソル位置にテキストを挿入する
            generateText = generateText.Insert(target.CaretIndex, pasteText);

            target.Text = generateText;
            target.Select(target.Text.Length, 0);
        }

        /// <summary>
        /// PreviewMouseUpイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void TextBoxBase_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // マウスクリックでフォーカスを取得したときにテキストを全選択する。
            this.SelectAllText();

            this.ReleaseMouseCapture();
            this.CaptureMouse();
            this.PreviewMouseUp -= this.TextBoxBase_PreviewMouseUp;
        }

        /// <summary>
        /// Unloadedイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        //private void TextBoxBase_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    this.Unloaded -= this.TextBoxBase_Unloaded;
        //    DataObject.RemovePastingHandler(this, this.TextBoxBase_PastingHandler);
        //}

        /// <summary>
        /// Validationのエラー状態に変更が発生したときに実行されるハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxBase_ValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Textプロパティに対するエラーが無くなっている場合は処理継続
            var expression = this.GetBindingExpression(this, TextProperty);

            if (expression.ValidationErrors.IsEmpty() || expression.ValidationErrors?.Count == 0)
            {
                // 入力値にエラーが無い状態なので処理を継続させる。
                e.Handled = true;
                if (this.CheckedFlg)
                {
                    this.BeforeText = this.Text;
                }
                return;
            }

            // エラーが残っている場合はフォーカスをセット
            this.Dispatcher.InvokeAsync(() =>
            {
                var control = sender as TextBox;
                control.Focus();

                // GoFocus→Converter→本メソッドの順に呼び出されている。
                // GoFocusでOldTextへコピーするとCoverterによる書式編集前の値が
                // コピーされてしまい、6と入力して6.00に編集されるパターンで
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