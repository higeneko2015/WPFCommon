using System.ComponentModel;
using System.Windows;

namespace WPFCommon
{
    /// <summary>
    /// テキストボックスの基底クラス
    /// </summary>
    public class TextBoxEx : TextBoxBase
    {
        /// <summary>
        /// 入力可能文字種を管理する依存プロパティ
        /// </summary>
        public static readonly DependencyProperty InputableCharacterProperty =
            DependencyProperty.Register(nameof(InputableCharacter), typeof(string), typeof(TextBoxEx), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 入力不可能文字種を管理する依存プロパティ
        /// </summary>
        public static readonly DependencyProperty UnInputableCharacterProperty =
            DependencyProperty.Register(nameof(UnInputableCharacter), typeof(string), typeof(TextBoxEx), new PropertyMetadata(string.Empty));

        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static TextBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxEx), new FrameworkPropertyMetadata(typeof(TextBoxEx)));
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        public TextBoxEx()
        {
            this.CheckInputCharacterHandler += this.CheckInputText;
            this.GotKeybordFocusInvokeHandler += this.GotKeyboardFocusInvoke;
            this.Unloaded += this.TextBoxEx_Unloaded;
        }

        /// <summary>
        /// 入力可能な文字種を取得または設定します。
        /// </summary>
        [Category("WPFCommon個別"), Description("入力可能な文字種を取得または設定します。")]
        public string InputableCharacter
        {
            get { return (string)this.GetValue(InputableCharacterProperty); }
            set { this.SetValue(InputableCharacterProperty, value); }
        }

        /// <summary>
        /// 入力不可能文字種を取得または設定します。
        /// </summary>
        [Category("WPFCommon個別"), Description("入力不可能文字種を取得または設定します。")]
        public string UnInputableCharacter
        {
            get { return (string)this.GetValue(UnInputableCharacterProperty); }
            set { this.SetValue(UnInputableCharacterProperty, value); }
        }

        /// <summary>
        /// 入力値の有効性を判定するメソッド
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="inputText">入力文字</param>
        /// <returns>
        /// 有効な入力値の場合：true
        /// 無効な入力値の場合：false
        /// </returns>
        private bool CheckInputText(object sender, string inputText)
        {
            var target = sender as TextBoxEx;

            if (target.UnInputableCharacter.IndexOf(inputText) != -1)
            {
                return false;
            }

            if (target.InputableCharacter.IsEmpty())
            {
                return true;
            }

            if (target.InputableCharacter.Contains(inputText) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// キーボードフォーカス取得時の最後に実行されるメソッド
        /// </summary>
        private void GotKeyboardFocusInvoke()
        {
            this.SelectAll();
        }

        /// <summary>
        /// Unloadedイベント発生時に実行されるイベントハンドラ
        /// </summary>
        /// <param name="sender">呼び出し元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void TextBoxEx_Unloaded(object sender, RoutedEventArgs e)
        {
            this.CheckInputCharacterHandler -= this.CheckInputText;
            this.GotKeybordFocusInvokeHandler -= this.GotKeyboardFocusInvoke;
            this.Unloaded -= this.TextBoxEx_Unloaded;
        }
    }
}